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
    public class Fire : Component, ITriggerListener, IUpdatable
    {
        enum Animations
        {
            Idle
        }

        Sprite<Animations> _animation;

        public override void onAddedToEntity()
        {
            var texture = entity.scene.contentManager.Load<Texture2D>("fire");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 16, 32);

            _animation = entity.addComponent(new Sprite<Animations>(subtextures[0]));
            _animation.setRenderLayer(10);

            _animation.addAnimation(Animations.Idle, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1],
                subtextures[2],
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
