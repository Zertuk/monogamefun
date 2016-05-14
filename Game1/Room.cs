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
        int _drawIndex;
        ContentManager _content;
        SpriteBatch _spriteBatch;

        public Room(int x, int y, ContentManager content, SpriteBatch spriteBatch)
        {
            _drawIndex = 0;
            _x = x;
            _y = y;
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

        public Tile[,] GenerateRoom()
        {
            var roomArray = new string[_x, _y];

            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    roomArray[i, j] = "F";
                    if (i == 0 || i == _y - 1 || j == 0 || j == _x - 1)
                    {
                        roomArray[i, j] = "W";
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
                    if (levelArray[i,j] == "W")
                    {
                        texture = _content.Load<Texture2D>("wall");
                        tile.IsPassable = false;
                        tile.Texture = texture;
                    }
                    else if (levelArray[i,j] == "F")
                    {
                        texture = _content.Load<Texture2D>("grass");
                        tile.IsPassable = true;
                        tile.Texture = texture;
                    }
                    tileArray[i, j] = tile;
                }
            }
            return tileArray;
        }
    }
}
