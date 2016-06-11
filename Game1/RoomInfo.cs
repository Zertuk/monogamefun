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
        private Tile[,] _currentLevel;
        private int _x;
        private int _y;
        private Random _rnd;
        public RoomInfo(string levelType, Tile[,] currentLevel, Random rnd)
        {
            var Seed = (int)DateTime.Now.Ticks;
            _rnd = rnd;
            _x = currentLevel.GetLength(0) - 1;
            _y = currentLevel.GetLength(1) - 1;
            _currentLevel = currentLevel;
        }
        public string[,] GenerateEnemies()
        {
            string[,] enemyArray = new string[_x + 1, _y + 1];
            while (curEnemies <= minEnemy)
            {
                for (var i = 0; i < _x; i++)
                {
                    for (var j = 0; j < _y; j++)
                    {
                        var random = _rnd.Next(1, 100);
                        if (curEnemies == maxEnemy)
                        {
                            break;
                        }
                        else if (random < enemyChance && string.IsNullOrEmpty(enemyArray[i, j]) && _currentLevel[i, j].IsSpawnable == true)
                        {
                            enemyArray[i, j] = "enemy";
                            curEnemies = curEnemies + 1;
                        }
                    }
                }
            }
            return enemyArray;
        }
        public void GenerateItems()
        {

        }
    }
}
