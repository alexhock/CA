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
        CellState[][] states;

        int width = 0;
        int height = 0;

        public List<float[]> GetData()
        {
            return this.data;
        }

        public CellState[][] CellStates
        {
            get { return states; }
        }

        public Data(List<float[]> data, int radius = 1)
        {
            this.data = data;
            this.neighbourhoodFunc = NeighbourhoodFunc;
            this.neighbourRadius = radius;

            int rowLength = data[0].Length;
            states = new CellState[data.Count][];
            for(int i=0; i<data.Count;i++)
                states[i] = new CellState[rowLength];

            width = rowLength;
            height = data.Count;
        }

        public CellState GetCellState(Position pos)
        {
            int[] pos1 = pos.Pos;
            int axes = pos1.Length;
            int x = pos1[0];
            int y = pos1[1];

            CellState state = states[y][x];
            if (state != null)
                return state;

            // create a new state
            float intensity = data[y][x];
            CellState s = new CellState(0, 0, intensity);
            states[y][x] = s;

            return s;
        }

        public void SetCellState(Position pos, CellState s)
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
            for (int i = 0; i < numSeeds; i++)
            {
                int labelIdx = r.Next(0, numLabels); //for ints
                int x = r.Next(0, width);
                int y = r.Next(0, height);
                float intensity = data[y][x];

                CellState s = new CellState(labelIdx, 1, intensity);
                states[y][x] = s;
            }


            return labels;
        }

        public float GetMaxIntensity()
        {
            float maxIntensity = 0.0f;
            for (int i = 0; i < data.Count; i++)
            {
                float[] row = data[i];
                for (int j = 0; j < row.Length; j++)
                {
                    float intensity = row[j];
                    if (intensity > maxIntensity)
                        maxIntensity = intensity;
                }
            }
            return maxIntensity;
        }

        List<Position> NeighbourhoodFunc(List<float[]> data, Position p, int radius = 1)
        {
            var positions = new List<Position>();

            int height = data.Count;
            int width = data[0].Length;

            int x = p.Pos[0];
            int y = p.Pos[1];

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (j == 0 && i == 0) continue;

                    int[] pos = new int[2];
                    pos[0] = x + i;
                    pos[1] = y + j;

                    if (pos[0] < 0) continue;
                    if (pos[1] < 0) continue;
                    if (pos[0] == width) continue;
                    if (pos[1] == height) continue;

                    positions.Add(new Position(pos));
                }
            }

            return positions;
        }
    }
}
