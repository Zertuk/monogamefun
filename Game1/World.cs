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
        public Tile[,][] _worldArray;
        private Map _map;
        private Tile[] _currentTileArray;
        private Level _level;
        private Tile[,] _tileArray;
        private Room _room;

        public World(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;

            _room = new Room(_content, _spriteBatch);

            var dungeon = new Dungeon(5, 5, 10);
            _dungeonArray = dungeon.GenerateDungeon();

            _map = new Map(_content, _spriteBatch);
            _tileArray = _room.GenerateRoom(10, 10); 
        }

        private void LinkLevelsToDungeon()
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    _currentTileArray = _worldArray[0, 0];
                }
            }
        }

        public void worldDraw()
        {
            _map.drawMap(_dungeonArray);
            _room.Draw(_tileArray);


        }

    }
}
