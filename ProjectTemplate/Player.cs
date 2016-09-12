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
        public enum State
        {
            Normal,
            Attack,
            Roll,
            Knockback
        }

        enum Animations
        {
            Walk,
            Run,
            RunRev,
            Idle,
            Attack,
            Attack2,
            Attack3,
            Death,
            Falling,
            Hurt,
            Jumping,
            Rolling,
            Knockback
        }

        private Sprite<Animations> _animation;
        private Mover _mover;
        private float _moveSpeed = 70f;
        public State activeState;
        public bool Invuln;

        private VirtualButton _jumpInput;
        private VirtualButton _rollInput;
        private VirtualButton _attackInput;
        private VirtualIntegerAxis _xAxisInput;

        private int _jumpTime;

        public int Health;
        public int MaxHealth;
        public bool Grounded;
        public bool IsRolling;
        public int DropCount;
        
        private int _groundFrames;
        private bool _hasJumped;
        private int _rollCount;
        private int _maxJumpTime;
        private int _attackTimer;
        private int _knockbackTimer;

        private bool _secondAttack;
        private bool _thirdAttack;
        private int _actionTimer;
        
        private void DisplayPosition()
        {
            var myScene = entity.scene as GameScene;
            myScene.UpdateScene();
        }

        public override void onAddedToEntity()
        {
            DropCount = 0;
            Health = 10;
            MaxHealth = 10;
            IsRolling = false;

            _actionTimer = 0;
            _attackTimer = 0;
            _groundFrames = 0;
            _hasJumped = false;
            _maxJumpTime = 20;
            _jumpTime = _maxJumpTime;
            _mover = entity.addComponent(new Mover());


            var texture = entity.scene.contentManager.Load<Texture2D>("leekrun-sheet");
            var textureRev = entity.scene.contentManager.Load<Texture2D>("leekrun-sheet2");
            var idleTexture = entity.scene.contentManager.Load<Texture2D>("leekidle-sheet");
            var fallTexture = entity.scene.contentManager.Load<Texture2D>("leekfall");
            var jumpTexture = entity.scene.contentManager.Load<Texture2D>("leekjump");
            var attackTexture = entity.scene.contentManager.Load<Texture2D>("leekattack-sheet");
            var rollTexture = entity.scene.contentManager.Load<Texture2D>("leekroll");
            

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 50, 50);
            var subtexturesRev = Subtexture.subtexturesFromAtlas(textureRev, 50, 50);
            var idleSubtexture = Subtexture.subtexturesFromAtlas(idleTexture, 50, 50);
            var fallSubtexture = Subtexture.subtexturesFromAtlas(fallTexture, 20, 21);
            var jumpSubtexture = Subtexture.subtexturesFromAtlas(jumpTexture, 20, 21);
            var attackSubtexture = Subtexture.subtexturesFromAtlas(attackTexture, 50, 50);
            var rollSubtexture = Subtexture.subtexturesFromAtlas(rollTexture, 20, 21);

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

            _animation.addAnimation(Animations.RunRev, new SpriteAnimation(new List<Subtexture>()
            {
                subtexturesRev[0],
                subtexturesRev[0],
                subtexturesRev[1],
                subtexturesRev[1],
                subtexturesRev[2],
                subtexturesRev[2],
                subtexturesRev[3],
                subtexturesRev[3]
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
                attackSubtexture[3],
                attackSubtexture[3]
            }));

            _animation.addAnimation(Animations.Attack2, new SpriteAnimation(new List<Subtexture>()
            {
                attackSubtexture[3],
                attackSubtexture[4],
                attackSubtexture[5],
                attackSubtexture[6],
                attackSubtexture[6],
                attackSubtexture[6]
            }));

            _animation.addAnimation(Animations.Attack3, new SpriteAnimation(new List<Subtexture>()
            {
                attackSubtexture[7],
                attackSubtexture[8],
                attackSubtexture[9],
                attackSubtexture[10],
                attackSubtexture[10]
            }));

            _animation.addAnimation(Animations.Death, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1],
                subtextures[2],
                subtextures[3]
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
                rollSubtexture[1],
                rollSubtexture[2],
                rollSubtexture[2]
            }));

            setupInput();
        }

        private void StateMachine()
        {
            switch(activeState)
            {
                case State.Normal:
                    DoNormal();
                    break;
                case State.Attack:
                    DoAttack();
                    break;
                case State.Roll:
                    DoRoll();
                    break;
                case State.Knockback:
                    DoKnockback();
                    break;
            }
        }

        public void UpdateDropCount(int val)
        {
            DropCount = DropCount + val;
        }

        public void DoKnockback()
        {
            var moveDir = new Vector2(2, -1);
            if (_animation.flipX)
            {
                moveDir.X = moveDir.X * -1;
            }

            _knockbackTimer = _knockbackTimer + 1;
            Console.WriteLine(_knockbackTimer);
            if (_knockbackTimer > 6)
            {
                _knockbackTimer = 0;
                activeState = State.Normal;
            }
            DoMovement(moveDir, Animations.Run);

        }

        private void DoNormal()
        {
            var moveDir = new Vector2(_xAxisInput.value, 0);
            var animation = Animations.Idle;
            _actionTimer = _actionTimer + 1;
            //jump
            if (CheckJumpInput())
            {
                moveDir.Y = Jump();
                if (moveDir.Y >= 0)
                {
                    _hasJumped = true;
                }
            }

            if (_actionTimer > 5)
            {
                //roll
                if (_rollInput.isPressed)
                {
                    activeState = State.Roll;
                }

                if (_attackInput.isPressed)
                {
                    activeState = State.Attack;
                }
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

            DoMovement(moveDir, animation);
        }

        private void DoAttack()
        {
            var animation = Animations.Attack;
            _attackTimer = _attackTimer + 1;

            if (22 >= _attackTimer && _attackTimer > 10)
            {
                if (_attackInput.isPressed)
                {
                    _secondAttack = true;                  
                }
            }

            //kill
            if (_attackTimer >= 18)
            {
                if (!_secondAttack && _attackTimer >= 22)
                {
                    ExitAttack();
                }
                else if (_secondAttack)
                {
                    animation = Animations.Attack2;
                }
            }


            if ((40 >= _attackTimer && _attackTimer > 26))
            {
                if (_attackInput.isPressed)
                {
                    _thirdAttack = true;
                }
            }

            if (_attackTimer >= 36)
            {
                if (!_thirdAttack && _attackTimer >= 40)
                {
                    ExitAttack();
                }
                else if (_thirdAttack)
                {
                    animation = Animations.Attack3;
                }
            }

            if (_attackTimer > 54)
            {
                animation = Animations.Attack3;
                ExitAttack();
            }

            var moveDir = new Vector2(0, 0);
            DoMovement(moveDir, animation);
        }

        private void ExitAttack()
        {
            _secondAttack = false;
            _thirdAttack = false;
            activeState = State.Normal;
            _attackTimer = 0;
            _actionTimer = 0;
        }

        public void DoHurt(int damage)
        {
            Health = Health - damage;
        }

        private void DoRoll()
        {
            var moveDir = new Vector2(0, 0);
            var animation = Animations.Rolling;
            _rollCount = _rollCount + 1;
            if (_rollCount > 15)
            {
                activeState = State.Normal;
                _rollCount = 0;
            }
            if (_animation.flipX)
            {
                moveDir.X = 2;
            }
            else
            {
                moveDir.X = -2;
            }
            _actionTimer = 0;
            DoMovement(moveDir, animation);
        }

        private void DoMovement(Vector2 moveDir, Animations animation)
        {
            CollisionResult res;
            var movement = (moveDir * _moveSpeed * Time.deltaTime);
            _mover.move(movement, out res);
            if (!_animation.isAnimationPlaying(animation))
            {
                _animation.play(animation);
            }
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

            _attackInput = new VirtualButton();
            _attackInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.F));
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
            if ((_jumpInput.isDown && !_hasJumped && _jumpTime != 0) || (_jumpInput.isDown && !_hasJumped && _jumpTime != _maxJumpTime))
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
            StateMachine();


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

