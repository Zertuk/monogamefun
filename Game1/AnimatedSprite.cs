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
        private int limiter;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            limiter = 0;
            totalFrames = Rows * Columns;
        }

        public void Update()
        {
            if (limiter % 4 == 0)
            {
                limiter = 0;
                currentFrame = currentFrame + 1;
                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
            }
            limiter = limiter + 1;
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
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(32, 32), spriteEffects, 0f);
            spriteBatch.End();
        }
    }
}
