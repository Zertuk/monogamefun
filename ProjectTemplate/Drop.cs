using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate
{
    class Drop : Component, ITriggerListener, IUpdatable
    {
        enum Animations
        {
            Idle
        }

        public enum State
        {
            Normal,
            Follow
        }

        private Mover _mover;
        private Sprite<Animations> _animation;
        private int _moveDirection;

        public bool Collected = false;
        public int Value;
        public State ActiveState;

        public Drop(int value)
        {
            ActiveState = State.Normal;
            Value = value;
        }

        public override void onAddedToEntity()
        {
            var texture = entity.scene.contentManager.Load<Texture2D>("drop");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 2, 2);

            _mover = entity.addComponent(new Mover());
            _animation = entity.addComponent(new Sprite<Animations>(subtextures[0]));
            _animation.addAnimation(Animations.Idle, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0]
            }));
        }

        private void StateMachine()
        {
            switch (ActiveState)
            {
                case State.Normal:
                    DoNormal();
                    break;
                case State.Follow:
                    DoFollow();
                    break;
            }
        }

        private void DoNormal()
        {
            DoMovement(new Vector2(0, 0));
        }

        private void DoFollow()
        {
            var moveDir = new Vector2(1 * _moveDirection, 0);
            Console.WriteLine("SHOULD FOLLOW!");
            DoMovement(moveDir);
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

        private void DoMovement(Vector2 moveDir)
        {
            var animation = Animations.Idle;
            if (!_animation.isAnimationPlaying(animation))
            {
                _animation.play(animation);
            }
            CollisionResult res;
            _mover.move(moveDir, out res);
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
