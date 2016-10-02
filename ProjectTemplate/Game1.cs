using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace ProjectTemplate
{
	public class Game1 : Core
	{


        public Game1()
        {
        }

        protected override void Initialize()
		{
			base.Initialize();
            var myScene = new GameScene();
            scene = myScene;
        }
        private void Update()
        {
        }
    }
}

