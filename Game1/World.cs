using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class World
    {
        public ContentManager _content;
        private SpriteBatch _spriteBatch;
        private string[,] _dungeonArray;
        public Tile[,][,] _worldArray;
        private Map _map;
        public Tile[,] _activeRoom;
        private Tile[,] _tileArray;
        private Room _room;
        private int[] _roomIndex;

        public World(ContentManager content, SpriteBatch spriteBatch)
        {
            _roomIndex = new int[2];
            _worldArray = new Tile[5, 5][,];
            _content = content;
            _spriteBatch = spriteBatch;
            _room = new Room(_content, _spriteBatch);

            var dungeon = new Dungeon(5, 5, 10);
            _dungeonArray = dungeon.GenerateDungeon();

            _map = new Map(_content, _spriteBatch);
            RoomToDungeon();
        }

        public void UseDoor(int x, int y)
        {
            if (x == 0)
            {
                _activeRoom = _worldArray[_roomIndex[0] - 1, _roomIndex[1]];
                _roomIndex[0] = _roomIndex[0] - 1;
            }
            if (y == 0)
            {
                _activeRoom = _worldArray[_roomIndex[0], _roomIndex[1] ];
            }
            //if (x == _activeRoom.GetLength(1) - 1)
            //{
            //    _activeRoom = _worldArray[_roomIndex[0] + 1, _roomIndex[1]];
            //}
            Console.WriteLine("USE DOOR HERE");
            Console.WriteLine(x);
            Console.WriteLine(y);
        }

        private bool[] DoorGenCheck(int x, int y)
        {
            //N E S W
            bool[] doors = { false, false, false, false };

            if (x != _dungeonArray.GetLength(0) - 1)
            {
                //east
                if (_dungeonArray[x + 1, y] == "_")
                {
                    doors[1] = true;
                }
            }
            if (x != 0)
            { 
                //west
                if (_dungeonArray[x - 1, y] == "_")
                {
                    doors[3] = true;
                }
            }
            if (y != _dungeonArray.GetLength(1)-1)
            {
                //south
                if (_dungeonArray[x, y + 1] == "_")
                {
                    doors[2] = true;
                }
            }
            if (y != 0)
            {
                //north
                if (_dungeonArray[x, y - 1] == "_")
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
                        //var doors = DoorGenCheck(i, j);
                        //var tileArray = _room.GenerateRoom(15, 9, doors);
                        //_worldArray[i,j] = tileArray;
                    }
                    else if (_dungeonArray[i, j] == "S")
                    {
                        Console.WriteLine("THIS SHOULD RUN ONCE ONLY");
                        var doors = DoorGenCheck(i, j);
                        var tileArray = _room.GenerateRoom(9, 9, doors);
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

        public void worldDraw()
        {
            //_room.Draw(_activeRoom);
            _map.drawMap(_dungeonArray, _roomIndex);
        }

    }
}
