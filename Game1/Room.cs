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
                    var pos = new Vector2(i * 64, j * 64);
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(tileArray[i,j].Texture, pos, Color.White);
                    _spriteBatch.End();
                }
            }
        }

        public Tile[,] GenerateRoom(int x, int y, bool[] doors)
        {
            _x = x;
            _y = y;

            for (var k = 0; k < doors.Length; k++)
            {
                Console.WriteLine("DOOR" + k);
                Console.WriteLine(doors[k]);
            }

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
                        if (j == 4)
                        {
                            if ((i == _x-1 && doors[1]) || (i == 0 && doors[3]))
                            {
                                roomArray[i, j] = "D";
                                valAssigned = true;
                            }
                            Console.WriteLine("j: " + j);
                            Console.WriteLine("i: " + i);
                        }
                        if (i == 4)
                        {
                            if ((j == _y - 1 && doors[2]) || (j == 0 && doors[0]))
                            {
                                roomArray[i, j] = "D";
                                valAssigned = true;
                            }
                        }
                        if (!valAssigned)
                        {
                            Console.WriteLine("replacing: " + roomArray[i, j]);
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
            var tileArray = new Tile[_x, _y];
            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    var tile = new Tile();
                    Texture2D texture;
                    if (levelArray[i, j] == "W")
                    {
                        texture = _content.Load<Texture2D>("wall");
                        tile.IsPassable = false;
                        tile.IsDoor = false;
                        tile.Texture = texture;
                    }
                    else if (levelArray[i, j] == "F")
                    {
                        texture = _content.Load<Texture2D>("grass");
                        tile.IsPassable = true;
                        tile.IsDoor = false;
                        tile.Texture = texture;
                    }
                    else if (levelArray[i, j] == "D")
                    {
                        texture = _content.Load<Texture2D>("door");
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
