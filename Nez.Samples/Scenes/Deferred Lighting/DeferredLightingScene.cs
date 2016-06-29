﻿using System;
using Nez.DeferredLighting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;


namespace Nez.Samples
{
	[SampleScene( "Deferred Lighting", "Press the number keys to change the light that is currently being controlled\nPressing f toggles the rendering of the individual buffers used by the deferred lighting system" )]
	public class DeferredLightingScene : SampleScene
	{
        const int RENDERABLES_LAYER = 5;
		const int LIGHT_LAYER = 10;


		public DeferredLightingScene() : base( false, false )
		{}


		public override void initialize()
		{
			base.initialize();

			// setup screen that fits our map based on the bg size
			setDesignResolution( 137 * 9, 89 * 9, Scene.SceneResolutionPolicy.ShowAllPixelPerfect );
			Screen.setSize( 137 * 9, 89 * 9 );
			clearColor = Color.DarkGray;

			// add our renderer setting the renderLayers we will use for lights and for renderables
			var deferredRenderer = addRenderer( new DeferredLightingRenderer( 0, LIGHT_LAYER, RENDERABLES_LAYER) )
				.setClearColor( Color.DarkGray );
			deferredRenderer.enableDebugBufferRender = false;

			// prep our textures. we have diffuse and normal maps to interact with the lights.
			var moonTex = contentManager.Load<Texture2D>( "DeferredLighting/moon" );
			var moonNorm = contentManager.Load<Texture2D>( "DeferredLighting/moonNorm" );
			var orangeTexture = contentManager.Load<Texture2D>( "DeferredLighting/orange" );
			var orangeNormalMap = contentManager.Load<Texture2D>( "DeferredLighting/orangeNorm" );
			var bgTexture = contentManager.Load<Texture2D>( "DeferredLighting/bg" );
			var bgNormalMap = contentManager.Load<Texture2D>( "DeferredLighting/bgNorm" );

			// prep our Materials. Deferred lighting requires a Material that is normal map aware. We can also leave our Material null and
			// a default Material will be used that is diffuse lighting only (no normal map).
			var moonMaterial = new DeferredSpriteMaterial( moonNorm );
			var orangeMaterial = new DeferredSpriteMaterial( orangeNormalMap );
			var bgMaterial = new DeferredSpriteMaterial( bgNormalMap );

			// create some Entities. When we add the Renderable (Sprite in this case) we need to be sure to set the renderLayer and Material
			var bgEntity = createEntity( "bg" );
			bgEntity.transform.setPosition( Screen.center ).setScale( 9 );
			bgEntity.addComponent( new Sprite( bgTexture ) ).setRenderLayer( RENDERABLES_LAYER ).setMaterial( bgMaterial ).setLayerDepth( 1 );
			bgEntity.addComponent( new DeferredLightingController() );

			var orangeEntity = createEntity( "orange" );
			orangeEntity.transform.setPosition( Screen.center ).setScale( 0.5f );
			orangeEntity.addComponent( new Sprite( orangeTexture ) ).setRenderLayer( RENDERABLES_LAYER ).setMaterial( orangeMaterial );
			orangeEntity.addComponent( new SpotLight() ).setRenderLayer( LIGHT_LAYER );

			var moonEntity = createEntity( "moon" );
			moonEntity.transform.setPosition( new Vector2( 100, 400 ) );
			moonEntity.addComponent( new Sprite( moonTex ) ).setRenderLayer( RENDERABLES_LAYER ).setMaterial( moonMaterial );
			moonEntity.addComponent( new DirLight( Color.Red ) ).setRenderLayer( LIGHT_LAYER ).setEnabled( true );

			var clone = orangeEntity.clone( new Vector2( 200, 200 ) );
			addEntity( clone );

			var mouseFollowEntity = createEntity( "mouse-follow" );
			mouseFollowEntity.addComponent( new MouseFollow() );
			mouseFollowEntity.addComponent( new PointLight( new Color( 0.8f, 0.8f, 0.9f ) ) ).setRadius( 200 ).setIntensity( 2 )
				.setRenderLayer( LIGHT_LAYER );
		}
	}
}

