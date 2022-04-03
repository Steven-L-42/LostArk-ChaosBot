using System.IO;
using System.Xml.Serialization;

namespace PixelAimbot.Classes.Misc
{
    public class Config
    {
        static string configPath = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5() + @"\main.ini";

        public string username { get; set; }
        public string password { get; set; }
        public string hwid { get; set; }
        // ...

        public static Config Load()
        {
            var serializer = new XmlSerializer(typeof(Config));
            using (StreamReader reader = new StreamReader(configPath))
                return (Config)serializer.Deserialize(reader);
        }

        public void Save()
        {
            var serializer = new XmlSerializer(typeof(Config));
            using (var writer = new StreamWriter(configPath))
                serializer.Serialize(writer, this);
        }
    }
}
