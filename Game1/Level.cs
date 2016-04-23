using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Level
    {
        private int _width;
        private int _height;
        private ContentManager _content;
        private string[] _levelArray;
        private SpriteBatch _spriteBatch;
        private int a;
        private int b;

        public Level(int x, int y, ContentManager content, SpriteBatch spriteBatch)
        {
            _width = x;
            _height = y;
            _content = content;
            _spriteBatch = spriteBatch;
            GenerateLevel();
            a = 1;
            b = 1;
        }

        private void Draw(Tile tile, int i)
        {
            int y;
            if (i < 5)
            {
                y = 0;
            }
            else if (i < 10)
            {
                y = 1;
                i = i - 5;
            }
            else if (i < 15)
            {
                y = 2;
                i = i - 10;
            }
            else if (i < 20)
            {
                y = 3;
                i = i - 15;
            }
            else
            {
                y = 4;
                i = i - 20;
            }
            var pos = new Vector2(i * 64, y*64);
            _spriteBatch.Begin();
            _spriteBatch.Draw(tile.Texture, pos, Color.White);
            _spriteBatch.End();
            Debug.WriteLine("this draws");
        }

        public void DisplayLevel()
        {
            Debug.WriteLine(_levelArray.Length);
            for (var i = 0; i < _levelArray.Length; i++)
            {
                Tile tile = new Tile();
                Texture2D texture;
                if (_levelArray[i] == "W")
                {
                    //wall texture
                    texture = _content.Load<Texture2D>("wall");
                    tile.IsPassable = false;
                    tile.Texture = texture;
                }
                else
                {
                    //floor texture
                    texture = _content.Load<Texture2D>("grass");
                    tile.IsPassable = true;
                    tile.Texture = texture;
                }
                Draw(tile, i);

            }
        }

        private void GenerateLevel()
        {
            _levelArray = new string[25];
            for (var i = 0; i < _height*_height; i++)
            {
                if (i <= _width)
                {
                    //wall
                    _levelArray[i] = "W";
                }
                else
                {
                    for (var j = 1; j < _height; j++)
                    {
                        if (i == (_width * j)) {
                            //wall
                            _levelArray[i] = "W";
                        }
                        else if ((i - _width*(_height-1)) >= 0) {
                            //wall
                            _levelArray[i] = "W";
                        }
                        else if ((i == _width*j -1))
                        {
                            _levelArray[i] = "W";
                        }
                    }
                }
                if (string.IsNullOrEmpty(_levelArray[i]))
                {
                    //floor
                    _levelArray[i] = "F";
                }
            }
            DisplayLevel();
        }
    }
}
