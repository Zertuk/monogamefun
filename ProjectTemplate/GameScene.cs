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
    //[SampleScene( "Platformer", "Work in progress..." )]
    public class GameScene : DefaultScene
    {
        private ArcadeRigidbody playerEntity;
        private ArcadeRigidbody enemyEntity;
        private int _width;
        private int _height;
        private Entity _tiledEntity;
        private string _prevTileMapName;
        private TiledTileLayer _tileCollLayer;
        public GameScene()
        {
            Transform.shouldRoundPosition = false;
            addRenderer(new ScreenSpaceRenderer(100, SCREEN_SPACE_RENDER_LAYER));
            _world = new World();
            
            enemyEntity = createRigidEntity(new Vector2(150, 50), 1f, 100f, 0f, new Vector2(0, 0), false, true);
            enemyEntity.entity.tag = 5;
            playerEntity = createRigidEntity(new Vector2(50, 50), 1f, 100f, 0, new Vector2(0, 0), true, false);

            UpdateTileMap(new Vector2(100, 100), false);

            //var graph = new WeightedGridGraph();

            var cameFrom = new Dictionary<Vector2, Vector2>();
            var tiled = contentManager.Load<TiledMap>(_world.activeRoom.tilemap);

            // add a component to have the Camera follow the player
            camera.entity.addComponent(new FollowCamera(playerEntity.entity));

        }
        private World _world;

        public override void initialize()
        {
            //Core.debugRenderEnabled = true;

            // setup a pixel perfect screen that fits our map
            setDesignResolution(256, 144, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.setSize(256*3, 144*3);
            //Screen.isFullscreen = true;

            DisplayHealthBar(9, 10, 0);
        }

        private void UpdateTileMap(Vector2 newPos, bool left)
        {
            //only load if actually new room and not just new part of old room
            if (_prevTileMapName != _world.activeRoom.tilemap)
            {
                if (_tiledEntity != null)
                {
                    _tiledEntity.destroy();
                }
                _tiledEntity = createEntity("tiled");






                var tiledmap = contentManager.Load<TiledMap>(_world.activeRoom.tilemap);
                var tiledMapComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "collision"));
                tiledMapComponent.setLayersToRender(new string[] { "collision" });
                tiledMapComponent.renderLayer = 10;
                tiledMapComponent.physicsLayer = 10;

                var accentComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "accent"));
                accentComponent.renderLayer = 0;
                accentComponent.collisionLayer = null;
                accentComponent.setLayersToRender("accent");

                var bgComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "bg"));
                bgComponent.setLayersToRender("bg");
                bgComponent.renderLayer = 11;
                bgComponent.collisionLayer = null;

                _tiledEntity.addComponent(new CameraBounds(new Vector2(0, 0), new Vector2(16 * (tiledmap.width), 16 * (tiledmap.height))));
                _width = tiledmap.width * 16;
                _height = tiledmap.height * 16;
                _prevTileMapName = _world.activeRoom.tilemap;
                _tileCollLayer = tiledMapComponent.collisionLayer;
                //if left make sure we set pos to new width
                if (left)
                {
                    newPos.X = _width - 16;
                }
                playerEntity.transform.position = newPos;
                
            }
        }

        private void UpdateHealthBar(int health, int maxHealth)
        {
        }

        private void DisplayHealthBar(int health, int maxHealth, int dropCount)
        {
            var canvas = createEntity("ui").addComponent(new UICanvas());
            canvas.isFullScreen = true;
            canvas.setRenderLayer(SCREEN_SPACE_RENDER_LAYER);
            var table = canvas.stage.addElement(new Table());
            table.setFillParent(true);
            var healthText = new Text(Graphics.instance.bitmapFont, health.ToString(), new Vector2(45, 7), Color.White);
            var healthEntity = createEntity("healthText");
            healthText.setRenderLayer(SCREEN_SPACE_RENDER_LAYER);
            healthEntity.addComponent(healthText);
            var healthBar = new ProgressBar(0, maxHealth, 1, false, ProgressBarStyle.create(Color.Red, Color.Black));
            var healthBarBorder = new ProgressBar(1, maxHealth, 1, false, ProgressBarStyle.create(Color.White, Color.White));
            var healthBarBorder2 = new ProgressBar(1, maxHealth, 1, false, ProgressBarStyle.create(Color.White, Color.White));

            //var dropText = new Text(Graphics.instance.bitmapFont, dropCount.ToString(), new Vector2(60, 100), Color.White);
            //dropText.setRenderLayer(SCREEN_SPACE_RENDER_LAYER);
            //healthEntity.addComponent(dropText);    

            healthBarBorder2.setSize(52, 12f);
            healthBarBorder2.setPosition(5f, 4f);
            healthBar.setValue(maxHealth);
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
            Physics.gravity.Y = 250f;
            CheckGrounded();
            
            //Console.WriteLine(playerEntity.transform.position.X + ", " + playerEntity.transform.position.Y);
            CheckDoors();

            var graph = new WeightedGridGraph(_tileCollLayer);

            var path = graph.search(VectorToPoint(enemyEntity.transform.position), VectorToPoint(playerEntity.transform.position));
            var enemy = enemyEntity.entity.getComponent<Enemy>();
        }

        public Point VectorToPoint(Vector2 input)
        {
            return new Point((int)input.X / 16, (int)input.Y / 16);
        }

        private void CheckGrounded()
        {
            CollisionResult res;

            var phys = Physics.getAllColliders();
            var colliders = phys.AsEnumerable();
            var player = playerEntity.entity.getComponent<Player>();

            if (player.activeState == Player.State.Roll)
            {
                Console.WriteLine("SHOULD IGNORE ENEMY COLISSION");
                player.Invuln = true;
                player.entity.colliders[0].isTrigger = true;
            }
            else
            {
                player.Invuln = false;
                player.entity.colliders[0].isTrigger = false;
            }

            foreach (var collider in colliders)
            {
                if (collider.physicsLayer == 100)
                {
                    //drops
                    if (collider.entity.tag == 4)
                    {
                        var drop = collider.entity.getComponent<Drop>();
                        if (drop.Collected)
                        {
                            continue;
                        }
                        drop.SetMoveDirection(drop.transform.position, player.transform.position);
                        drop.ActiveState = Drop.State.Follow;
                        if (collider.entity.colliders[0].overlaps(playerEntity.entity.colliders[0]))
                        {
                            player.UpdateDropCount(drop.Value);
                            Console.WriteLine("DROPCOUNT: " + player.DropCount);
                            drop.entity.detachFromScene();
                        }
                    }
                }
                if (collider.physicsLayer == 1)
                {
                    //enemies
                    if (collider.entity.tag == 5)
                    {
                        var enemy = collider.entity.getComponent<Enemy>();
                        if (enemy.ActiveState == Enemy.State.Attack)
                        {
                            if (enemy._attackCount > 30 && collider.entity.colliders[1].overlaps(playerEntity.entity.colliders[1]))
                            {
                                player.activeState = Player.State.Knockback;
                                Console.WriteLine("OVERLAP");
                            }
                        }
                        if (enemy.Dead)
                        {
                            var pos = enemy.transform.position;
                            for (var i = 0; i < 5; i++)
                            {
                                var random = Nez.Random.random;
                                var posX = pos.X + 1/10*random.Next() + i*5;
                                var posY = pos.Y + 1/10*random.Next() + i*5;
                                var dropEntity = CreateDrop(new Vector2(posX, posY));
                            }
                            collider.entity.detachFromScene();
                            return;
                        }
                        if (collider.entity.getComponent<Enemy>().ActiveState != Enemy.State.Stun)
                        {
                            collider.entity.getComponent<Enemy>().CheckInRange(collider.entity.transform.position, playerEntity.transform.position);
                        }
                        collider.entity.getComponent<Enemy>().SetMoveDirection(collider.entity.transform.position, playerEntity.transform.position);
                    }
                    if (player.activeState == Player.State.Attack)
                    {
                        if (collider.overlaps(playerEntity.entity.colliders[1]) && collider.entity.getComponent<Enemy>().ActiveState != Enemy.State.Stun)
                        {
                            collider.entity.getComponent<Enemy>().ActiveState = Enemy.State.Stun;
                            collider.entity.getComponent<Enemy>().DoHurt(1);
                            Console.WriteLine("HIT");
                        }
                    }
                    if (collider.overlaps(playerEntity.entity.colliders[0]) && playerEntity.entity.colliders[0] != collider && playerEntity.entity.colliders[1] != collider) 
                    {
                        player.activeState = Player.State.Knockback;
                        //player.DoHurt(1);
                        DisplayHealthBar(player.Health, player.MaxHealth, player.DropCount);
                        Console.WriteLine("SHOULD COLLIDE");
                    }
                }
                if (collider.physicsLayer == 10)
                {
                    if (playerEntity.velocity.Y == 0)
                    {
                        player.Grounded = true;
                    }
                }
            }
        }

        private void CheckDoors()
        {
            if (playerEntity.transform.position.X <= 0)
            {
                //go left
                Console.WriteLine("left");
                _world.ChangeRoom(0, -1);

                var newPos = new Vector2(_width - 16, playerEntity.transform.position.Y);
                UpdateTileMap(newPos, true);
            }
            else if (playerEntity.transform.position.X >= _width/_world.activeRoom.roomIndex[1])
            {
                //go right
                Console.WriteLine("right");
                _world.ChangeRoom(0, 1);
                var newPos = new Vector2(16, playerEntity.transform.position.Y);
                UpdateTileMap(newPos, false);
            }
            else if (playerEntity.transform.position.Y <= 0)
            {
                //go up
                Console.WriteLine("up");
                _world.ChangeRoom(-1, 0);
                var newPos = new Vector2(playerEntity.transform.position.X, _height - 16);
                UpdateTileMap(newPos, false);
            }
            else if (playerEntity.transform.position.Y >= _height / _world.activeRoom.roomIndex[0])
            {
                //go down
                Console.WriteLine("down");
                _world.ChangeRoom(1, 0);
                var newPos = new Vector2(playerEntity.transform.position.X, 16);
                UpdateTileMap(newPos, false);
            }
        }

        ArcadeRigidbody CreateDrop(Vector2 position)
        {
            var rigidbody = new ArcadeRigidbody();
            var entity = createEntity("DROP: " + Utils.randomString(3));
            entity.transform.position = position;
            entity.addComponent(new Drop(1));
            entity.addComponent(rigidbody);
            entity.tag = 4;

            var collider = new BoxCollider(-2, -3, 4, 4);
            collider.collidesWithLayers = 10;
            collider.physicsLayer = 100;
            entity.addCollider(collider);

            var hitboxCollider = new BoxCollider(-10, -10, 20, 20);
            //hitboxCollider.isTrigger = true;
            hitboxCollider.collidesWithLayers = 0;
            hitboxCollider.physicsLayer = 100;
            entity.addCollider(hitboxCollider);



            return rigidbody;
        }

        ArcadeRigidbody createRigidEntity(Vector2 position, float mass, float friction, float elasticity, Vector2 velocity, bool isPlayer, bool isEnemy)
        {
            var rigidbody = new ArcadeRigidbody()
                .setMass(mass)
                .setFriction(friction)
                .setElasticity(elasticity)
                .setVelocity(velocity);

            var entity = createEntity(Utils.randomString(3));
            entity.transform.position = position;
            if (isPlayer)
            {
                entity.addComponent(new Player());
            }
            else if (isEnemy)
            {
                entity.addComponent(new Enemy());
            }

            entity.addComponent(rigidbody);
            //entity.addCollider(new CircleCollider(8));
            var collider = new BoxCollider(-9, -6, 13, 16);
            collider.collidesWithLayers = 10;
            entity.addCollider(collider);

            var hitboxCollider = new BoxCollider(6, -14, 20, 24);
            //hitboxCollider.isTrigger = true;
            hitboxCollider.collidesWithLayers = 0;
            hitboxCollider.physicsLayer = 100;
            entity.addCollider(hitboxCollider);

            return rigidbody;
        }


    }
}

