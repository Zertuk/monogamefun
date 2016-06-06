using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class GameOptions
    {
        public double scale = 1.25;
        public int tileSize = 64;
        public int scaledTile;

        public GameOptions()
        {
            scaledTile = (int)(scale * tileSize);
        }
    }
}

