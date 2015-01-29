using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAImageSegmentation
{

    public class CellState
    {
        private int labelidx = -1;
        private float strength = -1;
        private float intensity = 0;

        public int LabelIdx
        {
            get { return labelidx; }
            set { this.labelidx = value; }
        }

        public float Strength
        {
            get { return strength; }
            set { this.strength = value; }
        }

        public float Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        public CellState(int labelidx, float strength, float intensity)
        {
            this.labelidx = labelidx;
            this.strength = strength;
            this.intensity = intensity;
        }
    }
}
