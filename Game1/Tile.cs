using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Tile
    {
        public bool IsPassable { get; set; }
        public bool IsDoor { get; set; }
        public bool IsSpawnable { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
    }
}
