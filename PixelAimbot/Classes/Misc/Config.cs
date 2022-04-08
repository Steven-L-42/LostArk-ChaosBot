using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PixelAimbot.Classes.Misc
{
    
    
    public class Config
    {
        public static string version { get; set; } = "1.5.8r";
        public string username { get; set; } = "";
        public string password { get; set; } = "";

        public bool chBoxRemember { get; set; } = false;

        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();
        public static string ConfigFileName { get; set; } = "main.ini";

        public static void init()
        {
            string newIni = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5() + @"\main.ini";
            string newConfigurationFolder = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();
            if (!File.Exists(newIni))
            {

                if (!Directory.Exists(newConfigurationFolder))
                {
                    Directory.CreateDirectory(newConfigurationFolder);
                }
                var createdFile = File.Create(newIni);
                createdFile.Close();
            }
        }


        public void Save()
        {
            string output = JsonConvert.SerializeObject(this);
            using (StreamWriter writer = new StreamWriter(Path.Combine(ConfigPath, ConfigFileName)))
            {
                writer.Write(PixelAimbot.frmLogin.blow1.Encrypt_CTR(output));
            }

        }

        public static Config Load()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(ConfigPath, "main.ini")))
            {
                string output = reader.ReadToEnd(); // 
                if (output != "")
                {
                    return JsonConvert.DeserializeObject<Config>(PixelAimbot.frmLogin.blow1.Decrypt_CTR(output));
                }
                
            }
            return new Config();
        }

    }

    public class Rotations
    {
        public string dungeontimer { get; set; } = "65";
        public string left { get; set; } = "LEFT";
        public string right { get; set; } = "RIGHT";
        public string q { get; set; } = "500";
        public string w { get; set; } = "500";
        public string e { get; set; } = "500";
        public string r { get; set; } = "500";
        public string a { get; set; } = "500";
        public string s { get; set; } = "500";
        public string d { get; set; } = "500";
        public string f { get; set; } = "500";
        public string instant { get; set; }
        public string potion { get; set; }
        public bool chboxinstant { get; set; } = false;
        public bool chboxheal { get; set; } = false;
        public bool chboxdungeontimer { get; set; } = false;
        public bool chBoxAutoRepair { get; set; } = false;
        public string autorepair { get; set; } = "10";
        public bool chboxPaladin { get; set; } = false;
        public bool chBoxShadowhunter { get; set; } = false;
        public bool chBoxBerserker { get; set; } = false;
        public string RestartTimer { get; set; } = "25";
        public bool chBoxSaveAll { get; set; } = false;
        public bool chBoxActivateF2 { get; set; } = false;
        public string txtDungeon2search { get; set; } = "7";
        public string txtDungeon2 { get; set; } = "18";
        public string cQ { get; set; } = "500";
        public string cW { get; set; } = "500";
        public string cE { get; set; } = "500";
        public string cR { get; set; } = "500";
        public string cA { get; set; } = "500";
        public string cS { get; set; } = "500";
        public string cD { get; set; } = "500";
        public string cF { get; set; } = "500";

        

        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();
        public static string ConfigFileName { get; set; }

        public void Save(string filename)
        {
            string output = JsonConvert.SerializeObject(this);
            using (StreamWriter writer = new StreamWriter(Path.Combine(ConfigPath, filename + ".ini")))
            {
                writer.Write(PixelAimbot.frmLogin.blow1.Encrypt_CTR(output));
            }
       
        }

        public static Rotations Load(string filename)
        {
            if (File.Exists(Path.Combine(ConfigPath, filename)))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(ConfigPath, filename)))
                {
                    string output = reader.ReadToEnd(); 
                    return JsonConvert.DeserializeObject<Rotations>(PixelAimbot.frmLogin.blow1.Decrypt_CTR(output));
                }
            } else
            {
                MessageBox.Show("File \"" + filename + "\" not found");
                return null;
            }
        }

    }
}