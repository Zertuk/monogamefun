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
        private Tile[,] currentLevel;
        private int _x;
        private int _y;
        public RoomInfo(string levelType, Tile[,] currentLevel)
        {
            var Seed = (int)DateTime.Now.Ticks;
            rnd = new Random(Seed);
            _x = currentLevel.GetLength(0) - 1;
            _y = currentLevel.GetLength(1) - 1;
        }
        public void GenerateEnemies()
        {
            string[,] enemyArray = new string[_x + 1, _y + 1];
            while (curEnemies <= minEnemy)
            {
                for (var i = 0; i < _x; i++)
                {
                    for (var j = 0; j < _y; j++)
                    {
                        var random = rnd.Next(1, 100);
                        if (curEnemies == maxEnemy)
                        {
                            break;
                        }
                        else if (random < enemyChance && string.IsNullOrEmpty(enemyArray[i, j]))
                        {
                            Console.WriteLine("SPAWN HERE");
                            Console.WriteLine(i);
                            Console.WriteLine(j);
                            enemyArray[i, j] = "enemy";
                            curEnemies = curEnemies + 1;
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
