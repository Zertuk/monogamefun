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
        private Tile[,] _activeRoom;
        private Tile[,] _tileArray;
        private Room _room;

        public World(ContentManager content, SpriteBatch spriteBatch)
        {
            _worldArray = new Tile[5,5][,];
            _content = content;
            _spriteBatch = spriteBatch;
            _room = new Room(_content, _spriteBatch);

            var dungeon = new Dungeon(5, 5, 10);
            _dungeonArray = dungeon.GenerateDungeon();

            _map = new Map(_content, _spriteBatch);
            _tileArray = _room.GenerateRoom(10, 10);
            RoomToDungeon();
        }

        private void RoomToDungeon()
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    if (_dungeonArray[i,j] == "_")
                    {
                        var tileArray = _room.GenerateRoom(20, 10);
                        _worldArray[i,j] = tileArray;
                    }
                    else if (_dungeonArray[i, j] == "S")
                    {
                        var tileArray = _room.GenerateRoom(20, 10);
                        _worldArray[i,j] = tileArray;
                        _activeRoom = tileArray;
                    }
                }
            }
        }

        public void worldDraw()
        {
            _room.Draw(_activeRoom);
            _map.drawMap(_dungeonArray);

        }

    }
}
