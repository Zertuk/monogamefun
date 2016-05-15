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
        SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;

        public AnimatedSprite animatedSprite;
        public Vector2 position;
        public string texture;
        public int health;
        public Vector2 prevPosition;


        public Player(Texture2D texture)
        {
            position = new Vector2(200, 200);
            animatedSprite = new AnimatedSprite(texture, 1, 1);
            
        }

        public Rectangle rectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }


        public Vector2 Input(KeyboardState state, bool colCheck)
        {
            if (!colCheck)
            {
                prevPosition = position;

                if (state.IsKeyDown(Keys.Right))
                {
                    position.X += 3;
                    spriteEffects = SpriteEffects.None;
                }
                if (state.IsKeyDown(Keys.Left))
                {
                    position.X -= 3;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                if (state.IsKeyDown(Keys.Up))
                {
                    position.Y -= 3;
                }
                if (state.IsKeyDown(Keys.Down))
                {
                    position.Y += 3;
                }
            }
            else
            {
                position = prevPosition;
            }

            return position;
        }
    }
}
