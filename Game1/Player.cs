using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Player
    {
        public SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;

        public AnimatedSprite animatedSprite;
        public Vector2 position;
        public string texture;
        public int health;
        public Vector2 prevPosition;
        public double speed = 3.5;


        public Player(Texture2D texture)
        {
            position = new Vector2(200, 200);
            animatedSprite = new AnimatedSprite(texture, 1, 6);
            
        }

        public Rectangle rectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }

        public bool Input(KeyboardState state, bool colCheck)
        {
            var playerMoving = false;
            if (!colCheck)
            {
                prevPosition = position;
                var dCount = 0;
                double y = 0;
                double x = 0;

                if (state.IsKeyDown(Keys.Right))
                {
                    dCount = dCount + 1;
                    x = speed;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    playerMoving = true;
                }
                if (state.IsKeyDown(Keys.Left))
                {
                    dCount = dCount + 1;
                    x = -speed;
                    spriteEffects = SpriteEffects.None;
                    playerMoving = true;
                }
                if (state.IsKeyDown(Keys.Up))
                {
                    dCount = dCount + 1;
                    y = -speed;
                    playerMoving = true;
                }
                if (state.IsKeyDown(Keys.Down))
                {
                    dCount = dCount + 1;
                    y = speed;
                    playerMoving = true;
                }
                if (dCount > 1)
                {
                    var negY = false;
                    if (y < 0)
                    {
                        negY = true;
                        y = y * -1;
                    }
                    var negX = false;
                    if (x < 0)
                    {
                        negX = true;
                        x = x * -1;
                    }
                    var posY = (double)Math.Sqrt(y) + speed/5;
                    var posX = (double)Math.Sqrt(x) + speed/5;
                    if (negX)
                    {
                        posX = posX * -1;
                    }
                    if (negY)
                    {
                        posY = posY * -1;
                    }
                    position.Y += (float)posY;
                    position.X += (float)posX;

                }
                else
                {
                    position.Y += (float)y;
                    position.X += (float)x;
                }

            }
            else
            {
                playerMoving = true;
                position = prevPosition;
            }

            if (!playerMoving)
            {
                animatedSprite.currentFrame = 0;
            }

            return playerMoving;
        }
    }
}
