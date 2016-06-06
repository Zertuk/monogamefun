using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set;  }
        public int Columns { get; set; }
        public int currentFrame;
        private int totalFrames;
        private int _limiter;
        private int _limiterTop;
        private static GameOptions _gameOptions = new GameOptions();
        private int _scaledTile = _gameOptions.scaledTile;
        private double _scale = _gameOptions.scale;

        public AnimatedSprite(Texture2D texture, int rows, int columns, int limiter)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            _limiter = 0;
            _limiterTop = limiter;
            totalFrames = Rows * Columns;
        }

        public void Update()
        {
            if (_limiter % _limiterTop == 0)
            {
                _limiter = 0;
                currentFrame = currentFrame + 1;
                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
            }
            _limiter = _limiter + 1;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, SpriteEffects spriteEffects)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, (int)(width * _scale), (int)(height*_scale));

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, null, destinationRectangle, sourceRectangle, new Vector2(_scaledTile / 2, _scaledTile / 2), 0, new Vector2((float)_scale, (float)_scale), Color.White, spriteEffects);
            spriteBatch.End();
        }
    }
}
