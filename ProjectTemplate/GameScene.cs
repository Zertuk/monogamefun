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
        private ArcadeRigidbody playerEntity;
        private int _width;
        private int _height;
        private Entity _tiledEntity;
        private string _prevTileMapName;
        public GameScene()
        {
            _world = new World();
            playerEntity = createRigidEntity(new Vector2(50, 50), 1f, 0, 0f, new Vector2(0, 0));
            playerEntity.shouldUseGravity = true;
            UpdateTileMap(new Vector2(100, 100));

            // add a component to have the Camera follow the player
            camera.entity.addComponent(new FollowCamera(playerEntity.entity));
        }
        private World _world;

        public override void initialize()
        {
            // setup a pixel perfect screen that fits our map
            setDesignResolution(320, 240, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.setSize(320*3, 240*3);
            DisplayHealthBar();
        }

        private void UpdateTileMap(Vector2 newPos)
        {
            //only load if actually new room and not just new part of old room
            Console.WriteLine(_prevTileMapName);
            Console.WriteLine(_world.activeRoom.tilemap);
            if (_prevTileMapName != _world.activeRoom.tilemap)
            {
                if (_tiledEntity != null)
                {
                    _tiledEntity.destroy();
                }
                _tiledEntity = createEntity("tiled");
                var tiledmap = contentManager.Load<TiledMap>(_world.activeRoom.tilemap);
                var tiledMapComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "collision"));
                _tiledEntity.addComponent(new CameraBounds(new Vector2(0, 0), new Vector2(16 * (tiledmap.width), 16 * (tiledmap.height))));
                _width = tiledmap.width * 16;
                _height = tiledmap.height * 16;
                tiledMapComponent.renderLayer = 10;
                _prevTileMapName = _world.activeRoom.tilemap;
                playerEntity.transform.position = newPos;
            }
            else
            {
                Console.WriteLine("DO NOT LOAD");
            }
        }

        private void DisplayHealthBar()
        {
            var stage = new Stage();

            var canvas = createEntity("ui").addComponent(new UICanvas());
            Console.WriteLine(canvas.width);
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

        public void UpdateScene()
        {
            //Core.debugRenderEnabled = true;
            //Console.WriteLine(playerEntity.transform.position.X + ", " + playerEntity.transform.position.Y);
            CheckDoors();
        }

        private void CheckDoors()
        {
            Console.WriteLine("CURRENT INDEX: " + _world.activeRoom.index[0] + ", " + _world.activeRoom.index[1]);
            if (playerEntity.transform.position.X <= 0)
            {
                //go left
                Console.WriteLine("left");
                _world.ChangeRoom(0, -1);
                var newPos = new Vector2(_width - 16, playerEntity.transform.position.Y);
                UpdateTileMap(newPos);
            }
            else if (playerEntity.transform.position.X >= _width/_world.activeRoom.roomIndex[1])
            {
                //go right
                Console.WriteLine("right");
                _world.ChangeRoom(0, 1);
                var newPos = new Vector2(16, playerEntity.transform.position.Y);
                UpdateTileMap(newPos);
            }
            else if (playerEntity.transform.position.Y <= 0)
            {
                //go up
                Console.WriteLine("up");
                _world.ChangeRoom(-1, 0);
                var newPos = new Vector2(playerEntity.transform.position.X, _height - 16);
                UpdateTileMap(newPos);
            }
            else if (playerEntity.transform.position.Y >= _height / _world.activeRoom.roomIndex[0])
            {
                //go down
                Console.WriteLine("down");
                _world.ChangeRoom(1, 0);
                var newPos = new Vector2(playerEntity.transform.position.X, 16);
                UpdateTileMap(newPos);
            }
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
            entity.addCollider(new CircleCollider(8, new Vector2(0, 0)));

            return rigidbody;
        }


    }
}

