using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Level
    {
        private int _width;
        private int _height;

        public Level(int x, int y)
        {
            _width = x;
            _height = y;
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            string[] levelArray = new string[25];
            for (var i = 0; i < _height*_height; i++)
            {
                if (i <= _width)
                {
                    //wall
                    levelArray[i] = "W";
                }
                else
                {
                    for (var j = 1; j < _height; j++)
                    {
                        if (i == (_width * j + 1)) {
                            //wall
                            levelArray[i] = "W";
                        }
                        else if (i == (_width * j)) {
                            //wall
                            levelArray[i] = "W";
                        }
                        else if ((i - _width*(_height-1)) >= 0) {
                            //wall
                            levelArray[i] = "W";
                        }
                    }
                }
                if (string.IsNullOrEmpty(levelArray[i]))
                {
                    //floor
                    levelArray[i] = "F";
                }
            }
        }
    }
}
