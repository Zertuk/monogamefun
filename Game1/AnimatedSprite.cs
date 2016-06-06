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
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, null, destinationRectangle, sourceRectangle, new Vector2(32, 32), 0, new Vector2((float)1, (float)1), Color.White, spriteEffects);
            spriteBatch.End();
        }
    }
}
