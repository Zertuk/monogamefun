using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class World
    {
        public ContentManager _content;
        public int tileSize;
        private SpriteBatch _spriteBatch;
        private string[,] _dungeonArray;
        public Tile[,][,] _worldArray;
        private Map _map;
        public Tile[,] _activeRoom;
        private Tile[,] _tileArray;
        private Room _room;
        private int[] _roomIndex;
        private Player _player;
        private static GameOptions _gameOptions = new GameOptions();
        private int _scaledTile = _gameOptions.scaledTile;
        private string[,][,] _enemyArray;
        private List<Enemy> _activeEnemies;
        private Random _rnd;
        private Collision _collision;

        public World(ContentManager content, SpriteBatch spriteBatch, Player player)
        {
            _rnd = new Random();
            _activeEnemies = new List<Enemy>();
            _roomIndex = new int[2];
            _worldArray = new Tile[5, 5][,];
            _enemyArray = new string[5, 5][,];
            _content = content;
            _spriteBatch = spriteBatch;
            _room = new Room(_content, _spriteBatch);
            _player = player;

            var dungeon = new Dungeon(5, 5, 10);
            _dungeonArray = dungeon.NewGenerateDungeon();

            _map = new Map(_content, _spriteBatch);
            RoomToDungeon();
        }

        public void resetPlayerPosition(string playerDirection)
        {
            switch(playerDirection)
            { 
                case "north":
                    _player.position.Y = (_activeRoom.GetLength(1) - 2) * _scaledTile;
                    break;
                case "south":
                    _player.position.Y = 100;
                    break;
                case "east":
                    _player.position.X = 100;
                    break;
                case "west":
                    _player.position.X = ((_activeRoom.GetLength(0) - 2)) * _scaledTile;
                    break;
            }
            var collision = new Collision(_activeRoom);
            _collision = collision;
            ChangeRooms(_roomIndex[0], _roomIndex[1]);
        }

        public void UseDoor(int x, int y)
        {
            if (x == 0)
            {
                _activeRoom = _worldArray[_roomIndex[0] - 1, _roomIndex[1]];
                _roomIndex[0] = _roomIndex[0] - 1;
                resetPlayerPosition("west");
            }
            if (y == 0)
            {
                _activeRoom = _worldArray[_roomIndex[0], _roomIndex[1] - 1];
                _roomIndex[1] = _roomIndex[1] - 1;
                resetPlayerPosition("north");
            }
            if (x == _activeRoom.GetLength(0) - 1)
            {
                _activeRoom = _worldArray[_roomIndex[0] + 1, _roomIndex[1]];
                _roomIndex[0] = _roomIndex[0] + 1;
                resetPlayerPosition("east");
            }
            if (y == _activeRoom.GetLength(1) - 1)
            {
                _activeRoom = _worldArray[_roomIndex[0], _roomIndex[1] + 1];
                _roomIndex[1] = _roomIndex[1] + 1;
                resetPlayerPosition("south");
            }
        }

        private void ChangeRooms(int x, int y)
        {
            _activeEnemies = new List<Enemy>();
            if (_enemyArray[x, y] != null)
            {
                var spawnEnemy = new SpawnEnemy(_enemyArray[x, y], _content);
                _activeEnemies = spawnEnemy.Spawn();
            }
        }

        private bool[] DoorGenCheck(int x, int y)
        {
            //N E S W
            bool[] doors = { false, false, false, false };

            if (x != _dungeonArray.GetLength(0) - 1)
            {
                //east
                if (_dungeonArray[x + 1, y] == "_" || _dungeonArray[x + 1, y] == "S")
                {
                    doors[1] = true;
                }
            }
            if (x != 0)
            { 
                //west
                if (_dungeonArray[x - 1, y] == "_" || _dungeonArray[x - 1, y] == "S")
                {
                    doors[3] = true;
                }
            }
            if (y != _dungeonArray.GetLength(1)-1)
            {
                //south
                if (_dungeonArray[x, y + 1] == "_" || _dungeonArray[x, y + 1] == "S")
                {
                    doors[2] = true;
                }
            }
            if (y != 0)
            {
                //north
                if (_dungeonArray[x, y - 1] == "_" || _dungeonArray[x, y - 1] == "S")
                {
                    doors[0] = true;
                }
            }

            return doors;
        }

        private void RoomToDungeon()
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    if (_dungeonArray[i,j] == "_")
                    {
                        var doors = DoorGenCheck(i, j);
                        var tileArray = _room.GenerateRoom(9, 9, doors);
                        var roomInfo = new RoomInfo("_", tileArray, _rnd);
                        _enemyArray[i, j] = roomInfo.GenerateEnemies();
                        _worldArray[i, j] = tileArray;
                    }
                    else if (_dungeonArray[i, j] == "S")
                    {
                        var doors = DoorGenCheck(i, j);
                        var tileArray = _room.GenerateRoom(9, 9, doors);
                        var roomInfo = new RoomInfo("_", tileArray, _rnd);
                        _worldArray[i,j] = tileArray;
                        _activeRoom = _worldArray[i,j];
                        var indX = i;
                        var indY = j;
                        _roomIndex[0] = indX;
                        _roomIndex[1] = indY;
                    }
                }
            }
        }

        public void DrawEnemies()
        {
            var count = _activeEnemies.Count();
            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    _activeEnemies[i].animatedSprite.Draw(_spriteBatch, _activeEnemies[i].position, _activeEnemies[i].spriteEffects);
                }
            }
        }

        private void UpdateEnemies()
        {
            var count = _activeEnemies.Count();
            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    _activeEnemies[i].animatedSprite.Update();
                }
            }
        }

        public void WorldUpdate()
        {
            UpdateEnemies();
        }

        public void worldDraw()
        {
            _room.Draw(_activeRoom);
            _map.drawMap(_dungeonArray, _roomIndex);
            DrawEnemies();
        }

    }
}
