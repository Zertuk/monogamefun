﻿using System;
using Microsoft.Xna.Framework;
using Nez.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;
using Nez.AI.FSM;

namespace ProjectTemplate
{
    public class Player : Component, ITriggerListener, IUpdatable
    {
        enum Animations
        {
            Walk,
            Run,
            Idle,
            Attack,
            Death,
            Falling,
            Hurt,
            Jumping
        }

        Sprite<Animations> _animation;
        Mover _mover;
        float _moveSpeed = 76f;

        VirtualButton _jumpInput;
        VirtualIntegerAxis _xAxisInput;
        int tileSize = 16;
        int jumpTime;
        int health;
        int maxHealth;
        float jumpGrav;
        public bool grounded;
        int groundFrames;
        bool hasJumped;
        public bool enableGravity;

        private void DisplayPosition()
        {
            var myScene = entity.scene as GameScene;
            myScene.UpdateScene();
        }

        public override void onAddedToEntity()
        {
            groundFrames = 0;
            hasJumped = false;
            health = 10;
            maxHealth = 10;
            var texture = entity.scene.contentManager.Load<Texture2D>("leekrun3");
            var idleTexture = entity.scene.contentManager.Load<Texture2D>("leekidle");
            var fallTexture = entity.scene.contentManager.Load<Texture2D>("leekfall");
            var jumpTexture = entity.scene.contentManager.Load<Texture2D>("leekjump");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 20, 21);
            var idleSubtexture = Subtexture.subtexturesFromAtlas(idleTexture, 20, 21);
            var fallSubtexture = Subtexture.subtexturesFromAtlas(fallTexture, 20, 21);
            var jumpSubtexture = Subtexture.subtexturesFromAtlas(jumpTexture, 20, 21);
            jumpTime = 20;
            _mover = entity.addComponent(new Mover());
            _animation = entity.addComponent(new Sprite<Animations>(subtextures[0]));
            //extract the animations from the atlas. they are setup in rows with 8 columns
            _animation.addAnimation(Animations.Walk, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1],
                subtextures[2],
                subtextures[3],
            }));

            _animation.addAnimation(Animations.Run, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1],
                subtextures[2],
                subtextures[3],
            }));

            _animation.addAnimation(Animations.Idle, new SpriteAnimation(new List<Subtexture>()
            {
                idleSubtexture[0],
                idleSubtexture[0],
                idleSubtexture[1],
                idleSubtexture[1],
                idleSubtexture[2],
                idleSubtexture[2]
            }));

            _animation.addAnimation(Animations.Attack, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1],
                subtextures[2],
                subtextures[3],
            }));

            _animation.addAnimation(Animations.Death, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1],
                subtextures[2],
                subtextures[3],
            }));

            _animation.addAnimation(Animations.Falling, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0]
            }));

            _animation.addAnimation(Animations.Hurt, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1]
            }));
            _animation.addAnimation(Animations.Jumping, new SpriteAnimation(new List<Subtexture>()
            {
                jumpSubtexture[0]
            }));
            _animation.addAnimation(Animations.Falling, new SpriteAnimation(new List<Subtexture>()
            {
                fallSubtexture[0]
            }));
            setupInput();
        }

        void setupInput()
        {
            // horizontal input from dpad, left stick or keyboard left/right
            _xAxisInput = new VirtualIntegerAxis();
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadDpadLeftRight());
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadLeftStickX());
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

            // vertical input from dpad, left stick or keyboard up/down
            _jumpInput = new VirtualButton();
            _jumpInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.A));
            _jumpInput.nodes.Add(new Nez.VirtualButton.GamePadButton(0, Buttons.A));
        }

        private float Jump()
        {
            var count = 0;
            if (jumpTime > 0)
            {
                count = count + 1;
                var x = 2.5f;
                float y = 1f - (0.5f* x*x);
                jumpTime = jumpTime - 1;
                return y;
            }
            count = 0;
            jumpTime = 0;
            return 0;
        }

        void IUpdatable.update()
        {
            DisplayPosition();

            // handle movement and animations
            var moveDir = new Vector2(_xAxisInput.value, 0);
            
            if ((_jumpInput.isDown && !hasJumped && jumpTime != 0))
            {
                moveDir.Y = (float)Jump();
            }
            else if (jumpTime < 0.5)
            {
                hasJumped = true;
            }

            var animation = Animations.Idle;

            if (moveDir.X < 0)
            {
                animation = Animations.Run;
                _animation.flipX = false;
            }
            else if (moveDir.X > 0)
            {
                animation = Animations.Run;
                _animation.flipX = true;
            }
            if (grounded)
            {
                    groundFrames = groundFrames + 1;
            }
            else
            {
                groundFrames = 0;
                Console.WriteLine(Time.deltaTime);
            }

            if (moveDir.Y > 0)
            {
                animation = Animations.Falling;
                grounded = false;
            }
            else if (moveDir.Y < 0)
            {
                animation = Animations.Jumping;
                grounded = false;
            }
            Console.WriteLine(moveDir.X);
            Console.WriteLine(moveDir.Y);
            if (moveDir.X == 0 && moveDir.Y == 0)
            {
                Console.WriteLine("test");
                animation = Animations.Idle;
            }

            CollisionResult res;
            var movement = (moveDir * _moveSpeed * Time.deltaTime);
            Console.WriteLine(movement.X);
            _mover.move(movement, out res);

            if (!_animation.isAnimationPlaying(animation))
            {
                _animation.play(animation);
            }

            if (grounded)
            {
                jumpTime = 20;
                if (!_jumpInput.isDown)
                {
                    hasJumped = false;
                }
            }
        }


        #region ITriggerListener implementation

        void ITriggerListener.onTriggerEnter(Collider other, Collider self)
        {
            Debug.log("triggerEnter: {0}", other.entity.name);
        }


        void ITriggerListener.onTriggerExit(Collider other, Collider self)
        {
            Debug.log("triggerExit: {0}", other.entity.name);
        }

        #endregion

    }
}

