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
        int[] _spawnCoords;
        List<int[]> _roomPositions;
        List<int[]> _coordsChecked;
        bool _playerSpawn;
        bool _dungeonCreating;
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
            _deadRooms = 0;
            _dungeonCreating = true;
            var _dungeonArray = new string[_width, _height];
            _roomPositions = new List<int[]>();
            _coordsChecked = new List<int[]>();
            var seed = (int)DateTime.Now.Ticks;
            var rnd = new Random(seed);
            var spawnX = rnd.Next(0, _width);
            var spawnY = rnd.Next(0, _height);
            _spawnCoords = new int[] { spawnX, spawnY };
            _dungeonArray[spawnX, spawnY] = "S";
            CheckNeighbors(spawnX, spawnY, rnd);
            for (var i = 0; i < _roomPositions.Count(); i++)
            {
                var coords = _roomPositions[i];
                if (_dungeonArray[coords[0], coords[1]] != "S")
                {
                    _dungeonArray[coords[0], coords[1]] = "_";
                }
            }
            return _dungeonArray;
        }


        private void TryRoomCreate(int x, int y, Random rnd)
        {
            int[] coords = { x, y };
            var chance = rnd.Next(0, 100);
            var roll = rnd.Next(0, 100);
            if (_coordsChecked.Contains(coords))
            {
                return;
            }
            _coordsChecked.Add(coords);
            if (roll < chance && _roomPositions.Count() < 8)
            {
                _roomPositions.Add(coords);
                CheckNeighbors(x, y, rnd);
            }
        }
        private void CheckNeighbors(int x, int y, Random rnd)
        {
            int dist;
            var distX = Math.Abs(_spawnCoords[0] - x);
            var distY = Math.Abs(_spawnCoords[1] - y);
            if (distX < distY)
            {
                dist = distY;
            }
            else
            {
                dist = distX;
            }
            int minRooms;
            int maxRooms;
            if (dist == 0)
            {
                minRooms = 2;
                maxRooms = 4;
            }
            else if (dist == 1)
            {
                minRooms = 1;
                maxRooms = 3;
            }
            else if (dist == 2)
            {
                minRooms = 0;
                maxRooms = 3;
            }
            else
            {
                minRooms = 0;
                maxRooms = 2;
            }
            if (x != 0)
            {
                TryRoomCreate(x - 1, y, rnd);
            }
            if (x != _width - 1)
            {
                TryRoomCreate(x  + 1, y, rnd);
            }
            if (y != 0)
            {
                TryRoomCreate(x, y - 1, rnd);
            }
            if (y != _height - 1)
            {
                TryRoomCreate(x, y + 1, rnd);
            }
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
