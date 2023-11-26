using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace PixelAimbot.Classes.Misc
{
    public class Config
    {
        //public static string version { get; set; } = "2.8.6r";    // Aktuelle Old und Basis Version
        //public static string version { get; set; } = "3.5.9r";    // Aktuelle Stable Version
        public static string version { get; set; } = "4.0.0 (free)";     // Aktuelle Developer Version

        public string username { get; set; } = "FreeForAll";
        public string password { get; set; } = "DontDecryptThisPassword";
        public string discorduser { get; set; } = "";
        public bool oldversion { get; set; }
        public bool devversion { get; set; }

        private static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();
        private static string ConfigFileName { get; set; } = "main.ini";

        public static void Init()
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
                writer.Write(FrmLogin.Blow1.Encrypt_CTR(output));
            }
        }

        public static Config Load()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(ConfigPath, "main.ini")))
            {
                string output = reader.ReadToEnd(); //
                if (output != "")
                {
                    return JsonConvert.DeserializeObject<Config>(FrmLogin.Blow1.Decrypt_CTR(output));
                }
            }
            return new Config();
        }
    }

    public class Rotations
    {
        public string steampath { get; set; } = @"C:\Program Files (x86)\Steam\steam.exe";
        public string q { get; set; } = "500";
        public string w { get; set; } = "500";
        public string e { get; set; } = "500";
        public string r { get; set; } = "500";
        public string a { get; set; } = "500";
        public string s { get; set; } = "500";
        public string d { get; set; } = "500";
        public string f { get; set; } = "500";
        public string txtRestart { get; set; } = "15";
        public string txtDeath { get; set; } = "70";
        public string txtHeal10 { get; set; } = "F1";
        public string textBoxAutoAttack { get; set; } = "1500";
        public bool radioGerman { get; set; }
        public bool radioEnglish { get; set; } = true;
        public bool chBoxAwakening { get; set; }
        public bool chBoxCrashDetection { get; set; } = true;
        public bool checkBoxDiscordNotifications { get; set; } = true;
        public bool chBoxLeavetimer { get; set; }
        public bool chBoxRevive { get; set; }
        public bool chBoxValtanAltQ { get; set; }
        public bool chBoxNpcRepair { get; set; }
        public bool chBoxCompare { get; set; }
        public bool chBoxAutoMovement { get; set; } = true;
        public bool chBoxCooldownDetection { get; set; } = true;
        public bool chBoxRedStage { get; set; }
        public int cmBoxEsoterik1 { get; set; }
        public int cmBoxEsoterik2 { get; set; }
        public int cmBoxEsoterik3 { get; set; }
        public int cmBoxEsoterik4 { get; set; }

        public int HealthSlider1 { get; set; } = 801;
        public bool chBoxAutoRepair { get; set; }
        public string autorepair { get; set; } = "10";
        public string autologout { get; set; } = "";
        public bool chBoxautologout { get; set; }
        public bool chboxPaladin { get; set; }
        public bool chBoxShadowhunter { get; set; }

        public bool chBoxGlavier { get; set; }
        public bool chBoxSoulfist { get; set; }
        public string txLeaveTimerFloor2 { get; set; } = "165";
        public bool chBoxUnstuckF1 { get; set; } = true;

        public bool chBoxBerserker { get; set; }

        public int cmbDeathblade { get; set; }
        public int cmbBard { get; set; }
        public int cmbDestroyer { get; set; }
        public bool chBoxSorcerer { get; set; }
        public bool chBoxSharpshooter { get; set; }
        public int cmbGunlancer { get; set; }

        public bool chBoxChannelSwap { get; set; }
        public bool chBoxSaveAll { get; set; }
        public bool chBoxActivateF2 { get; set; }
        public string txtDungeon2Search { get; set; } = "5";
        public string txtDungeon2 { get; set; } = "15";
        public string cQ { get; set; } = "500";
        public string cW { get; set; } = "500";
        public string cE { get; set; } = "500";
        public string cR { get; set; } = "500";
        public string cA { get; set; } = "500";
        public string cS { get; set; } = "500";
        public string cD { get; set; } = "500";
        public string cF { get; set; } = "500";
        public string txPq { get; set; } = "1";
        public string txPw { get; set; } = "2";
        public string txPe { get; set; } = "3";
        public string txPr { get; set; } = "4";
        public string txPa { get; set; } = "5";
        public string txPs { get; set; } = "6";
        public string txPd { get; set; } = "7";
        public string txPf { get; set; } = "8";

        public string txCharSelect { get; set; } = "1";

        public bool chBoxDoubleQ { get; set; }
        public bool chBoxDoubleW { get; set; }
        public bool chBoxDoubleE { get; set; }
        public bool chBoxDoubleR { get; set; }
        public bool chBoxDoubleA { get; set; }
        public bool chBoxDoubleS { get; set; }
        public bool chBoxDoubleD { get; set; }
        public bool chBoxDoubleF { get; set; }

        public int mouseButton { get; set; }
        public int cmbHealKey { get; set; }
        public int comboBox1 { get; set; }
        public int cmbHour { get; set; } = DateTime.Now.Hour;
        public int cmbMinute { get; set; } = DateTime.Now.Minute;

        public void Save(string filename)
        {
            string output = JsonConvert.SerializeObject(this);
            /* Save all Configurations additional within Database for future changes */
            // var config = Config.Load();
        
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5() + @"\" + filename + ".ini", output);
            //    HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            //    var webclient = new WebClient();
            //    webclient.CachePolicy = noCachePolicy;
            //    var values = new NameValueCollection
            //    {
            //        ["user"] = config.username,
            //        ["settings"] = FrmLogin.Blow1.Encrypt_CTR(output),
            //        ["name"] = filename,
            //    };

            //    webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //    webclient.UploadValues(new Uri("https://about-steven.de/Download/updateOrCreateRotation"), "POST", values);
        }

        public static Rotations Load(string filename)
        {
          
           
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), HWID.GetAsMD5(), filename);

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"The file {filename} was not found at {filePath}");
                }

                var data = File.ReadAllText(filePath);
         
                return JsonConvert.DeserializeObject<Rotations>(data);
            


            //var config = Config.Load();

            //HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            //var webclient = new WebClient();
            //webclient.CachePolicy = noCachePolicy;
            //var values = new NameValueCollection
            //{
            //    ["user"] = config.username,
            //    ["name"] = filename,
            //};
            //webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //var result = webclient.UploadValues(new Uri("https://admin.symbiotic.link/api/getRotation"), "POST", values);
            //return JsonConvert.DeserializeObject<Rotations>(FrmLogin.Blow1.Decrypt_CTR(Encoding.Default.GetString(result)));



        }
    }
}