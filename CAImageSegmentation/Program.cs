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

            String basePath = config.Get("input_folder"); //"C:/Users/ah14aeb/OneDrive/data/output14_15_01_15/abell
            String path = basePath + "/image.png";

            Image im = Image.FromFile(path, false);

            List<float[]> data = new List<float[]>();
            //im.RawFormat

            float maxIntensity = 0.0f;
            float threshold = 0.0f;

            Algorithm a = new Algorithm();
            a.Train(data, maxIntensity, threshold);
            

        }

        public static void LoadImage(String path)
        {
            
        }
    }
}
