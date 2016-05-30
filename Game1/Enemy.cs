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
        public Enemy(Texture2D texture)
        {
            position = new Vector2(400, 400);
            animatedSprite = new AnimatedSprite(texture, 1, 6, 5);
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
