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
        public double speed = 3.5;
        public double health = 3;
        private double maxHealth = 4;
        private bool dashed = false;
        private bool dashing = false;
        private int dashCount = 0;
        private int[] dashUpdateDir;


        public void dash(string direction)
        {
            if (!dashed)
            {
                switch (direction)
                {
                    case "right":
                        dashUpdateDir = new int[] { 1, 0 };
                        break;
                    case "left":
                        dashUpdateDir = new int[] { -1, 0 };
                        break;
                    case "up":
                        dashUpdateDir = new int[] { 0, -1 };
                        break;
                    case "down":
                        dashUpdateDir = new int[] { 0, 1 };
                        break;
                    case "leftup":
                        dashUpdateDir = new int[] { -1, -1 };
                        break;
                    case "leftdown":
                        dashUpdateDir = new int[] { -1, 1 };
                        break;
                    case "rightup":
                        dashUpdateDir = new int[] { 1, -1 };
                        break;
                    case "rightdown":
                        dashUpdateDir = new int[] { 1, 1 };
                        break;
                }
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
            
        }

        public Rectangle rectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }

        private void dashUpdate()
        {
            if (dashing && dashCount < 5)
            {
                position.X = position.X + dashUpdateDir[0] * 20;
                position.Y = position.Y + dashUpdateDir[1] * 20;
                dashCount = dashCount + 1;
            }
            else
            {
                dashCount = 0;
                dashed = false;
                dashing = false;
            }
        }

        public bool Input(KeyboardState state, GamePadState padState, bool colCheck)
        {
            var direction = "";
            var playerMoving = false;
            if (!colCheck)
            {
                prevPosition = position;
                var dCount = 0;
                double y = 0;
                double x = 0;

                if (state.IsKeyDown(Keys.Right) || padState.DPad.Right == ButtonState.Pressed)
                {
                    dCount = dCount + 1;
                    x = speed;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                    playerMoving = true;
                    direction = "right";
                }
                if (state.IsKeyDown(Keys.Left) || padState.DPad.Left == ButtonState.Pressed)
                {
                    dCount = dCount + 1;
                    x = -speed;
                    spriteEffects = SpriteEffects.None;
                    playerMoving = true;
                    direction = "left";
                }
                if (state.IsKeyDown(Keys.Up) || padState.DPad.Up == ButtonState.Pressed)
                {
                    dCount = dCount + 1;
                    y = -speed;
                    playerMoving = true;
                    direction = direction + "up";
                }
                if (state.IsKeyDown(Keys.Down) || padState.DPad.Down == ButtonState.Pressed)
                {
                    dCount = dCount + 1;
                    y = speed;
                    playerMoving = true;
                    direction = direction + "down";
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

            if (state.IsKeyDown(Keys.F) || padState.Buttons.X == ButtonState.Pressed)
            {
                dash(direction);
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
