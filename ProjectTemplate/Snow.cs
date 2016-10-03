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
    class Snow : Component, ITriggerListener, IUpdatable
    {
        enum Animations
        {
            Idle
        }

        private Mover _mover;
        private Sprite<Animations> _animation;

        public override void onAddedToEntity()
        {
            var texture = entity.scene.contentManager.Load<Texture2D>("snow");

            var subtextures = Subtexture.subtexturesFromAtlas(texture, 256, 144);

            _mover = entity.addComponent(new Mover());
            _animation = entity.addComponent(new Sprite<Animations>(subtextures[0]));
            _animation.origin = new Vector2(0, 0);
            _animation.setRenderLayer(998);
            _animation.addAnimation(Animations.Idle, new SpriteAnimation(new List<Subtexture>()
            {
                subtextures[0],
                subtextures[1],
                subtextures[2],
                subtextures[3],
                subtextures[4],
                subtextures[5],
                subtextures[6],
                subtextures[7],
                subtextures[8],
                subtextures[9],
                subtextures[10],
                subtextures[11],
                subtextures[12],
                subtextures[13],
                subtextures[14],
                subtextures[15],
                subtextures[16],
                subtextures[17],
                subtextures[18],
                subtextures[19],
                subtextures[20],
                subtextures[21],
                subtextures[22],
                subtextures[23],
                subtextures[24],
                subtextures[25],
                subtextures[26]
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
