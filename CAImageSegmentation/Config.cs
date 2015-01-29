using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CAImageSegmentation
{
    class Config : Dictionary<String, String>
    {
        private String path = null;

        public Config() { }

        public Config(String path) { this.path = path; }

        public int Int(String key)
        {
            if (!ValidateKey(key))
                return -1;

            try
            {
                return Int32.Parse(this[key]);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Could not parse int value: {0} {1} ", key, this[key]);
                throw ex;
            }
        }

        public float Float(String key)
        {
            if (!ValidateKey(key))
                return -1.0f;

            try
            {
                return float.Parse(this[key]);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Could not parse float value: {0}, {1}", key, this[key]);
                throw ex;
            }
        }

        public double Double(String key)
        {
            if (!ValidateKey(key))
                return -1.0d;

            try
            {
                return double.Parse(this[key]);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Could not parse double value: {0}, {1}", key, this[key]);
                throw ex;
            }
        }

        public string Get(String key)
        {
            if (!ValidateKey(key))
                return null;

            return this[key];
        }

        public bool Bool(String key)
        {
            String val = "";
            try
            {
                val = this[key].ToLower();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Key doesn't exist {0}", key);
                throw new Exception("Key doesn't exist: " + key, ex);
            }

            if (val == "true" || val == "y" || val == "yes")
                return true;
            else
                return false;

        }

        public bool ValidateKey(String key)
        {
            if (ContainsKey(key))
                return true;

            Console.WriteLine("Key doesn't exist: {0}", key);
            return false;
        }

        public void Reload()
        {
            this.Clear();

            foreach (var row in File.ReadAllLines(this.path))
            {
                string key = row.Split('=')[0];
                int idx = row.IndexOf('=');
                string val = row.Substring(idx + 1);
                Console.WriteLine("{0}={1}", key, val);
                Add(key, val);
            }

            Console.WriteLine("Loaded {0} properties", this.Keys.Count);
        }

        public static Config LoadConfig(String path)
        {
            Console.WriteLine("Loading config file: {0}", path);
            var config = new Config(path);
            config.Reload();
            return config;
        }
    }

}
