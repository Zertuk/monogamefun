using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Timers;

namespace Game1
{
    class Level
    {
        private int _width;
        private int _height;
        private ContentManager _content;
        private string[] _levelArray;
        private Tile[] _tileArray;
        private SpriteBatch _spriteBatch;
        private int a;
        private int b;
        private int _drawIndex;

        public Level(int x, int y, ContentManager content, SpriteBatch spriteBatch)
        {
            _width = x;
            _height = y;
            _content = content;
            _spriteBatch = spriteBatch;
            GenerateLevel();
            a = 1;
            b = 1;
            _drawIndex = 0;
        }

        private void Draw()
        {
            for (var i = 0; i < _tileArray.Length ; i++)
            {
                if (i % 10 == 0 && i != 0)
                {
                    _drawIndex = _drawIndex + 1;
                    Console.WriteLine(i + " indexx");
                    Console.WriteLine(_drawIndex);
                }
                var pos = new Vector2((i % 10) * 64, _drawIndex * 64);
                _spriteBatch.Begin();
                _spriteBatch.Draw(_tileArray[i].Texture, pos, Color.White);
                _spriteBatch.End();
            }
            _drawIndex = 0;
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
                _tileArray[i] = tile;
            }
            Draw();
        }

        private void GenerateLevel()
        {
            _levelArray = new string[100];
            _tileArray = new Tile[100];
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
