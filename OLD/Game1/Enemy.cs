﻿using Microsoft.Xna.Framework;
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
        public SpriteEffects spriteEffects;
        private int testage;
        public Enemy(Texture2D texture, int col, int row, int limiter, Vector2 setPos)
        {
            position = setPos;
            animatedSprite = new AnimatedSprite(texture, col, row, limiter);
            testage = 0;
        }

        private void Navigate()
        {

        }

        public void Walk(Vector2 unitPosition, bool inDistance)
        {
            //if (inDistance)
            //{
            //    position.X = position.X - 10;
            //}
            //testage = testage + 1;
            //if (testage < 70)
            //{
            //    position.X = position.X + 2;
            //    spriteEffects = SpriteEffects.None;
            //}
            //else if (testage < 140)
            //{
            //    position.X = position.X - 2;
            //    spriteEffects = SpriteEffects.FlipHorizontally;
            //}
            //else
            //{
            //    testage = 0;
            //}
        }
    }
}
