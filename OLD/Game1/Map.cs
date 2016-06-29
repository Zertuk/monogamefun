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
        private ContentManager _content;

        public Map(ContentManager content)
        {
            _content = content;
        }

        public void drawMap(string[,] dungeonArray, int[] roomIndex, SpriteBatch spriteBatch)
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
                        spriteBatch.Draw(texture, pos, Color.White);
                    }
                    else if (_dungeonArray[i,k] == "S")
                    {
                        texture = _content.Load<Texture2D>("boss");
                        spriteBatch.Draw(texture, pos, Color.White);
                    }
                    //Console.WriteLine("index 1: " + roomIndex[0]);
                    //Console.WriteLine("index 2: " + roomIndex[1]);
                    if (roomIndex[0] == i && roomIndex[1] == k)
                    {

                        texture = _content.Load<Texture2D>("playerpos");
                        spriteBatch.Draw(texture, pos, Color.White);
                    }

                }
            }
        }
    }
}
