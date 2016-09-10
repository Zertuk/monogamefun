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
            Attack
        }

        Sprite<Animations> _animation;
        public int health;
        private int _stunCount;
        private int _attackCount;
        private Mover _mover;
        private float _moveSpeed;
        public State ActiveState;
        private int _moveDirection;
        private int _attackRange;
        public Enemy()
        {
            _attackRange = 20;
            _moveSpeed = 20f;
            health = 10;
            _stunCount = 0;
            ActiveState = State.Normal;
        }

        public override void onAddedToEntity()
        {
            var texture = entity.scene.contentManager.Load<Texture2D>("beetle");
            var hurtTexture = entity.scene.contentManager.Load<Texture2D>("beetlehurt");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 16, 16);
            var hurtSubtextures = Subtexture.subtexturesFromAtlas(hurtTexture, 20, 20);
            _mover = entity.addComponent(new Mover());
            _animation = entity.addComponent(new Sprite<Animations>(subtextures[0]));
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
            var animation = Animations.Stun;

            _attackCount = _attackCount + 1;
            if (_attackCount > 25)
            {
                _attackCount = 0;
                ActiveState = State.Normal;
            }

            DoMovement(moveDir, animation);
        }

        private void DoNormal()
        {
            var moveDir = new Vector2(_moveDirection, 0);
            var animation = Animations.Idle;

            DoMovement(moveDir, animation);
        }

        public void CheckInRange(Vector2 position, Vector2 followVector)
        {
            if (Vector2.Distance(position, followVector) < _attackRange)
            {
                ActiveState = State.Attack;
            }
        }

        public void SetMoveDirection(Vector2 position, Vector2 followVector)
        {
            if (position.X > followVector.X)
            {
                _moveDirection = -1;
            }
            else
            {
                _moveDirection = 1;
            }
        }

        private void DoStun()
        {
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
            StateMachine();
            
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
