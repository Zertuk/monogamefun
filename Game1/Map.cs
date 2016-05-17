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

        public void drawMap(string[,] dungeonArray, int[] roomIndex)
        {
            _dungeonArray = dungeonArray;
            for (int i = 0; i < _dungeonArray.GetLength(0); i++)
            {
                for (int k = 0; k < _dungeonArray.GetLength(1); k++)
                {
                    Texture2D texture;
                    var pos = new Vector2(500 + (i * 17), 500 + (k * 17));
                    if (_dungeonArray[i,k] == "_")
                    {
                        texture = _content.Load<Texture2D>("room");
                        _spriteBatch.Begin();
                        _spriteBatch.Draw(texture, pos, Color.White);
                        _spriteBatch.End();
                    }
                    else if (_dungeonArray[i,k] == "S")
                    {
                        texture = _content.Load<Texture2D>("boss");
                        _spriteBatch.Begin();
                        _spriteBatch.Draw(texture, pos, Color.White);
                        _spriteBatch.End();
                    }
                    //Console.WriteLine("index 1: " + roomIndex[0]);
                    //Console.WriteLine("index 2: " + roomIndex[1]);
                    if (roomIndex[0] == i && roomIndex[1] == k)
                    {

                        texture = _content.Load<Texture2D>("playerpos");
                        _spriteBatch.Begin();
                        _spriteBatch.Draw(texture, pos, Color.White);
                        _spriteBatch.End();
                    }

                }
            }
        }
    }
}
