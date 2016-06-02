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

        public bool CheckCollision(Player player, World world)
        {
            var tileSize = 64;
            var scale = 1;
            var scaledTile = (int)(tileSize * scale);
            int playerYMax;
            int playerYMin;
            int playerXMax;
            int playerXMin;
            int rows = _tileArray.GetLength(1);
            int columns = _tileArray.GetLength(0);
            if ((int)player.position.Y / scaledTile >= rows)
            {
                playerYMax = rows - 1;
            }
            else
            {
                playerYMax = ((int)player.position.Y / scaledTile);
            }
            if ((int)player.position.X / scaledTile >= columns)
            {
                playerXMax = columns - 1;
            }
            else
            {
                playerXMax = ((int)player.position.X / scaledTile);
            }
            if ((int)player.position.Y / scaledTile - 2 <= 0)
            {
                playerYMin = 0;
            }
            else
            {
                playerYMin = (int)player.position.Y / scaledTile;
            }
            if ((int)(player.position.X / scaledTile - 2) <= 0)
            {
                playerXMin = 0;
            }
            else
            {
                playerXMin = (int)player.position.X / scaledTile;
            }

            for (int y = playerYMin; y <= playerYMax; y++)
            {
                for (int x = playerXMin; x <= playerXMax; x++)
                {
                    if (!_tileArray[x, y].IsPassable)
                    {
                        if (new Rectangle(x* scaledTile, y* scaledTile, scaledTile, scaledTile).Intersects(player.rectangle()))
                        {
                            //Check farthest valid position and put player there
                            return true;
                        }
                    }
                    if (_tileArray[x,y].IsDoor)
                    {
                        if (new Rectangle(x*scaledTile, y * scaledTile, scaledTile, scaledTile).Intersects(player.rectangle()))
                        {
                            //Check farthest valid position and put player there
                            world.UseDoor(x, y);
                        }
                    }

                }
            }
            return false;
        }
    }
}
