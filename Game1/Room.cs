using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Room
    {
        int _x;
        int _y;
        ContentManager _content;
        SpriteBatch _spriteBatch;
        private static GameOptions _gameOptions = new GameOptions();
        private int _scaledTile = _gameOptions.scaledTile;
        private double _scale = _gameOptions.scale;

        public Room(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;
        }

        public void Draw(Tile[,] tileArray)
        {
            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(tileArray[i, j].Texture, tileArray[i, j].Position, null, null, new Vector2(0, 0), 0, new Vector2((float)_scale, (float)_scale), Color.White, SpriteEffects.None);
                    _spriteBatch.End();
                }
            }
        }

        public Tile[,] GenerateRoom(int x, int y, bool[] doors)
        {
            _x = x;
            _y = y;

            var roomArray = new string[_x, _y];

            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    roomArray[i, j] = "F";
                    if (i == 0 || i == _x - 1 || j == 0 || j == _y - 1)
                    {

                        //((int)Math.Floor((double)_x / 2))
                        //((int)Math.Floor((double)_y/2))
                        var valAssigned = false;
                        if (j == Math.Floor((double)_y/2) - 1 || j == Math.Floor((double)_y / 2))
                        {
                            if ((i == _x-1 && doors[1]) || (i == 0 && doors[3]))
                            {
                                roomArray[i, j] = "D";
                                valAssigned = true;
                            }
                        }
                        if (i == Math.Floor((double)_x / 2) - 1 || i == Math.Floor((double)_x / 2))
                        {
                            if ((j == _y - 1 && doors[2]) || (j == 0 && doors[0]))
                            {
                                roomArray[i, j] = "D";
                                valAssigned = true;
                            }
                        }
                        if (!valAssigned)
                        {
                            roomArray[i, j] = "W";
                        }
                    }
                }
            }
            var tileArray = GenerateTileArray(roomArray);
            return tileArray;
        }

        private Tile[,] GenerateTileArray(string[,] levelArray)
        {
            var firstNorthDoor = false;
            var tileArray = new Tile[_x, _y];
            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    var tile = new Tile();
                    Texture2D texture;
                    tile.Position = new Vector2(i * _scaledTile, j * _scaledTile);
                    if (levelArray[i, j] == "W")
                    {
                        if (j == 0)
                        {
                            texture = _content.Load<Texture2D>("grassN1");
                        }
                        else
                        {
                            tile.Position = new Vector2(i * _scaledTile, j * _scaledTile - 20);
                            texture = _content.Load<Texture2D>("grassT1");
                        }
                        tile.IsPassable = false;
                        tile.IsDoor = false;
                        tile.Texture = texture;
                    }
                    else if (levelArray[i, j] == "F")
                    {
                        texture = _content.Load<Texture2D>("grass2");
                        tile.IsPassable = true;
                        tile.IsDoor = false;
                        tile.IsSpawnable = true;
                        tile.Texture = texture;
                    }
                    else if (levelArray[i, j] == "D")
                    {
                        if (j == 0 && !firstNorthDoor)
                        {
                            firstNorthDoor = true;
                            texture = _content.Load<Texture2D>("doorN1");
                        }
                        else if (j == 0)
                        {
                            texture = _content.Load<Texture2D>("doorN2");
                        }
                        else
                        {
                            texture = _content.Load<Texture2D>("door");
                        }
                        tile.IsPassable = true;
                        tile.IsDoor = true;
                        tile.Texture = texture;
                    }
                    tileArray[i, j] = tile;
                }
            }
            return tileArray;
        }
    }
}
