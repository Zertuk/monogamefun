using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Collision
    {
        private Tile[,] _tileArray;
        public Collision(Tile[,] tileArray)
        {
            _tileArray = tileArray;
        }

        public bool CheckCollision(Player player)
        {
            int playerY;
            int playerX;
            if ((int)player.position.Y / 64  +2> 9)
            {
                playerY = 9;
            }
            else
            {
                playerY = ((int)player.position.Y / 64 + 2);
            }
            if ((int)player.position.X / 64 + 2 > 9)
            {
                playerX = 9;
            }
            else
            {
                playerX = ((int)player.position.X / 64 + 2);
            }


            for (int y = (int)player.position.Y/64; y <= playerY; y++)
            { //If your player is just the size of a single tile then just -2/+2 will do. 9*9 is already an expensive 81 rectangles to check.
                for (int x = (int)player.position.X/64; x <= playerX; x++)
                {
                    if (!_tileArray[x, y].IsPassable)
                    {
                        if (new Rectangle(x*64, y*64, 64, 64).Intersects(player.rectangle()))
                        {
                            //Check farthest valid position and put player there
                            return true;
                        }
                    }

                }
            }
            return false;
        }
    }
}
