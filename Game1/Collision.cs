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
        private static GameOptions _gameOptions = new GameOptions();
        private int _scaledTile = _gameOptions.scaledTile;

        public Collision(Tile[,] tileArray)
        {
            _tileArray = tileArray;
        }

        public bool CheckCollision(Player player, World world)
        {
            int playerYMax;
            int playerYMin;
            int playerXMax;
            int playerXMin;
            int rows = _tileArray.GetLength(1);
            int columns = _tileArray.GetLength(0);
            if ((int)player.position.Y / _scaledTile >= rows)
            {
                playerYMax = rows;
            }
            else
            {
                playerYMax = ((int)player.position.Y / _scaledTile);
            }
            if ((int)player.position.X / _scaledTile >= columns)
            {
                playerXMax = columns;
            }
            else
            {
                playerXMax = ((int)player.position.X / _scaledTile);
            }
            if ((int)player.position.Y / _scaledTile - 2 <= 0)
            {
                playerYMin = 0;
            }
            else
            {
                playerYMin = (int)player.position.Y / _scaledTile;
            }
            if ((int)(player.position.X / _scaledTile - 2) <= 0)
            {
                playerXMin = 0;
            }
            else
            {
                playerXMin = (int)player.position.X / _scaledTile;
            }

            for (int y = playerYMin; y <= playerYMax; y++)
            {
                for (int x = playerXMin; x <= playerXMax; x++)
                {
                    if (!_tileArray[x, y].IsPassable)
                    {
                        if (new Rectangle(x * _scaledTile, y * _scaledTile, _scaledTile, _scaledTile).Intersects(player.rectangle()))
                        {
                            //Check farthest valid position and put player there
                            return true;
                        }
                    }
                    Console.WriteLine(_tileArray[x,y].IsDoor);
                    if (_tileArray[x,y].IsDoor)
                    {
                        if (new Rectangle(x * _scaledTile, y * _scaledTile, _scaledTile, _scaledTile).Intersects(player.rectangle()))
                        {
                            world.UseDoor(x, y);
                        }
                    }

                }
            }
            return false;
        }
    }
}
