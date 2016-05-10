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
        Player _player;
        Level _level;
        Map _map;
        Dungeon _dungeon;
        World _world;
        private string[,] _dungeonArray;

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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Score");
            Texture2D texture = Content.Load<Texture2D>("leek");
            Texture2D wallTexture = Content.Load<Texture2D>("wall");
            Texture2D grassTexture = Content.Load<Texture2D>("grass");
            _player = new Player(texture);
            // TODO: use this.Content to load your game content here
            _world = new World(Content, spriteBatch);

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

            _player.Input(state);

            // TODO: Add your update logic here

            score++;

            _player.animatedSprite.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.Transparent);
            _world.worldDraw();
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "FPS: " + frameRate, new Vector2(10, 10), Color.Black);
            spriteBatch.End();

            _player.animatedSprite.Draw(spriteBatch, _player.position);

            base.Draw(gameTime);
        }
    }
}
