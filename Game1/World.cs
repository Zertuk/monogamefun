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
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private string[,] _dungeonArray;
        private Map _map;
        public World(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;

            var dungeon = new Dungeon(5, 5, 10);
            _dungeonArray = dungeon.GenerateDungeon();

            _map = new Map(_content, _spriteBatch);
        }
        public void worldDraw()
        {
            _map.drawMap(_dungeonArray);
        }
        
    }
}
