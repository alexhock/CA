using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAImageSegmentation
{
    class CA
    {
        float maxIntensity = -1.0f;
        float threshold = -1.0f;
        Func<float, float, float, float, float> stateTransitionFunc;
        int numStateTransitions = 0;

        public float Threshold
        {
            get { return threshold; }
        }

        public int NumStateTransitions
        {
            get { return numStateTransitions; }
            set { numStateTransitions = value; }
        }

        public CA(float maxIntensity, float threshold)
        {
         
            this.stateTransitionFunc = StateTransitionFunc;
            this.maxIntensity = maxIntensity;
            this.threshold = threshold;
        }

        float StateTransitionFunc(float max_intensity, float intensity_p, float intensity_q, float strength_q)
        {
            float x = Math.Abs(intensity_p - intensity_q);

            float g = 1 - (x / max_intensity);

            return g * strength_q;
        }


        public float GetStateTransition(CellState p_state, CellState q_state)
        {
            return stateTransitionFunc(this.maxIntensity, p_state.Intensity, q_state.Intensity, q_state.Strength);
        }


        public void ProcessCell(Data data, List<int> labels, Position p, CellState p_state)
        {
            List<Position> neighbours = data.GetNeighbours(p);
            foreach (Position q in neighbours)
            {
                CellState q_state = data.GetCellState(q);
                if (q_state.LabelIdx == 0) continue;

                if (labels[p_state.LabelIdx] != labels[q_state.LabelIdx])
                {
                    float stateTransition = GetStateTransition(p_state, q_state);
                    if (stateTransition > Threshold)
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

                        numStateTransitions++;
                    }
                }
            }
        }

    }
}
