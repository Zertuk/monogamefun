using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nez;
using Nez.UI;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Nez.Tweens;


namespace ProjectTemplate
{
    /// <summary>
    /// this entire class is one big sweet hack job to make adding samples easier. An exceptional hack is made so that we can render small
    /// pixel art scenes pixel perfect and still display our UI at a reasonable size.
    /// </summary>
    public abstract class DefaultScene : Scene
    {
        public const int SCREEN_SPACE_RENDER_LAYER = 999;
        public UICanvas canvas;

        Table _table;
        List<Button> _sceneButtons = new List<Button>();
        ScreenSpaceRenderer _screenSpaceRenderer;


        public DefaultScene(bool addExcludeRenderer = true, bool needsFullRenderSizeForUI = false) : base()
        {
            // setup one renderer in screen space for the UI and then (optionally) another renderer to render everything else
            if (needsFullRenderSizeForUI)
            {
                // dont actually add the renderer since we will manually call it later
                _screenSpaceRenderer = new ScreenSpaceRenderer(100, SCREEN_SPACE_RENDER_LAYER);
                _screenSpaceRenderer.shouldDebugRender = false;
            }
            else
            {
                addRenderer(new ScreenSpaceRenderer(100, SCREEN_SPACE_RENDER_LAYER));
            }

            if (addExcludeRenderer)
                addRenderer(new RenderLayerExcludeRenderer(0, SCREEN_SPACE_RENDER_LAYER));

            // create our canvas and put it on the screen space render layer
            canvas = createEntity("ui").addComponent(new UICanvas());
            canvas.isFullScreen = true;
            canvas.renderLayer = SCREEN_SPACE_RENDER_LAYER;
        }
    }

}

