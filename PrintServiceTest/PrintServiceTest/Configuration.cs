using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintServiceTest
{
    public class Configuration
    {

        public string Work { get; private set; }
        public string Backup { get; private set; }
        public string Finished { get; private set; }
        public bool CanDelete { get; private set; }
        public string Failed { get; private set; }

        private static Configuration _instance = null;
        private Configuration() { }

        public static Configuration GetInstance()
        {
            if (_instance is null)
            {
                _instance = new Configuration();
                string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                _instance.ReadConfig(configPath);
            }

            return _instance;
        }

        private void ReadConfig(string configPath)
        {
            configPath += "/PrintService/";

            if (!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);

            string configName = "config.csv";

            string configFile = Path.Combine(configPath, configName);
            Console.WriteLine(configFile);
            if (!File.Exists(configFile))
            {
                throw new FileNotFoundException("File not found", configFile);
            }

            string[] values = File.ReadAllText(configFile).Split(';');

            Work = values[0];
            Backup = values[1];
            Finished = values[2];
            Failed = values[3];

            for (int i = 0; i < values.Length - 1; i++) // letztes ist kein Verzeichnis, daher -1
            {
                if (!Directory.Exists(values[i]))
                    Directory.CreateDirectory(values[i]);
            }

            if (bool.TryParse(values[4], out bool tmpCanDelete))
            {
                CanDelete = tmpCanDelete;
            }
            else
            {
                CanDelete = false;
            }
        }
    }
}
