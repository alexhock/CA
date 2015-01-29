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

        List<Position> NeighbourhoodFunc(List<float[]> data, Position p, int radius = 1)
        {
            var positions = new List<Position>();

            int height = data.Count;
            int width = data[0].Length;

            int x = p.Pos[0];
            int y = p.Pos[1];

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; i < 2; i++)
                {
                    if (j == 0 && i == 0) continue;

                    int[] pos = new int[2];
                    pos[0] = x + i;
                    pos[1] = y + j;

                    positions.Add(new Position(pos));
                }
            }

            return positions;
        }

        float StateTransitionFunc(float max_intensity, float intensity_p, float intensity_q, float strength_q)
        {
            float x = Math.Abs(intensity_p - intensity_q);

            float g = 1 - (x / max_intensity);

            return g * strength_q;
        }

        public void Train(List<float[]> data, float maxIntensity, float threshold, int radius = 1)
        {
            Data d = new Data(data, NeighbourhoodFunc, radius);
            CA ca = new CA(StateTransitionFunc, maxIntensity, threshold);
            List<int> labels = d.SeedValues(20, 40);

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
                    // copy previous state
                    State s = null; // GetPositionState(p); // new State(label_p, 0.0f, I_p);
                    ProcessPoint(ca, d, labels, p, s);
                    y++;
                }
                x++;
            }
        }

        void ProcessPoint(CA ca, Data data, List<int> labels, Position p, State p_state)
        {
            List<Position> neighbours = data.GetNeighbours(p);
            foreach (Position q in neighbours)
            {
                State q_state = data.GetState(q);

                if (labels[p_state.LabelIdx] != labels[q_state.LabelIdx])
                {
                    float stateTransition = ca.GetStateTransition(p_state, q_state);
                    if (stateTransition > ca.Threshold)
                    {
                        // neighbour q wins so update central point p

                        int new_labelidx_p = q_state.LabelIdx;
                        float new_strength_p = stateTransition;

                        if (p_state.LabelIdx != 0)
                        {
                            // update the label for all other cells with the same label.
                            labels[p_state.LabelIdx] = new_labelidx_p;
                        }

                        // set new state for p
                        p_state.Strength = new_strength_p;
                        p_state.LabelIdx = new_labelidx_p;
                        //SetState(p, p_state);
                    }
                }
            }
        }

        float GetMaxIntensity(float[][] data, int ndim)
        {
            float maxIntensity = 0.0f;
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < ndim; j++)
                {
                    float intensity = data[i][j];
                    if (intensity > maxIntensity)
                        maxIntensity = intensity;
                }
            }
            return maxIntensity;
        }

    }
}
