using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Enemy
    {
        public AnimatedSprite animatedSprite;
        public Vector2 position;
        public Enemy(Texture2D texture, int col, int row, int limiter)
        {
            position = new Vector2(400, 400);
            animatedSprite = new AnimatedSprite(texture, col, row, limiter);
        }

        public void Walk(Vector2 unitPosition, bool inDistance)
        {
            if (inDistance)
            {
                position.X = position.X - 10;
            }
        }
    }
}
