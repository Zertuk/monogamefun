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
            Knockback,
            Float,
            Climb,
            Bounce
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
            Knockback,
            Float,
            Climb
        }

        private Sprite<Animations> _animation;
        private Mover _mover;
        private float _moveSpeed = 70f;
        public State activeState;

        public bool IsFlipped = false;
        public bool IsInvuln = false;
        public bool ShouldBounce = false;
        private bool _isBouncing = false;
        private int _invulnCount = 0;

        private VirtualButton _jumpInput;
        private VirtualButton _rollInput;
        private VirtualButton _floatInput;
        private VirtualButton _attackInput;
        private VirtualIntegerAxis _xAxisInput;
        public VirtualIntegerAxis YAxisInput;

        private int _jumpTime;

        public Vector2 HitboxOffset;
        public Vector2 HitboxOffsetFlip;
        public Collider LadderInUse;
        public int Health;
        public int MaxHealth;
        public bool Grounded;
        public bool IsRolling;
        public int DropCount;
        public bool ThirdAttack;
        public bool IgnoreGravity;


        private int _groundFrames;
        private bool _hasJumped;
        private int _rollCount;
        private int _maxJumpTime;
        private int _attackTimer;
        private int _knockbackTimer;

        private bool _secondAttack;
        private bool _thirdAttack;
        private int _actionTimer;
        private bool _floatUsed;

        public bool Dead = false;


        private void DisplayPosition()
        {
            var myScene = entity.scene as GameScene;
            myScene.UpdateScene();
        }

        public override void onAddedToEntity()
        {
            HitboxOffset = new Vector2(7, -14);
            HitboxOffsetFlip = new Vector2(-26, -14);
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


            var texture = entity.scene.contentManager.Load<Texture2D>("leekrun5");
            var textureRev = entity.scene.contentManager.Load<Texture2D>("leekrun-sheet2");
            var idleTexture = entity.scene.contentManager.Load<Texture2D>("leekidle");
            var fallTexture = entity.scene.contentManager.Load<Texture2D>("leekfall");
            var jumpTexture = entity.scene.contentManager.Load<Texture2D>("leekjump");
            var attackTexture = entity.scene.contentManager.Load<Texture2D>("leekattack-sheet");
            var rollTexture = entity.scene.contentManager.Load<Texture2D>("leekroll");
            var floatTexture = entity.scene.contentManager.Load<Texture2D>("leekfloat");
            var climbAnimation = entity.scene.contentManager.Load <Texture2D>("leekclimb");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 50, 50);
            var subtexturesRev = Subtexture.subtexturesFromAtlas(textureRev, 50, 50);
            var idleSubtexture = Subtexture.subtexturesFromAtlas(idleTexture, 50, 50);
            var fallSubtexture = Subtexture.subtexturesFromAtlas(fallTexture, 20, 21);
            var jumpSubtexture = Subtexture.subtexturesFromAtlas(jumpTexture, 20, 21);
            var attackSubtexture = Subtexture.subtexturesFromAtlas(attackTexture, 50, 50);
            var rollSubtexture = Subtexture.subtexturesFromAtlas(rollTexture, 20, 21);
            var floatSubtextureTexture = Subtexture.subtexturesFromAtlas(floatTexture, 50, 50);
            var climbSubtexture = Subtexture.subtexturesFromAtlas(climbAnimation, 22, 22);

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
                rollSubtexture[1],
                rollSubtexture[2],
                rollSubtexture[2]
            }));

            _animation.addAnimation(Animations.Float, new SpriteAnimation(new List<Subtexture>()
            {
                floatSubtextureTexture[0]
            }));

            _animation.addAnimation(Animations.Climb, new SpriteAnimation(new List<Subtexture>()
            {
                climbSubtexture[0],
                climbSubtexture[0],
                climbSubtexture[1],
                climbSubtexture[1],
                climbSubtexture[2],
                climbSubtexture[2],
                climbSubtexture[3],
                climbSubtexture[3]
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
                case State.Float:
                    DoFloat();
                    break;
                case State.Climb:
                    DoClimb();
                    break;
            }
        }

        public void UpdateDropCount(int val)
        {
            DropCount = DropCount + val;
        }


        private void DoClimb()
        {
            entity.transform.position = new Vector2(LadderInUse.absolutePosition.X + 8, entity.transform.position.Y);
            var moveDir = new Vector2(0, YAxisInput.value);
            var xMoveDir = _xAxisInput.value;
            IgnoreGravity = true;
            Grounded = true;

            if (xMoveDir < 0)
            {
                _animation.flipX = false;
            }
            else if (xMoveDir > 0)
            {
                _animation.flipX = true;
            }

            //jump
            if (CheckJumpInput()&& xMoveDir != 0)
            {
                moveDir = new Vector2(xMoveDir, Jump());
                activeState = State.Normal;
            }
            var playAnimation = true;
            Console.WriteLine(moveDir.Y);
            if (moveDir.Y == 0)
            {
                playAnimation = false;
            }
            DoMovement(moveDir, Animations.Climb, playAnimation);
        }

        private void DoFloat()
        {
            if (!Grounded && _floatInput.isDown)
            {
                IgnoreGravity = true;
            }
            else
            {
                activeState = State.Normal;
                IgnoreGravity = false;
            }

            var moveDir = new Vector2(_xAxisInput.value, 0);

            if (moveDir.X < 0)
            {
                _animation.flipX = false;
            }
            else if (moveDir.X > 0)
            {
                _animation.flipX = true;
            }


            DoMovement(moveDir, Animations.Float);
        }

        public void DoKnockback()
        {
            var moveDir = new Vector2(2, -1);
            if (_animation.flipX)
            {
                moveDir.X = moveDir.X * -1;
            }

            _knockbackTimer = _knockbackTimer + 1;
            if (_knockbackTimer > 6)
            {
                _knockbackTimer = 0;
                activeState = State.Normal;
            }
            DoMovement(moveDir, Animations.Run);

        }

        private void DoNormal()
        {
            IgnoreGravity = false;
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

            if (_floatInput.isDown && !Grounded && !_floatUsed)
            {
                activeState = State.Float;
                _floatUsed = true;
            }

            if (_actionTimer > 5)
            {
                //roll
                if (_rollInput.isPressed && Grounded)
                {
                    activeState = State.Roll;
                }

                if (_attackInput.isPressed && Grounded)
                {
                    activeState = State.Attack;
                }
                else if (_attackInput.isPressed && !Grounded)
                {
                    //DO ARIAL ATTACK HERE
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

            if (ShouldBounce)
            {
                if (!_isBouncing)
                {
                    _jumpTime = _maxJumpTime;
                    _isBouncing = true;
                }
                Grounded = false;
                moveDir.Y = Jump();
            }
            else
            {
                _isBouncing = false;
            }
            DoMovement(moveDir, animation);
        }

        private void DoAttack()
        {

            var animation = Animations.Attack;
            var moveDir = new Vector2(0, 0);
            var flip = 1;

            if (!_animation.flipX)
            {
                flip = -1;
            }

            _attackTimer = _attackTimer + 1;

            if (22 >= _attackTimer && _attackTimer > 10)
            {
                moveDir.X = 0.3f * flip;

                if (_attackInput.isPressed)
                {
                    _secondAttack = true;                  
                }
            }
            //kill
            if (_attackTimer >= 18)
            {
                moveDir.X = 0 * flip;
                if (!_secondAttack && _attackTimer >= 22)
                {
                    ExitAttack();
                }
                else if (_secondAttack)
                {
                    animation = Animations.Attack2;
                }
            }


            if ((40 >= _attackTimer && _attackTimer > 24))
            {
                moveDir.X = 0.1f * flip;
                if (_attackInput.isPressed)
                {
                    _thirdAttack = true;
                    ThirdAttack = true;
                }
            }

            if (_attackTimer >= 36)
            {
                moveDir.X = 0.2f * flip;
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

            DoMovement(moveDir, animation);
        }

        private void ExitAttack()
        {
            _secondAttack = false;
            _thirdAttack = false;
            ThirdAttack = false;
            activeState = State.Normal;
            _attackTimer = 0;
            _actionTimer = 0;
        }

        public void DoHurt(int damage)
        {
            if (!IsInvuln)
            {
                Health = Health - damage;
                IsInvuln = true;
                _invulnCount = 0;
            }
        }

        private void DoRoll()
        {
            var moveDir = new Vector2(0, 0);
            var animation = Animations.Rolling;
            _rollCount = _rollCount + 1;

            if (_rollCount > 20)
            {
                activeState = State.Normal;
                _rollCount = 0;
            }
            if (_animation.flipX)
            {
                moveDir.X = 1.5f;
            }
            else
            {
                moveDir.X = -1.5f;
            }
            _actionTimer = 0;
            DoMovement(moveDir, animation);
        }

        private void DoMovement(Vector2 moveDir, Animations animation, bool playAnimation = true)
        {
            CollisionResult res;
            var movement = (moveDir * _moveSpeed * Time.deltaTime);
            
            _mover.move(movement, out res);
            if (!_animation.isAnimationPlaying(animation)&&playAnimation)
            {
                _animation.play(animation);
            }
            else if (!playAnimation)
            {
                _animation.stop();
            }
        }

        void setupInput()
        {
            // horizontal input from dpad, left stick or keyboard left/right
            _xAxisInput = new VirtualIntegerAxis();
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadDpadLeftRight());
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadLeftStickX());
            _xAxisInput.nodes.Add(new Nez.VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

            YAxisInput = new VirtualIntegerAxis();
            YAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadDpadUpDown());
            YAxisInput.nodes.Add(new Nez.VirtualAxis.GamePadLeftStickY());
            YAxisInput.nodes.Add(new Nez.VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));

            // vertical input from dpad, left stick or keyboard up/down
            _jumpInput = new VirtualButton();
            _jumpInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.A));
            _jumpInput.nodes.Add(new Nez.VirtualButton.GamePadButton(0, Buttons.A));

            _rollInput = new VirtualButton();
            _rollInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.Q));
            _rollInput.nodes.Add(new Nez.VirtualButton.GamePadButton(0, Buttons.X));

            _floatInput = new VirtualButton();
            _floatInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.Space));

            _attackInput = new VirtualButton();
            _attackInput.nodes.Add(new Nez.VirtualButton.KeyboardKey(Keys.F));
        }

        private float Jump()
        {
            if (_isBouncing)
            {
                Console.WriteLine("BOUNCE");
                if (_jumpTime + 10 > 0)
                {
                    var x = 3f;
                    float y = 1f - (0.5f * x * x);
                    _jumpTime = _jumpTime - 1;
                    return y;
                }
                else
                {
                    Console.WriteLine("TURN OFF JUMP");
                    _isBouncing = false;
                    _jumpTime = _maxJumpTime;
                    ShouldBounce = false;
                    return 0;
                }
            }
            
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
            if (Health <= 0)
            {
                Dead = true;
            }

            if (IsInvuln)
            {
                _invulnCount = _invulnCount + 1;
                if (_invulnCount > 50)
                {
                    IsInvuln = false;
                }
            }

            //dev
            DisplayPosition();

            //keep
            StateMachine();

            if (!Grounded)
            {
                if (!_jumpInput.isDown)
                {
                    _hasJumped = true;
                }
            }

            if (Grounded)
            {
                //ShouldBounce = false;
                _jumpTime = _maxJumpTime;
                if (!_jumpInput.isDown)
                {
                    _hasJumped = false;
                    _floatUsed = false;
                }
            }
            if (_animation.flipX)
            {
                entity.colliders[1].setLocalOffset(HitboxOffset);
                IsFlipped = true;
            }
            else
            {
                IsFlipped = false;
                entity.colliders[1].setLocalOffset(HitboxOffsetFlip);
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

