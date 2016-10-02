using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate
{
    class World
    {
        public Room activeRoom;
        private Room[,] _rooms;
        public World()
        {
            var dark = true;
            var notDark = false;
            var enemyInfo = new EnemyInfo[] { new EnemyInfo(new Vector2(100, 50), "beetle"), new EnemyInfo(new Vector2(400, 50), "beetle") };
            _rooms = new Room[,] { 
                { new Room(0, 0, "map", 1, 2, enemyInfo, dark),  new Room(0, 1, "map", 1, 1, enemyInfo, dark),  new Room(0, 2, "map3", 1, 1), },
                { new Room(1, 0, "map2", 1, 1), new Room(1, 1, "map3", 1, 1), new Room(1, 2, "map2", 1, 1) },
            };
            activeRoom = _rooms[0, 0];
        }
        public void ChangeRoom(int y, int x)
        {
            int[] newIndex = { activeRoom.index[0] + y, activeRoom.index[1] + x };
            activeRoom = _rooms[newIndex[0], newIndex[1]];
        }
    }
}
