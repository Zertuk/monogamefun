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
            int playerYMax;
            int playerYMin;
            int playerXMax;
            int playerXMin;
            int rows = _tileArray.GetLength(1);
            int columns = _tileArray.GetLength(0);
            Debug.WriteLine("columns: " + columns);
            Debug.WriteLine("rows: " + rows);
            if ((int)player.position.Y / 64 + 2 >= rows)
            {
                playerYMax = rows - 1;
            }
            else
            {
                playerYMax = ((int)player.position.Y / 64);
            }
            if ((int)player.position.X / 64 + 2 >= columns)
            {
                playerXMax = columns - 1;
            }
            else
            {
                playerXMax = ((int)player.position.X / 64);
            }
            if ((int)player.position.Y / 64 - 2 <= 0)
            {
                playerYMin = 0;
            }
            else
            {
                playerYMin = (int)player.position.Y / 64 - 2;
            }
            if ((int)(player.position.X / 64 - 2) <= 0)
            {
                playerXMin = 0;
            }
            else
            {
                playerXMin = (int)player.position.X / 64 - 2;
            }

            Debug.WriteLine("X: " + playerXMax + " , " + playerXMin);
            Debug.WriteLine("Y: " + playerYMax + " , " + playerYMin);

            for (int y = playerYMin; y <= playerYMax; y++)
            {
                for (int x = playerXMin; x <= playerXMax; x++)
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
