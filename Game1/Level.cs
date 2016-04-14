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

        public Level(int x, int y, ContentManager content, SpriteBatch spriteBatch)
        {
            _width = x;
            _height = y;
            _content = content;
            _spriteBatch = spriteBatch;
            GenerateLevel();
        }

        private void Draw(Texture2D texture)
        {
            var rnd = new Random();
            var a = rnd.Next(1, 25);
            var b = rnd.Next(1, 25);
            var random = new Vector2(a, b);
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, random, Color.White);
            _spriteBatch.End();
        }

        private void DisplayLevel()
        {
            for (var i = 0; i < _levelArray.Length; i++)
            {
                Texture2D texture;
                if (_levelArray[i] == "W")
                {
                    //wall texture
                    texture = _content.Load<Texture2D>("wall");

                }
                else
                {
                    //floor texture
                    texture = _content.Load<Texture2D>("grass");
                }
                Draw(texture);

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
                        if (i == (_width * j + 1)) {
                            //wall
                            _levelArray[i] = "W";
                        }
                        else if (i == (_width * j)) {
                            //wall
                            _levelArray[i] = "W";
                        }
                        else if ((i - _width*(_height-1)) >= 0) {
                            //wall
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
