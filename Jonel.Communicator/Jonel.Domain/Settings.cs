using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jonel.Domain
{
    public class Settings : ISettings
    {
        protected static ISettings settings;

        public int Port { get; set; }

        public Settings()
        {
            Port = 508;
        }

        public void LoadSettings()
        {
            //LoadSettings("settings.json");
        }

        public void LoadSettings(string settingsFilename)
        {
            using (StreamReader r = new StreamReader(settingsFilename))
            {
                string json = r.ReadToEnd();
                settings = JsonConvert.DeserializeObject<Settings>(json);
            }
        }

        public static ISettings Shared
        {
            get
            {
                if (settings == null)
                {
                    settings = new Settings();
                    settings.LoadSettings();
                }

                return settings;
            }
        }
    }

    public interface ISettings
    {
        void LoadSettings();
        void LoadSettings(string settingsFilename);

        int Port { get; set; }
    }
}
