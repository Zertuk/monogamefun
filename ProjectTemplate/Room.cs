using Microsoft.Xna.Framework;
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
        public int[] roomIndex;
        public int width;
        public int height;
        public EnemyInfo[] EnemyInfo;
        public bool IsDark;
        public bool IsSnowing;

        public Room(int x, int y, string tileMapName, int roomX, int roomY, EnemyInfo[] enemyInfo = null, bool dark = false, bool snow = false)
        {
            tilemap = tileMapName;
            index = new int[] { x, y };
            roomIndex = new int[] { roomX, roomY };
            EnemyInfo = enemyInfo;
            IsDark = dark;
            IsSnowing = snow;
        }
    }
}
