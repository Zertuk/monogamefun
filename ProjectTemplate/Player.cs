using System;
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
        float _moveSpeed = 100f;

        VirtualButton _fireInput;
        VirtualIntegerAxis _xAxisInput;
        VirtualIntegerAxis _yAxisInput;
        int tileSize = 16;
        int scale = 2;
        double jumpTime;
        int health;
        int maxHealth;
        float jumpGrav;
        public bool grounded;
        bool ignoreGravity;



        private void DisplayHealthBar()
        {
        }

        private void DisplayPosition()
        {
            var myScene = entity.scene as GameScene;
            myScene.UpdateScene();
        }

        public override void onAddedToEntity()
        {
            jumpGrav = 0;
            ignoreGravity = false;
            health = 10;
            maxHealth = 10;
            var texture = entity.scene.contentManager.Load<Texture2D>("leekrun");
            var subtextures = Subtexture.subtexturesFromAtlas(texture, 20, 21);
            jumpTime = 1;
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
                subtextures[0]
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
                subtextures[0],
                subtextures[1],
                subtextures[2],
                subtextures[3],
            }));

            setupInput();
        }


        public override void onRemovedFromEntity()
        {
            // deregister virtual input
            _fireInput.deregister();
        }


        void setupInput()
        {
            // setup input for shooting a fireball. we will allow z on the keyboard or a on the gamepad
            _fireInput = new VirtualButton();
            _fireInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.Z));
            _fireInput.nodes.Add(new Nez.VirtualButton.GamePadButton(0, Buttons.A));

            // horizontal input from dpad, left stick or keyboard left/right
            _xAxisInput = new VirtualIntegerAxis();
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadDpadLeftRight());
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadLeftStickX());
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

            // vertical input from dpad, left stick or keyboard up/down
            _yAxisInput = new VirtualIntegerAxis();
            _yAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadDpadUpDown());
            _yAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadLeftStickY());
            _yAxisInput.nodes.Add(new Nez.VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
        }

        private double Jump()
        {
            if (_yAxisInput != 0 || jumpTime < 1 && jumpTime > 0.5)
            {
                if (jumpTime > 0)
                {
                    jumpGrav = jumpGrav + 0.01f;
                    double y = -(3)*0.75 + (jumpGrav*jumpTime);
                    jumpTime = jumpTime - 0.1;
                    return y;
                }
            }
            else
            {
                jumpTime = 0;
            }
            jumpGrav = 0;
            return 0;
        }

        void IUpdatable.update()
        {
            DisplayPosition();
            DisplayHealthBar();
            // handle movement and animations
            var moveDir = new Vector2(_xAxisInput.value, 0);
            if (grounded || jumpTime < 1)
            {
                moveDir.Y = (float)Jump();
            }
            var animation = Animations.Idle;

            if (moveDir.X < 0)
            {
                animation = Animations.Walk;
                _animation.flipX = false;
            }
            else if (moveDir.X > 0)
            {
                animation = Animations.Run;
                _animation.flipX = true;
            }

            if (moveDir.Y < 0)
            {
                animation = Animations.Falling;
                grounded = false;
            }
            else if (moveDir.Y > 0)
            {
                animation = Animations.Jumping;
                grounded = false;
            }
                
            CollisionResult res;
            var movement = moveDir * _moveSpeed * Time.deltaTime;
            _mover.move(movement, out res);

            if (moveDir != Vector2.Zero)
            {
                if (!_animation.isAnimationPlaying(animation))
                    _animation.play(animation);

            }
            else
            {
                _animation.stop();
            }

            if (grounded)
            {
                jumpTime = 1;
                jumpGrav = 0;
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

