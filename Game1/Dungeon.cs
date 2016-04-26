using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Dungeon
    {
        public Dungeon(int width, int height, int deadRooms)
        {
            _width = width;
            _height = height;
            _deadRooms = deadRooms;
        }
        int _width;
        int _height;
        int _deadRooms;
        string[,] _dungeonArray;

        private void ConsoleWriteDungeon()
        {
            for (int i = 0; i < _dungeonArray.GetLength(0); i++)
            {
                for (int k = 0; k < _dungeonArray.GetLength(1); k++)
                {
                    Console.Write(_dungeonArray[i, k]);
                }
                Console.WriteLine();
            }
        }

        public string[,] GenerateDungeon()
        {
            string[,] dungeonArray = new string[_width, _height];
            var deadRoomsAdded = 0;
            var Seed = (int)DateTime.Now.Ticks;
            var rnd = new Random(Seed);
            while (deadRoomsAdded < _deadRooms)
            {
                for (var i = 0; i < _width; i++)
                {
                    for (var j = 0; j < _height; j++)
                    {
                        var random = rnd.Next(1, 100);
                        Console.WriteLine(random);
                        if (random > 95 && dungeonArray[i, j] != "D" && deadRoomsAdded < _deadRooms)
                        {
                            deadRoomsAdded = deadRoomsAdded + 1;
                            dungeonArray[i, j] = "D";
                        }
                        else if (dungeonArray[i, j] != "D")
                        {
                            dungeonArray[i, j] = "_";
                        }
                        random = 0;
                    }
                }
            }
            return dungeonArray;
        }


    }
}
