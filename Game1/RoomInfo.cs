using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class RoomInfo
    {
        private int enemyChance = 10;
        private int maxEnemy = 5;
        private int minEnemy = 2;
        private Random rnd;
        private int curEnemies = 0;
        public RoomInfo(string level)
        {
            var Seed = (int)DateTime.Now.Ticks;
            rnd = new Random(Seed);

        }
        public void GenerateEnemies()
        {
            while (curEnemies <= maxEnemy)
            {
                for (var i)
                {
                    for (var j)
                    {
                        if (curEnemies <= maxEnemy)
                        {
                            break;
                        }

                    }
                }
            }
        }
        public void GenerateItems()
        {

        }
    }
}
