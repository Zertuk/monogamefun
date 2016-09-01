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
            Jumping,
            Rolling
        }

        private Sprite<Animations> _animation;
        private Mover _mover;
        private float _moveSpeed = 76f;

        private VirtualButton _jumpInput;
        private VirtualButton _rollInput;
        private VirtualIntegerAxis _xAxisInput;

        private int _tileSize = 16;
        private int _jumpTime;

        public int Health;
        public int MaxHealth;
        public bool Grounded;

        private int _groundFrames;
        private bool _hasJumped;
        private bool _isRolling;
        private int _rollCount;
        private int _maxJumpTime;

        private void DisplayPosition()
        {
            var myScene = entity.scene as GameScene;
            myScene.UpdateScene();
        }

        public override void onAddedToEntity()
        {
            _groundFrames = 0;
            _hasJumped = false;
            _maxJumpTime = 20;
            Health = 10;
            MaxHealth = 10;

            var texture = entity.scene.contentManager.Load<Texture2D>("leekrun4");
            var idleTexture = entity.scene.contentManager.Load<Texture2D>("leekidle");
            var fallTexture = entity.scene.contentManager.Load<Texture2D>("leekfall");
            var jumpTexture = entity.scene.contentManager.Load<Texture2D>("leekjump");
            var attackTexture = entity.scene.contentManager.Load<Texture2D>("leekattack2");
            var rollTexture = entity.scene.contentManager.Load<Texture2D>("leekroll");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 21, 21);
            var idleSubtexture = Subtexture.subtexturesFromAtlas(idleTexture, 20, 21);
            var fallSubtexture = Subtexture.subtexturesFromAtlas(fallTexture, 20, 21);
            var jumpSubtexture = Subtexture.subtexturesFromAtlas(jumpTexture, 20, 21);
            var attackSubtexture = Subtexture.subtexturesFromAtlas(attackTexture, 42, 30);
            var rollSubtexture = Subtexture.subtexturesFromAtlas(rollTexture, 20, 21);
            _jumpTime = _maxJumpTime;
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
                subtextures[0],
                subtextures[1],
                subtextures[1],
                subtextures[2],
                subtextures[2],
                subtextures[3],
                subtextures[3]
            }));

            _animation.addAnimation(Animations.Idle, new SpriteAnimation(new List<Subtexture>()
            {
                idleSubtexture[0],
                idleSubtexture[0],
                idleSubtexture[1],
                idleSubtexture[1],
                idleSubtexture[2],
                idleSubtexture[2]
                //attackSubtexture[0],
                //attackSubtexture[1],
                //attackSubtexture[2],
                //attackSubtexture[3]

            }));

            _animation.addAnimation(Animations.Attack, new SpriteAnimation(new List<Subtexture>()
            {
                attackSubtexture[0],
                attackSubtexture[1],
                attackSubtexture[2],
                attackSubtexture[3],
                attackSubtexture[3]
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
            _animation.addAnimation(Animations.Rolling, new SpriteAnimation(new List<Subtexture>()
            {
                rollSubtexture[0],
                rollSubtexture[0],
                rollSubtexture[1],
                rollSubtexture[1],
                rollSubtexture[2],
                rollSubtexture[2],
                rollSubtexture[2],
                rollSubtexture[2]
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

            _rollInput = new VirtualButton();
            _rollInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.Q));
            _rollInput.nodes.Add(new Nez.VirtualButton.GamePadButton(0, Buttons.X));
        }

        private float Jump()
        {
            var count = 0;
            if (_jumpTime > 0)
            {
                count = count + 1;
                var x = 2.5f;
                float y = 1f - (0.5f* x*x);
                _jumpTime = _jumpTime - 1;
                return y;
            }
            count = 0;
            _jumpTime = 0;
            return 0;
        }

        private bool CheckJumpInput()
        {
            if ((_jumpInput.isDown && !_hasJumped && _jumpTime != 0))
            {
                return true;
            }
            return false;
        }

        void IUpdatable.update()
        {
            //dev
            DisplayPosition();


            //keep
            var moveDir = new Vector2(_xAxisInput.value, 0);
            var animation = Animations.Idle;

            //jump
            if (CheckJumpInput())
            {
                moveDir.Y = (float)Jump();
            }
            else if (_jumpTime < 5)
            {
                _hasJumped = true;
            }

            //run
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

            //grounded
            if (Grounded)
            {
                _groundFrames = _groundFrames + 1;
            }
            else
            {
                _groundFrames = 0;
            }

            //fall/jumping
            if (moveDir.Y > 0)
            {
                animation = Animations.Falling;
                Grounded = false;
            }
            else if (moveDir.Y < 0)
            {
                animation = Animations.Jumping;
                Grounded = false;
            }

            //idle
            if (moveDir.X == 0 && moveDir.Y == 0)
            {
                animation = Animations.Idle;
            }

            //roll
            if (_rollInput || _isRolling)
            {
                _isRolling = true;
                animation = Animations.Rolling;
                _rollCount = _rollCount + 1;

                if (_rollCount > 30)
                {
                    _isRolling = false;
                    _rollCount = 0;
                }
                else
                {
                    if (_animation.flipX)
                    {
                        moveDir.X = 1;
                    }
                    else
                    {
                        moveDir.X = -1;
                    }
                }
            }

            CollisionResult res;
            var movement = (moveDir * _moveSpeed * Time.deltaTime);
            _mover.move(movement, out res);

            if (!_animation.isAnimationPlaying(animation))
            {
                _animation.play(animation);
            }

            if (Grounded)
            {
                _jumpTime = _maxJumpTime;
                if (!_jumpInput.isDown)
                {
                    _hasJumped = false;
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

