using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAImageSegmentation
{
    class Position
    {
        private int[] pos;

        public int[] Pos
        {
            get { return this.pos; }
        }
    
        public Position(int[] pos)
        {
            this.pos = pos;
        }
    }
}
