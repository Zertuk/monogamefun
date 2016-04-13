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


        public Player(Texture2D texture)
        {
            position = new Vector2(100, 100);
            animatedSprite = new AnimatedSprite(texture, 4, 4);
            
        }

        public Vector2 Input(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 5;
                spriteEffects = SpriteEffects.None;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 5;
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                position.Y -= 5;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                position.Y += 5;
            }
            return position;
        }
    }
}
