using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate
{
    class Room
    {
        public string tilemap;
        public int[] index;
        public Room(int x, int y, string tileMapName)
        {
            tilemap = tileMapName;
            index = new int[] { x, y };
        }
    }
}
