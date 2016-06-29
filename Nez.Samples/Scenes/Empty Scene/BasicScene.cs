﻿using System;
using Nez.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.UI;


namespace Nez.Samples
{
	[SampleScene( "Basic Scene", "Scene with a single Entity. The minimum to have something to show" )]
	public class BasicScene : SampleScene
	{
		public override void initialize()
		{
			base.initialize();

			// default to 1280x720 with no SceneResolutionPolicy
			setDesignResolution( 1280, 720, Scene.SceneResolutionPolicy.None );
			Screen.setSize( 1280, 720 );

			var moonTex = contentManager.Load<Texture2D>( "Shared/moon" );
			var playerEntity = createEntity( "player", new Vector2( Screen.width / 2, Screen.height / 2 ) );
			playerEntity.addComponent( new Sprite( moonTex ) );
		}
	}
}

