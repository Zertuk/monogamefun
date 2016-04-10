using Microsoft.Xna.Framework;
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
        public static Vector2 Input(KeyboardState state, Vector2 position)
        {
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 5;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 5;
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
