using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Camera
    {
        private Matrix _transform;
        private Vector2 _pos;
        private Viewport _viewport;

	    public Camera(Viewport viewport)
	    {
	        _pos = Vector2.Zero;
	        _viewport = viewport;
	    }

        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        
        public void Update(Vector2 position)
        {
  	        //Create view matrix
	        _transform = Matrix.CreateRotationZ(0f) *
                         Matrix.CreateScale(new Vector3(1f, 1f, 1)) *
                         Matrix.CreateTranslation(position.X - 1366 / 2f, position.Y - 768 / 2f, 0);
        }
    }
}
