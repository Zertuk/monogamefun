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
    public class GameScene : DefaultScene
    {
        private ArcadeRigidbody playerEntity;
        private int _width;
        private int _height;
        private Entity _tiledEntity;
        private UICanvas _canvas;
        private string _prevTileMapName;
        private TiledTileLayer _tileCollLayer;
        private bool _isUpdating = false;
        private bool _shouldUpdate = true;
        private Player _player;
        private ProgressBar _healthBar;
        private Entity _healthEntity;
        private int _shakeCount;
        private bool _shaking = false;
        private List<Entity> _shroomList;
        public GameScene()
        {
            _shroomList = new List<Entity>();
            Transform.shouldRoundPosition = false;
            addRenderer(new ScreenSpaceRenderer(100, SCREEN_SPACE_RENDER_LAYER));
            _world = new World();

            playerEntity = CreatePlayer(new Vector2(50, 50));
            _player = playerEntity.entity.getComponent<Player>();
            DisplayHealthBar();


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
            Screen.isFullscreen = false;

            CreateUI();
        }

        private Scene resetstuff()
        {
            return this;
        }

        private void ClearScene()
        {
            //entities.removeAllEntities();
            _shouldUpdate = false;
            while (_isUpdating)
            {
                //wait..
            }
            for (var i = 0; i < entities.Count; i++)
            {
                if (entities[i].tag == 5 || entities[i].tag == 4)
                {
                    entities[i].destroy();
                }
            }
            _shouldUpdate = true;
        }


        private void UpdateTileMap(Vector2 newPos, bool left)
        {
            //only load if actually new room and not just new part of old room
            if (_prevTileMapName != _world.activeRoom.tilemap)
            {
               
                if (_tiledEntity != null)
                {
                    // transitions within the current Scene with a SquaresTransition
                    //var transition = new SquaresTransition();
                    //transition.onScreenObscured = ClearScene();
                    //Core.startSceneTransition(transition);
                    _tiledEntity.destroy();
                    ClearScene();
                }
                _tiledEntity = createEntity("tiled");

                var tiledmap = contentManager.Load<TiledMap>(_world.activeRoom.tilemap);

                try
                {
                    var shrooms = tiledmap.objectGroups[0].objectsWithName("shroom");
                    for (var i = 0; i < shrooms.Count; i++)
                    {
                        CreateShroom(new Vector2(shrooms[i].x, shrooms[i].y - 8));
                    }

                }
                catch
                {

                }
                var tiledMapComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "collision"));
                tiledMapComponent.setLayersToRender(new string[] { "collision" });
                tiledMapComponent.renderLayer = 10;
                tiledMapComponent.physicsLayer = 10;
                //fix me ;-;

                try
                {
                    var spikeComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "spike"));
                    spikeComponent.renderLayer = 1;
                    spikeComponent.physicsLayer = 501;
                    spikeComponent.setLayersToRender("spike");
                }
                catch
                {
                }

                try
                {
                    var ladderComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "ladder"));
                    ladderComponent.renderLayer = 1;
                    ladderComponent.physicsLayer = 500;
                    ladderComponent.setLayersToRender("ladder");
                    //ladderComponent.collisionLayer = null;
                }
                catch {

                }

                try
                {
                    var accentComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "accent"));
                    accentComponent.renderLayer = 0;
                    accentComponent.collisionLayer = null;
                    accentComponent.setLayersToRender("accent");
                }
                catch
                {
                }

                try
                {
                    var bgComponent = _tiledEntity.addComponent(new TiledMapComponent(tiledmap, "bg"));
                    bgComponent.setLayersToRender("bg");
                    bgComponent.renderLayer = 11;
                    bgComponent.collisionLayer = null;
                }
                catch
                {
                }

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
                if (true)
                {
                    SpawnEnemies();
                }
            }
        }

        private void SpawnEnemies()
        {
            if (_world.activeRoom.EnemyInfo != null)
            {
                foreach (var enemy in _world.activeRoom.EnemyInfo)
                {
                    var newEnemy = CreateEnemy(enemy.SpawnPosition, enemy.Type);
                }
            }
        }

        private void UpdateHealthBar(int health, int maxHealth)
        {
        }

        private void CreateUI()
        {
            _canvas = createEntity("ui").addComponent(new UICanvas());
            _canvas.isFullScreen = true;
            _canvas.setRenderLayer(SCREEN_SPACE_RENDER_LAYER);

        }

        private void ShakeCamera()
        {
            if (_shaking)
            {
                if (_shakeCount > 8)
                {
                    _shakeCount = 0;
                    _shaking = false;
                    return;
                }
                _shakeCount = _shakeCount + 1;

                var xAdjustment = Nez.Random.random.Next(-2, 2); // get random number between -15 and 15
                var yAdjustment = Nez.Random.random.Next(-10, 10); // get random number between -15 and 15

                camera.entity.transform.position += new Vector2(xAdjustment, yAdjustment);
            }
        }

        private void DisplayHealthBar()
        {
            var table = _canvas.stage.addElement(new Table());
            table.setFillParent(true);
            _healthBar = new ProgressBar(0, 9, 1, false, ProgressBarStyle.create(Color.Red, Color.Black));
            var healthBarBorder = new ProgressBar(0, 10, 1, false, ProgressBarStyle.create(Color.White, Color.White));
            var healthBarBorder2 = new ProgressBar(0, 10, 1, false, ProgressBarStyle.create(Color.White, Color.White));

            healthBarBorder2.setSize(52, 12f);
            healthBarBorder2.setPosition(5f, 4f);
            _healthBar.setValue(_player.Health);
            healthBarBorder.setSize(52, 12f);
            healthBarBorder.setPosition(5f, 6f);
            _healthBar.setSize(50f, 5f);
            _healthBar.setPosition(6f, 8f);
            table.addElement(healthBarBorder);
            table.addElement(healthBarBorder2);
            table.addElement(_healthBar);
        }

        private void UpdateUIText()
        {
            if (_healthEntity != null)
            {
                _healthEntity.destroy();
            }
            var healthText = new Text(Graphics.instance.bitmapFont, _player.Health.ToString(), new Vector2(45, 7), Color.White);
            var dropText = new Text(Graphics.instance.bitmapFont, "Buttons: " + _player.DropCount.ToString(), new Vector2(6, 18), Color.White);

            _healthEntity = createEntity("healthText");
            healthText.setRenderLayer(SCREEN_SPACE_RENDER_LAYER);
            dropText.setRenderLayer(SCREEN_SPACE_RENDER_LAYER);
            _healthEntity.addComponent(healthText);
            _healthEntity.addComponent(dropText);
        }

        private Scene PlayerDeath()
        {
            var deathScene = new DeathScene();
            return deathScene;
        }



        public void UpdateScene()
        {
            ShakeCamera();
            UpdateUIText();
            _healthBar.setValue(_player.Health - 1);
            if (_player.Dead)
            {
                _healthBar.setStyle(ProgressBarStyle.create(Color.Black, Color.Black));
                var transition = new SquaresTransition(PlayerDeath);
                Core.startSceneTransition(transition);
                Core.scene = PlayerDeath();
            }
            Physics.gravity.Y = 250f;
            CheckGrounded();
            //Console.WriteLine(playerEntity.transform.position.X + ", " + playerEntity.transform.position.Y);
            CheckDoors();

            var graph = new WeightedGridGraph(_tileCollLayer);
        }

        public Point VectorToPoint(Vector2 input)
        {
            return new Point((int)input.X / 16, (int)input.Y / 16);
        }

        //wow refactor me pls
        private void CheckGrounded()
        {
            if (!_shouldUpdate)
            {
                return;
            }
            _isUpdating = true;
            var phys = Physics.getAllColliders();
            var colliders = phys.AsEnumerable();
            var rect = new RectangleF(playerEntity.entity.transform.position.X + 10, playerEntity.entity.transform.position.Y - 20, 20, 4);
            var neighborColliders = Physics.boxcastBroadphaseExcludingSelf(playerEntity.entity.colliders[0]);


            if (_player.activeState == Player.State.Climb)
            {
                playerEntity.shouldUseGravity = false;
            }
            else
            {
                playerEntity.shouldUseGravity = true;
            }

            if (_player.IgnoreGravity)
            {
                //playerEntity.shouldUseGravity = false;
                playerEntity.setVelocity(new Vector2(playerEntity.velocity.X, playerEntity.velocity.Y*0.75f));
            }
            else
            {
                playerEntity.shouldUseGravity = true;
            }


            // loop through and check each Collider for an overlap
            foreach (var collider in neighborColliders)
            {

                if (collider.entity.tag == 30)
                {
                    if (collider.overlaps(_player.entity.colliders[0]))
                    {

                        if (!_player.ShouldBounce)
                        {
                            Console.WriteLine("SHOULD BOUNCE HERE");
                            _player.ShouldBounce = true;
                        }
                    }

                }

                //climbables
                if (collider.overlaps(playerEntity.entity.colliders[2]))
                {
                    if (collider.physicsLayer == 500 && _player.YAxisInput != 0)
                    {
                        _player.activeState = Player.State.Climb;
                        _player.LadderInUse = collider;
                    }
                }
                if (collider.overlaps(playerEntity.entity.colliders[0]))
                {
                    //enemies
                    if (collider.entity.tag == 5)
                    {

                    }
                    //drops
                    if (collider.entity.tag == 4)
                    {

                    }

                    //instadeath like spikes ;-;
                    if (collider.physicsLayer == 501)
                    {
                        if (collider.overlaps(playerEntity.entity.colliders[0]))
                        {
                            _player.Health = 0;
                        }
                    }


                }

            }

            //not on ladder, stop climbing
            if (_player.LadderInUse != null && !_player.LadderInUse.overlaps(playerEntity.entity.colliders[0])&& _player.activeState != Player.State.Float)
            {
                _player.activeState = Player.State.Normal;
                _player.LadderInUse = null;
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
                            drop.entity.destroy();
                            continue;
                        }
                        drop.SetMoveDirection(drop.transform.position, _player.transform.position);
                        drop.ActiveState = Drop.State.Follow;
                        if (collider.entity.colliders[0].overlaps(playerEntity.entity.colliders[0]))
                        {
                            drop.Collected = true;
                            _player.UpdateDropCount(drop.Value);
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
                            if (enemy._attackCount > 0 && collider.entity.colliders[1].overlaps(playerEntity.entity.colliders[0]) && _player.activeState != Player.State.Roll && enemy.AttackCanDamage)
                            {
                                _player.activeState = Player.State.Knockback;

                                if (!_player.IsInvuln)
                                {
                                    _shaking = true;
                                    _player.DoHurt(1);
                                }
                            }
                        }
                        if (enemy.Dead)
                        {
                            var pos = enemy.transform.position;
                            for (var i = 0; i < 5; i++)
                            {
                                var mult = -1;
                                if (i % 2 == 0)
                                {
                                    mult = 1;
                                }

                                var posX = pos.X + 5 * i * mult;
                                var posY = pos.Y - 5 * i * mult - 5;
                                var dropEntity = CreateDrop(new Vector2(posX, posY));
                            }
                            collider.entity.detachFromScene();
                            return;
                        }
                        if (collider.entity.getComponent<Enemy>().ActiveState != Enemy.State.Stun)
                        {
                            collider.entity.getComponent<Enemy>().CheckInRange(collider.entity.transform.position, playerEntity.transform.position);
                        }
                        if (Vector2.Distance(collider.entity.transform.position, _player.transform.position) < 100)
                        {
                            collider.entity.getComponent<Enemy>().SetMoveDirection(collider.entity.transform.position, playerEntity.transform.position);
                        }
                        else
                        {
                            collider.entity.getComponent<Enemy>().ActiveState = Enemy.State.Stun;
                        }
                    }
                    if (_player.activeState == Player.State.Attack)
                    {
                        if (collider.overlaps(playerEntity.entity.colliders[1]) && collider.entity.getComponent<Enemy>().ActiveState != Enemy.State.Stun)
                        {
                            collider.entity.getComponent<Enemy>().ActiveState = Enemy.State.Stun;
                            if (_player.ThirdAttack == true)
                            {
                                collider.entity.getComponent<Enemy>().DoHurt(2);
                                _shaking = true;
                            }
                            else
                            {
                                collider.entity.getComponent<Enemy>().DoHurt(1);
                                _shaking = true;
                            }
                        }
                    }
                    if (collider.overlaps(playerEntity.entity.colliders[0]) && playerEntity.entity.colliders[0] != collider && playerEntity.entity.colliders[1] != collider && _player.activeState != Player.State.Roll && _player.activeState != Player.State.Knockback) 
                    {
                        _player.activeState = Player.State.Knockback;

                        if (!_player.IsInvuln)
                        {
                            _player.DoHurt(1);
                            _shaking = true;
                        }
                        //DisplayHealthBar();
                    }
                }
                if (collider.physicsLayer == 10)
                {
                    if (playerEntity.velocity.Y == 0)
                    {
                        _player.Grounded = true;
                    }
                }
                _isUpdating = false;
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

        ArcadeRigidbody CreateShroom(Vector2 position)
        {
            var rigidBody = new ArcadeRigidbody();
            rigidBody.setMass(0);
            var entity = createEntity("SHROOM: " + Utils.randomString(3));
            entity.transform.position = position;
            entity.addComponent(rigidBody);
            entity.addComponent(new Shroom());
            entity.tag = 30;

            var collider = new BoxCollider(-8, -8, 16, 16);
            collider.collidesWithLayers = 10;
            collider.physicsLayer = 100;
            entity.addCollider(collider);

            return rigidBody;
        }

        ArcadeRigidbody CreateDrop(Vector2 position)
        {
            var rigidbody = new ArcadeRigidbody();
            rigidbody.setElasticity(0.7f);
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

        private ArcadeRigidbody CreatePlayer(Vector2 position)
        {
            var rigidbody = new ArcadeRigidbody()
                                .setMass(50000f)
                                .setFriction(1)
                                .setElasticity(0)
                                .setVelocity(new Vector2(0, 0));


            var entity = createEntity("player");
            entity.tag = 1;
            entity.transform.position = position;
            entity.addComponent(new Player());
            entity.addComponent(rigidbody);

            var collider = new BoxCollider(-6, -6, 13, 16);
            collider.collidesWithLayers = 10;
            //collider.physicsLayer = 101;
            entity.addCollider(collider);

            var hitboxCollider = new BoxCollider(7, -14, 20, 24);
            hitboxCollider.collidesWithLayers = 0;
            hitboxCollider.physicsLayer = 100;
            entity.addCollider(hitboxCollider);

            var climbCollider = new BoxCollider(0, -5, 3, 16);
            climbCollider.collidesWithLayers = 0;
            climbCollider.physicsLayer = 100;
            entity.addCollider(climbCollider);



            return rigidbody;
        }

        private ArcadeRigidbody CreateEnemy(Vector2 position, string enemyType)
        {
            var rigidbody = new ArcadeRigidbody()
                                .setMass(50000f)
                                .setFriction(1)
                                .setElasticity(0)
                                .setVelocity(new Vector2(0, 0));

            var entity = createEntity(enemyType + ": " + Utils.randomString(3));
            entity.transform.position = position;
            entity.addComponent(new Enemy(enemyType));
            entity.tag = 5;
            entity.addComponent(rigidbody);

            var collider = new BoxCollider(-6, -6, 13, 16);
            collider.collidesWithLayers = 10;
            //collider.physicsLayer = 101;
            entity.addCollider(collider);
            var hitboxCollider = new BoxCollider(7, -14, 20, 24);
            hitboxCollider.collidesWithLayers = 0;
            hitboxCollider.physicsLayer = 100;
            entity.addCollider(hitboxCollider);

            return rigidbody;
        }
    }
}

