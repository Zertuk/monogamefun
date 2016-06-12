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
    class SpawnEnemy
    {
        private string[,] _enemyArray;
        private int _x;
        private int _y;
        private ContentManager _content;
        private static GameOptions _gameOptions = new GameOptions();
        private int _scaledTile = _gameOptions.scaledTile;
        private double _scale = _gameOptions.scale;

        public SpawnEnemy(string[,] enemyArray, ContentManager content)
        {
            _enemyArray = enemyArray;
            _x = _enemyArray.GetLength(0);
            _y = _enemyArray.GetLength(1);
            _content = content;
        }

        public List<Enemy> Spawn()
        {
            var activeEnemies = new List<Enemy>();
            Texture2D bee = _content.Load<Texture2D>("beemanrun");
            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    if (!string.IsNullOrEmpty(_enemyArray[i, j]))
                    {
                        //var enemyPos = new Vector2(_scaledTile * i, _scaledTile * j);
                        //var enemy = new Enemy(bee, 1, 4, 12, enemyPos);
                        //activeEnemies.Add(enemy);
                    }
                }
            }
                var enemyPos = new Vector2(300, 300);
                var enemy = new Enemy(bee, 1, 4, 12, enemyPos);

                activeEnemies.Add(enemy);
            return activeEnemies;
        }

    }
}
