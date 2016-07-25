using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate
{
    class Gravity
    {
        private int airTime;
        public float calcGrav()
        {
            var y = -1 + airTime * 0.1f;
            airTime = airTime + 1;
            return y;
        }
    }
}
