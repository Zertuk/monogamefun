using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.AI.Pathfinding;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate
{
    public class Enemy : Component, ITriggerListener, IUpdatable
    {
        public enum State
        {
            Normal,
            Stun,
            Attack
        }
        enum Animations
        {
            Idle,
            Stun,
            Attack,
            Run
        }

        Sprite<Animations> _animation;
        public bool AttackCanDamage = false;
        public int CurrentFrame;
        public int[] FramesToAttack;
        public bool IsFlipped;
        public Vector2 HitboxOffset;
        public Vector2 HitboxOffsetFlip;
        public int Health;
        public bool Dead;
        private int _stunCount;
        public int _attackCount;
        private Mover _mover;
        private float _moveSpeed;
        public State ActiveState;
        private int _moveDirection;
        private int _attackRange;
        private int _dropAmount;
        private string _type;
        public Enemy(string type)
        {
            _dropAmount = 5;
            _attackRange = 30;
            _moveSpeed = 40f;
            Health = 3;
            _stunCount = 0;
            ActiveState = State.Normal;
            _type = type;
        }

        public override void onAddedToEntity()
        {
            FramesToAttack = new int[] { 7 };
            CurrentFrame = 0;
            HitboxOffset = new Vector2(7, -14);
            HitboxOffsetFlip = new Vector2(-26, -14);
            var texture = entity.scene.contentManager.Load<Texture2D>(_type);
            var runTexture = entity.scene.contentManager.Load<Texture2D>(_type + "run");
            var hurtTexture = entity.scene.contentManager.Load<Texture2D>(_type + "hurt");
            var attackTexture = entity.scene.contentManager.Load<Texture2D>(_type + "attack");
            
            var subtextures = Subtexture.subtexturesFromAtlas(texture, 16, 16);
            var hurtSubtextures = Subtexture.subtexturesFromAtlas(hurtTexture, 20, 20);
            var attackSubtextures = Subtexture.subtexturesFromAtlas(attackTexture, 50, 50);
            var runSubtextureTexture = Subtexture.subtexturesFromAtlas(runTexture, 50, 50);

            _mover = entity.addComponent(new Mover());
            _animation = entity.addComponent(new Sprite<Animations>(subtextures[0]));
            _animation.setRenderLayer(0);
            _animation.addAnimation(Animations.Idle, new SpriteAnimation(new List<Subtexture>()
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

            _animation.addAnimation(Animations.Stun, new SpriteAnimation(new List<Subtexture>()
            {
                hurtSubtextures[0],
                hurtSubtextures[0]
            }));

            _animation.addAnimation(Animations.Attack, new SpriteAnimation(new List<Subtexture>()
            {
                attackSubtextures[0],
                attackSubtextures[1],
                attackSubtextures[2],
                attackSubtextures[3],
                attackSubtextures[4],
                attackSubtextures[5],
                attackSubtextures[6],
                attackSubtextures[7],
                attackSubtextures[8],
                attackSubtextures[9]
            }));

            _animation.addAnimation(Animations.Run, new SpriteAnimation(new List<Subtexture>()
            {
                runSubtextureTexture[0],
                runSubtextureTexture[1],
                runSubtextureTexture[2],
                runSubtextureTexture[3]
            }));
        }

        private void StateMachine()
        {
            switch (ActiveState)
            {
                case State.Normal:
                    DoNormal();
                    break;
                case State.Stun:
                    DoStun();
                    break;
                case State.Attack:
                    DoAttack();
                    break;
            }
        }

        private void DoAttack()
        {
            var moveDir = new Vector2(0, 0);
            var animation = Animations.Attack;
            Console.WriteLine(_animation.currentFrame);
            _attackCount = _attackCount + 1;
            if (_attackCount > 35)
            {
                moveDir.X = -1.5f;
            }

            if (_attackCount > 50)
            {
                _attackCount = 0;
                ActiveState = State.Normal;
            }

            if (_animation.flipX)
            {
                moveDir.X = moveDir.X * -1;
            }

            DoMovement(moveDir, animation);

            if (FramesToAttack.contains(_animation.currentFrame))
            {
                AttackCanDamage = true;
            }
            else
            {
                AttackCanDamage = false;
            }
        }

        public void DoDeath()
        {
            
        }

        private void DoNormal()
        {
            var moveDir = new Vector2(_moveDirection, 0);
            var animation = Animations.Idle;
            if (moveDir.X != 0)
            {
                animation = Animations.Run;
            }
            if (moveDir.X > 0)
            {
                _animation.flipX = true;
            }
            else
            {
                _animation.flipX = false;
            }

            DoMovement(moveDir, animation);
        }

        public void CheckInRange(Vector2 position, Vector2 followVector)
        {
            if (Vector2.Distance(position, followVector) < _attackRange)
            {
                ActiveState = State.Attack;
            }
        }

        public void SetMoveDirection(Vector2 position, Vector2 follow)
        {
            if (position.X > follow.X)
            {
                _moveDirection = -1;
            }
            else
            {
                _moveDirection = 1;
            }
        }

        public void DoHurt(int damage)
        {
            Health = Health - damage;
        }

        private void DoStun()
        {
            _attackCount = 0;
            _stunCount = _stunCount + 1;
            var animation = Animations.Stun;
            var moveDir = new Vector2(0f, -0f);

            if (_stunCount > 25)
            {
                _stunCount = 0;
                ActiveState = State.Normal;
            }

            DoMovement(moveDir, animation);
        }

        private void DoMovement(Vector2 moveDir, Animations animation)
        {
            if (!_animation.isAnimationPlaying(animation))
            {
                _animation.play(animation);
            }


            CollisionResult res;
            var movement = moveDir * _moveSpeed * Time.deltaTime;
            _mover.move(movement, out res);

        }



        void IUpdatable.update()
        {
            if (Health <= 0)
            {
                Dead = true;
            }
            StateMachine();

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


        //#region ITriggerListener implementation

        void ITriggerListener.onTriggerEnter(Collider other, Collider self)
        {
            Debug.log("triggerEnter: {0}", other.entity.name);
        }

        void ITriggerListener.onTriggerExit(Collider other, Collider self)
        {
            Debug.log("triggerExit: {0}", other.entity.name);
        }

        //#endregion



    }
}
