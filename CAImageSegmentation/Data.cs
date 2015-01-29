using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAImageSegmentation
{
    class Data
    {
        Func<List<float[]>, Position, int, List<Position>> neighbourhoodFunc;
        int neighbourRadius = 1;
        List<float[]> data = null;
        State[][] states;

        int width = 0;
        int height = 0;

        public List<float[]> GetData()
        {
            return this.data;
        }

        public Data(List<float[]> data, Func<List<float[]>, Position, int, List<Position>> neighbourhoodFunc, int radius)
        {
            this.data = data;
            this.neighbourhoodFunc = neighbourhoodFunc;
            this.neighbourRadius = radius;

            int rowLength = data[0].Length;
            states = new State[data.Count][];
            for(int i=0; i<data.Count;i++)
                states[i] = new State[rowLength];

            width = rowLength;
            height = data.Count;
        }

        public State GetState(Position pos)
        {
            int[] pos1 = pos.Pos;
            int axes = pos1.Length;
            int x = pos1[0];
            int y = pos1[1];

            State state = states[y][x];
            if (state != null)
                return state;

            // create a new state
            float intensity = data[y][x];
            State s = new State(0, 0, intensity);
            states[y][x] = s;

            return s;
        }

        public void SetState(Position pos, State s)
        {
            int[] pos1 = pos.Pos;
            int axes = pos1.Length;
            int x = pos1[0];
            int y = pos1[1];
            states[y][x] = s;
        }

        public List<Position> GetNeighbours(Position p)
        {
            return neighbourhoodFunc(this.data, p, neighbourRadius);
        }

        public List<int> SeedValues(int numLabels, int numSeeds)
        {
            List<int> labels = new List<int>();
            for (int i = 0; i < numLabels; i++)
            {
                labels.Add(i);
            }

            Random r = new Random();
            for (int i = 0; i < 20; i++)
            {
                int labelIdx = r.Next(0, 20); //for ints
                int x = r.Next(0, width);
                int y = r.Next(0, height);
                float intensity = data[y][x];

                State s = new State(labelIdx, 1, intensity);
                states[y][x] = s;
            }


            return labels;
        }
    }
}
