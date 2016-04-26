using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Map
    {
        private string[,] _dungeonArray;
        private SpriteBatch _spriteBatch;
        private ContentManager _content;

        public Map(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;
        }

        public void Draw(string[,] dungeonArray)
        {
            _dungeonArray = dungeonArray;
            for (int i = 0; i < _dungeonArray.GetLength(0); i++)
            {
                for (int k = 0; k < _dungeonArray.GetLength(1); k++)
                {
                    if (_dungeonArray[i,k] == "_")
                    {
                        var pos = new Vector2(500 + (i * 16), 500 + (k * 16));
                        Texture2D texture = _content.Load<Texture2D>("room");
                        _spriteBatch.Begin();
                        _spriteBatch.Draw(texture, pos, Color.White);
                        _spriteBatch.End();
                    }

                }
            }
        }
    }
}
