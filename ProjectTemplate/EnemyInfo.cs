using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate
{
    class EnemyInfo
    {
        public Vector2 SpawnPosition;
        public string Type;
        public EnemyInfo(Vector2 spawnPosition, string type)
        {
            SpawnPosition = spawnPosition;
            Type = type;
        }
    }
}
