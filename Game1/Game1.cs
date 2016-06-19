using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;
        private int score = 0;
        Tile[,] _tileArray;
        Player _player;
        Enemy _enemy;
        Enemy _bullet;
        Map _map;
        Dungeon _dungeon;
        World _world;
        private string[,] _dungeonArray;
        ItemDrop _itemDrop;
        private GameOptions _gameOptions;
        private double _scale;
        Camera _camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1366;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Score");
            Texture2D texture = Content.Load<Texture2D>("leeks");
            Texture2D bee = Content.Load<Texture2D>("beemanrun");
            Texture2D wallTexture = Content.Load<Texture2D>("wall");
            Texture2D grassTexture = Content.Load<Texture2D>("grass");
            _player = new Player(texture);
            // TODO: use this.Content to load your game content here
            _world = new World(Content, _player);
            _itemDrop = new ItemDrop("heartfloat", Content);
            //_enemy = new Enemy(bee, 1, 4, 12,);
            _tileArray = _world._activeRoom;
            _gameOptions = new GameOptions();
            _scale = _gameOptions.scale;
            _camera = new Camera(GraphicsDevice.Viewport);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            var padState = GamePad.GetState(PlayerIndex.One);
            var collision = new Collision(_tileArray);
            var colCheck = collision.CheckCollision(_player, _world);
            _tileArray = _world._activeRoom;
            var playerMoving = _player.Input(state, padState, colCheck);
            // TODO: Add your update logic here
            _world.WorldUpdate();
            score++;
            if (playerMoving)
            {
                _player.animatedSprite.Update();
            }
            if (_itemDrop != null)
            {
                _itemDrop.animatedSprite.Update();
            }
            //_enemy.animatedSprite.Update();
            base.Update(gameTime);

            //var inDistance = checkDistance(_enemy.position, _player.position);
            //_enemy.Walk(_player.position, inDistance);
            if (_itemDrop != null)
            {
                var inDistance = checkDistance(_itemDrop.position, _player.position);
                var pickUp = _itemDrop.PickUp(_player, inDistance);
                if (pickUp)
                {
                    _itemDrop = null;
                }
            }

            _camera.Update(_player.position);
        }

        private bool checkDistance(Vector2 unitA, Vector2 unitB)
        {
            if (Vector2.Distance(unitA, unitB) < 50*_scale)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.Transparent);
            //_world.DrawEnemies();
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, Matrix.Invert(_camera.Transform));
            _world.worldDraw(spriteBatch);
            _player.animatedSprite.Draw(spriteBatch, _player.position, _player.spriteEffects, true);
            spriteBatch.End();


            //ui
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            spriteBatch.DrawString(font, "FPS: " + frameRate, new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(font, "x: " + _player.rectangle().X, new Vector2(10, 30), Color.Black);
            spriteBatch.DrawString(font, "y: " + _player.rectangle().Y, new Vector2(10, 50), Color.Black);
            _world.uiDraw(spriteBatch);
            //if (_itemDrop != null)
            //{
            //    _itemDrop.animatedSprite.Draw(spriteBatch, _itemDrop.position, SpriteEffects.None, false);
            //}
            _player.drawHealth(spriteBatch, Content);
            spriteBatch.End();
            //_enemy.animatedSprite.Draw(spriteBatch, _enemy.position, _enemy.spriteEffects);

            base.Draw(gameTime);
        }
    }
}
