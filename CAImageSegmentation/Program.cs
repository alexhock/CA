using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CAImageSegmentation
{
    class Program
    {

        static void Main(string[] args)
        {
            var config = Config.LoadConfig(args[0]);

            String inputFolder = config.Get("input_folder");
            String path = inputFolder + config.Get("image_file");
            String outPath = inputFolder + "output_file.png";
            bool saveAsGrayScale = config.Bool("save_input_as_grayscale", false);
            float threshold = config.Float("threshold", 0.99f);
            int numTrainings = config.Int("num_trainings", 100);
            int numLabels = config.Int("num_labels", 20);
            int numSeedPositions = config.Int("num_seed_positions", 100);

            Bitmap bm = LoadImage(path);
            if (true)
            {
                Bitmap mp = ConvertToGrayScale(bm);
                SaveImage(mp, outPath + "input_as_grayscale.png");
            }

            List<float[]> data = LoadImageAsGrayScale(bm);
            Data cellData = new Data(data);
            List<int> labels = cellData.SeedValues(numLabels, numSeedPositions);
            float maxIntensity = cellData.GetMaxIntensity();
            CA ca = new CA(maxIntensity, threshold);

            DebugSeedPositions(cellData, labels);
            DebugLabels(labels);

            for (int i=0; i<numTrainings;i++)
            {
                IterateCells(ca, cellData, labels, threshold);

                "Iteration: {0} Num state transitions: {1}".Cout(i, ca.NumStateTransitions);
                if (ca.NumStateTransitions == 0)
                    break;
                ca.NumStateTransitions = 0;               
            }

            DebugSeedPositions(cellData, labels);
            DebugLabels(labels);
            SaveOutputImage(cellData, labels, outPath);
        }

        public static void DebugLabels(List<int> labels)
        {
            int i=0;
            foreach (int label in labels)
            {
                Console.WriteLine("Label: {0}  LabelVal: {1}", i++, label);
            }
        }
        public static void DebugSeedPositions(Data cellData, List<int> labels)
        {
            foreach (Position pos in cellData.SeedPositions)
            {
                int x = pos.Pos[0];
                int y = pos.Pos[1];

                if (x >= 94 && x <= 352 &&
                    y >= 26 && y <= 140)
                {
                    CellState cs = cellData.GetCellState(pos);
                    Console.WriteLine("Label: {0}  LabelVal: {3} Strength: {1}  Intensity: {2}", cs.LabelIdx, cs.Strength, cs.Intensity, labels[cs.LabelIdx]);
                }                                
            }
        }

        public static Data IterateCells(CA ca, Data cellData, List<int> labels, float threshold)
        {
            var imageData = cellData.GetData();
            
            int y = 0;
            foreach (float[] row in imageData)
            {
                int x = 0;
                foreach (float pixel_value in row)
                {
                    int[] fp = new int[2];
                    fp[0] = x;
                    fp[1] = y;
                    Position p = new Position(fp);
                    CellState s = cellData.GetCellState(p);
                    ca.ProcessCell(cellData, labels, p, s);
                    x++;
                }
                y++;
            }
            return cellData;
        }

        static void SaveOutputImage(Data data, List<int> labels, String outPath)
        {
            System.Array colorsArray = Enum.GetValues(typeof(KnownColor));
            KnownColor[] allColors = new KnownColor[colorsArray.Length];
            Array.Copy(colorsArray, allColors, colorsArray.Length);


            CellState[][] cellData = data.CellStates;

            int width = cellData[0].Length;
            int height = cellData.Length;

            Bitmap outputImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                CellState[] row = cellData[y];
                for (int x = 0; x < row.Length; x++)
                {
                    CellState cs = row[x];
                    int label = labels[cs.LabelIdx];
                    Color nc = Color.FromName(allColors[label].ToString()); //Color.FromArgb(label, label, label, label);
                    outputImage.SetPixel(x, y, nc);
                }
            }

            outputImage.Save(outPath);
        }

        static List<float[]> LoadImageAsGrayScale(Bitmap bm)
        {
            List<float[]> data = new List<float[]>();

            for (int i = 0; i < bm.Height; i++)
            {
                float[] row = new float[bm.Width];

                for (int j = 0; j < bm.Width; j++)
                {
                    Color oc = bm.GetPixel(j, i);
                    // http://stackoverflow.com/questions/2265910/convert-an-image-to-grayscale
                    float grayScale = (float)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    row[j] = grayScale;
                }

                data.Add(row);
            }

            return data;
        }

        static Bitmap LoadImage(String path)
        {
            "Image file: {0}".Cout(path);

            Image im = Image.FromFile(path, false);
            Bitmap bm = new Bitmap(im);

            "Image width: {0} height: {1}".Cout(bm.Width, bm.Height);

            return bm;

        }

        static void SaveImage(Bitmap image, String path)
        {
            image.Save(path);
        }

        static Bitmap ConvertToGrayScale(Bitmap bm)
        {
            // http://stackoverflow.com/questions/2265910/convert-an-image-to-grayscale
            Bitmap d = new Bitmap(bm.Width, bm.Height);

            for (int i = 0; i < bm.Height; i++)
            {
                for (int j = 0; j < bm.Width; j++)
                {
                    Color oc = bm.GetPixel(j, i);
                    double grayScaled = ((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    int grayScalei = (int)grayScaled;
                    Color nc = Color.FromArgb(oc.A, grayScalei, grayScalei, grayScalei);
                    d.SetPixel(j, i, nc);
                }
            }
            return d;
        }
    }
}
