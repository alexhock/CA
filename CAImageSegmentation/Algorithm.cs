using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CAImageSegmentation
{
    public class Algorithm
    {

        public CellState[][] Train(List<float[]> data, float threshold)
        {            
            Data d = new Data(data);
            float maxIntensity = d.GetMaxIntensity();
            CA ca = new CA(maxIntensity, threshold);
            List<int> labels = d.SeedValues(20, 40);

            for (int i = 0; i < 400; i++)
            {
                int y = 0;
                foreach (float[] row in data)
                {
                    int x = 0;
                    foreach (float pixel_value in row)
                    {
                        int[] fp = new int[2];
                        fp[0] = x;
                        fp[1] = y;
                        Position p = new Position(fp);
                        CellState s = d.GetCellState(p);
                        ca.ProcessCell(d, labels, p, s);
                        x++;
                    }
                    y++;
                }

                "Iteration: {0} Num state transitions: {1}".Cout(i, ca.NumStateTransitions);

                if (ca.NumStateTransitions == 0)
                    break;
                ca.NumStateTransitions = 0;


            }
            return d.CellStates;
            
        }



    }
}
