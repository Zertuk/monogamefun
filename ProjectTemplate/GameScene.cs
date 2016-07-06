using System;
using Nez.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Tiled;
using Nez;
using Nez.UI;

namespace ProjectTemplate
{
    //[SampleScene( "Platformer", "Work in progress..." )]
    public class GameScene : DefaultScene
    {
        public GameScene()
        {
        }
        private World _world;
        public override void initialize()
        {
            // setup a pixel perfect screen that fits our map
            setDesignResolution(320, 240, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.setSize(320 * 3, 240 * 3);

            _world = new World();

            var tiledEntity = createEntity("tiled");
            var tiledmap = contentManager.Load<TiledMap>(_world.activeRoom.tilemap);
            var tiledMapComponent = tiledEntity.addComponent(new TiledMapComponent(tiledmap, "collision"));
            tiledMapComponent.renderLayer = 10;

            var playerEntity = createRigidEntity(new Vector2(50, 50), 1f, 0, 0f, new Vector2(0, 0));
            playerEntity.shouldUseGravity = true;
            DisplayHealthBar();
            // add a component to have the Camera follow the player
            //camera.entity.addComponent(new FollowCamera(playerEntity.entity));
        }

        private void DisplayHealthBar()
        {
            var stage = new Stage();

            var canvas = createEntity("ui").addComponent(new UICanvas());
            canvas.isFullScreen = true;

            var table = canvas.stage.addElement(new Table());

            var healthText = new Text(Graphics.instance.bitmapFont, "10", new Vector2(45, 7), Color.White);
            var healthEntity = createEntity("healthText");
            healthEntity.addComponent(healthText);
            var healthBar = new ProgressBar(0, 10, 1, false, ProgressBarStyle.create(Color.Red, Color.Black));
            var healthBarBorder = new ProgressBar(1, 10, 1, false, ProgressBarStyle.create(Color.White, Color.White));
            var healthBarBorder2 = new ProgressBar(1, 10, 1, false, ProgressBarStyle.create(Color.White, Color.White));

            healthBarBorder2.setSize(52, 12f);
            healthBarBorder2.setPosition(5f, 4f);
            healthBar.setValue(10);
            healthBarBorder.setSize(52, 12f);
            healthBarBorder.setPosition(5f, 6f);
            healthBar.setSize(50f, 5f);
            healthBar.setPosition(6f, 8f);
            table.addElement(healthBarBorder);
            table.addElement(healthBarBorder2);

            table.addElement(healthBar);
        }

        ArcadeRigidbody createRigidEntity(Vector2 position, float mass, float friction, float elasticity, Vector2 velocity)
        {
            var rigidbody = new ArcadeRigidbody()
                .setMass(mass)
                .setFriction(friction)
                .setElasticity(elasticity)
                .setVelocity(velocity);

            var entity = createEntity(Utils.randomString(3));
            entity.transform.position = position;
            entity.addComponent(new Player());
            entity.addComponent(rigidbody);
            entity.addCollider(new CircleCollider());

            return rigidbody;
        }


    }
}

