using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coding4Fun.CleanTamagotchi
{
    public class SettingsManager
    {
        public Settings GetSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            
            StreamReader reader = new StreamReader(GetSettingsPath());
            reader.ReadToEnd();
            var settings = (Settings)serializer.Deserialize(reader);
            reader.Close();

            return settings;
        }

        public void SetSettings(Settings settings)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Settings));
            using (StreamWriter xmlTextWriter = new StreamWriter(GetSettingsPath()))
            {
                xs.Serialize(xmlTextWriter, this);
                xmlTextWriter.Flush();
            }
        }

        private string GetSettingsPath()
        {
            var fullPath = this.GetType().Assembly.Location;
            string theDirectory = Path.GetDirectoryName(fullPath);
            return Path.Combine(theDirectory, "config.xml");
        }
    }
}
