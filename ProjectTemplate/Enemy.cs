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

        enum Animations
        {
            Idle
        }

        Sprite<Animations> _animation;
        public int health;
        private Mover _mover;
        private float _moveSpeed;
        public Enemy()
        {
            _moveSpeed = 100f;
            health = 10;
            //_mover = entity.addComponent(new Mover());

        }

        //public void UsePath(List<Point> path)
        //{
        //    var next = path.First();
        //    Console.WriteLine(next);
        //    CollisionResult res;
        //    _mover.move(new Vector2(_moveSpeed, 0), out res);
        //}

        public override void onAddedToEntity()
        {
            var texture = entity.scene.contentManager.Load<Texture2D>("beetle");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 16, 16);
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
            

        }

        void IUpdatable.update()
        {
            var animation = Animations.Idle;
            if (!_animation.isAnimationPlaying(animation))
            {
                _animation.play(animation);
            }
            var moveDir = new Vector2(0, 0);
            //moveDir.Y = _gravity.calcGrav();
            var movement = moveDir * _moveSpeed * Time.deltaTime;
            CollisionResult res;
            _mover.move(movement, out res);

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
