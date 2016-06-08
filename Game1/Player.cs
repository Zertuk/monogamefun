using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public Vector2 prevPosition;
        public double baseSpeed = 3.5;
        public double health = 3;
        public double maxHealth = 4;
        private bool dashed = false;
        private bool dashing = false;
        private int dashCount = 0;
        private float[] dashUpdateDir;
        private static GameOptions _gameOptions = new GameOptions();
        private int _scaledTile = _gameOptions.scaledTile;
        private double _scale = _gameOptions.scale;
        public double speed;

        public void dash(float[] dirArr)
        {
            if (!dashed)
            {
                dashUpdateDir = dirArr;
                dashing = true;
                dashed = true;
            }
        }

        public void drawHealth(SpriteBatch spriteBatch, ContentManager content)
        {
            var heart = content.Load <Texture2D>("heart");
            var emptyHeart = content.Load<Texture2D>("heartempty");
            for (var i = 0; i < maxHealth; i++)
            {
                if (health < maxHealth && health < i + 1)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(emptyHeart, new Vector2(16 + (i * 48), 16), Color.White);
                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(heart, new Vector2(16 + (i* 48), 16), Color.White);
                    spriteBatch.End();
                }


            }
        }


        public Player(Texture2D texture)
        {
            position = new Vector2(200, 200);
            animatedSprite = new AnimatedSprite(texture, 1, 6, 5);
            speed = baseSpeed * _scale;

    }

    public Rectangle rectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, _scaledTile / 2, _scaledTile / 2);
        }

        private void dashUpdate()
        {
            if (dashing && dashCount < 8)
            {
                position.X = (float)(position.X + dashUpdateDir[0] * 20*_scale);
                position.Y = (float)(position.Y + dashUpdateDir[1] * -20*_scale);
                dashCount = dashCount + 1;
            }
            else if (dashCount > 15)
            {
                dashCount = 0;
                dashed = false;
                dashing = false;
            }
            else
            {
                dashCount = dashCount + 1;
            }
        }

        public bool Input(KeyboardState state, GamePadState padState, bool colCheck)
        {
            var playerMoving = false;
            if (!colCheck)
            {
                prevPosition = position;
                var dCount = 0;
                double y = 0;
                double x = 0;

                if (state.IsKeyDown(Keys.Right) || padState.ThumbSticks.Left.X > 0f)
                {
                    dCount = dCount + 1;
                    x = speed;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    playerMoving = true;
                }
                if (state.IsKeyDown(Keys.Left) || padState.ThumbSticks.Left.X < 0f)
                {
                    dCount = dCount + 1;
                    x = -speed;
                    spriteEffects = SpriteEffects.None;
                    playerMoving = true;
                }
                if (state.IsKeyDown(Keys.Up) || padState.ThumbSticks.Left.Y > 0f)
                {
                    dCount = dCount + 1;
                    y = -speed;
                    playerMoving = true;
                }
                if (state.IsKeyDown(Keys.Down) || padState.ThumbSticks.Left.Y < 0f)
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

            float[] dirArray = { padState.ThumbSticks.Left.X, padState.ThumbSticks.Left.Y };
            if (state.IsKeyDown(Keys.F) || padState.Buttons.X == ButtonState.Pressed)
            {
                dash(dirArray);
            }
            dashUpdate();

            if (!playerMoving)
            {
                animatedSprite.currentFrame = 0;
            }

            return playerMoving;
        }
    }
}
