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
            _rooms = new Room[,] { 
                { new Room(0, 0, "map"), new Room(0, 1, "map3") },
                { new Room(1, 0, "map"), new Room(1, 1, "map") }
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
