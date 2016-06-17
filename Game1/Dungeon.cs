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
            _playerSpawn = false;
        }
        int _width;
        int _height;
        int _deadRooms;
        bool _playerSpawn;
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

        private bool ChooseSpawnTile(Random rnd)
        {
            var random = rnd.Next(1, 100);
            if (random > 95)
            {
                return true;
            }
            return false;
        }

        private void GeneratePlayerSpawn(string[,] dungeonArray, Random rnd)
        {
            for (int i = 0; i < dungeonArray.GetLength(0); i++)
            {
                for (int k = 0; k < dungeonArray.GetLength(1); k++)
                {
                    if (dungeonArray[i, k] == "_" && ChooseSpawnTile(rnd))
                    {
                        dungeonArray[i, k] = "S";
                        _playerSpawn = true;
                        return;
                    }
                }
            }
        }

        private bool CheckLinkedRooms(int x, int y, string[,] dungeonArray)
        {
            if (dungeonArray[x, y] == "S" || dungeonArray[x, y] == "_")
            {
                return true;
            }
            return false;
        }

        public string[,] NewGenerateDungeon()
        {
            string[,] dungeonArray = new string[_width, _height];
            var Seed = (int)DateTime.Now.Ticks;
            var rnd = new Random(Seed);
            var spawnX = rnd.Next(0, _width);
            var spawnY = rnd.Next(0, _height);
            dungeonArray[spawnX, spawnY] = "S";
            var bossX = rnd.Next(0, _width);
            var bossY = rnd.Next(0, _height);
            dungeonArray[bossX, bossY] = "_";
            var deadRoomsAdded = 0;

            return dungeonArray;
        }

        public string[,] GenerateDungeon()
        {
            string[,] dungeonArray = new string[_width, _height];
            var deadRoomsAdded = 0;
            var Seed = (int)DateTime.Now.Ticks;
            var rnd = new Random(Seed);
            var playerSpawn = false;
            while (deadRoomsAdded < _deadRooms)
            {
                for (var i = 0; i < _width; i++)
                {
                    for (var j = 0; j < _height; j++)
                    {
                        var random = rnd.Next(1, 100);
                        if (random > 95 && dungeonArray[i, j] != "D" && dungeonArray[i, j] != "S" && deadRoomsAdded < _deadRooms)
                        {
                            deadRoomsAdded = deadRoomsAdded + 1;
                            dungeonArray[i, j] = "D";
                        }
                        else if (dungeonArray[i, j] != "D" && dungeonArray[i, j] != "S")
                        {
                            dungeonArray[i, j] = "_";
                            if (ChooseSpawnTile(rnd) && !playerSpawn)
                            {
                                dungeonArray[i, j] = "S";
                                playerSpawn = true;
                            }
                        }
                        random = 0;
                    }
                }
            }
            while (!playerSpawn)
            {
                GeneratePlayerSpawn(dungeonArray, rnd);
            }
            return dungeonArray;
        }


    }
}
