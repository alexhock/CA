using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAImageSegmentation
{
    class CA
    {
        int neighbourRadius = 1;
        float maxIntensity = -1.0f;
        float threshold = -1.0f;
        //Data data;        
        Func<float, float, float, float, float> stateTransitionFunc;

        public float Threshold
        {
            get { return threshold; }
        }

        public CA(
            //Data data,            
            Func<float, float, float, float, float> stateTransitionFunc,
            //int neighbourRadius,
            float maxIntensity,
            float threshold)
        {
         
            this.stateTransitionFunc = stateTransitionFunc;
            this.maxIntensity = maxIntensity;
            this.threshold = threshold;
        }

        // float StateTransitionFunc(float max_intensity, float intensity_p, float intensity_q, float strength_q)
        public float GetStateTransition(State p_state, State q_state)
        {
            return stateTransitionFunc(this.maxIntensity, p_state.Intensity, q_state.Intensity, q_state.Strength);
        }

    }
}
