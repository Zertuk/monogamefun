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
                playerY = ((int)player.position.Y / 64);
            }
            if ((int)player.position.X / 64 + 2 > 9)
            {
                playerX = 9;
            }
            else
            {
                playerX = ((int)player.position.X / 64);
            }


            for (int y = (int)player.position.Y/64 - 2; y <= playerY + 2; y++)
            {
                for (int x = (int)player.position.X / 64 - 2; x <= playerX + 2; x++)
                {
                    if (x < 0)
                    {
                        x = 0;
                    }
                    if (y < 0)
                    {
                        y = 0;
                    }
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
