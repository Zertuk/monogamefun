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
    public class Shroom : Component, ITriggerListener, IUpdatable
    {
        public enum State
        {
            Normal,
            Bounce
        }
        enum Animations
        {
            Idle,
            Bounce
        }

        Sprite<Animations> _animation;
        public State ActiveState;
        public Shroom()
        {
        }

        public override void onAddedToEntity()
        {
            var texture = entity.scene.contentManager.Load<Texture2D>("shroom");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 16, 16);

            _animation = entity.addComponent(new Sprite<Animations>(subtextures[0]));
            _animation.addAnimation(Animations.Idle, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0]
            }));

            _animation.addAnimation(Animations.Bounce, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0]
            }));

        }

        private void StateMachine()
        {
            switch (ActiveState)
            {
                case State.Normal:
                    //DoNormal();
                    break;
                case State.Bounce:
                    //DoBounce();
                    break;
            }
        }




        void IUpdatable.update()
        {
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
