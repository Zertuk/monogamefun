using System;
using Nez.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Tiled;
using Nez;
using Nez.UI;
using Microsoft.Xna.Framework.Input;
using static Nez.Tiled.TiledMapMover;
using System.Linq;
using Nez.AI.Pathfinding;
using System.Collections.Generic;

namespace ProjectTemplate
{
    public class DeathScene : DefaultScene
    {
        private UICanvas _canvas;
        private Entity _ripTextEntity;
        private ContinueComponent _continue;

        public DeathScene()
        {
            CreateUI();
            AddText();
            var entity = new Entity();
            _continue = new ContinueComponent();
            entity.addComponent(_continue);
            entity.attachToScene(this);
        }
        private void CreateUI()
        {
            _canvas = createEntity("ui").addComponent(new UICanvas());
            _canvas.isFullScreen = true;
            _canvas.setRenderLayer(SCREEN_SPACE_RENDER_LAYER);
        }
        private void AddText()
        {
            var table = _canvas.stage.addElement(new Table());
            table.setFillParent(true);
            table.setBackground(new PrimitiveDrawable(Color.Black));

            var ripText = new Label(";-; RIP ;-;").setFontColor(Color.White);
            ripText.fillParent = true;
            ripText.setWrap(true);
            ripText.setAlignment(Align.center);
            ripText.setFontScale(4);
            table.addElement(ripText);
        }
        private Scene ReturnScene()
        {
            return new GameScene(); ;
        }

        public void Update()
        {
            if (_continue.ShouldContinue)
            {
                var transition = new SquaresTransition(ReturnScene);
                Core.startSceneTransition(transition);

                Core.scene = new GameScene();

            }
        }

    }
}

