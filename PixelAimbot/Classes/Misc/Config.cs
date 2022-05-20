using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using Newtonsoft.Json;

namespace PixelAimbot.Classes.Misc
{
    
    
    public class Config
    {
        public static string version { get; set; } = "3.0.4r";
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public string discorduser { get; set; } = "";
        public bool oldversion { get; set; } = false;

        public string telegram { get; set; } = "";

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
        public bool radioGerman { get; set; } = false;
        public bool radioEnglish { get; set; } = true;
        public bool chBoxAwakening { get; set; } = false;
        public bool chBoxCrashDetection { get; set; } = true;
        public bool checkBoxDiscordNotifications { get; set; } = true;
        public bool chBoxLeavetimer { get; set; } = false;
        public bool chBoxRevive { get; set; } = false;
        public bool chBoxValtanAltQ { get; set; } = false;
        public bool chBoxNPCRepair { get; set; } = false;
        public bool chBoxCompare { get; set; } = false;
        public bool chBoxAutoMovement { get; set; } = false;
        public bool chBoxCooldownDetection { get; set; } = true;   

        public int HealthSlider1 { get; set; } = 801;
        public bool chboxdungeontimer { get; set; } = false;
        public bool chBoxAutoRepair { get; set; } = false;
        public string autorepair { get; set; } = "10";
        public string autologout { get; set; } = "";
        public bool chBoxautologout { get; set; } = false;
        public bool chboxPaladin { get; set; } = false;
        public bool chBoxShadowhunter { get; set; } = false;
        public bool chBoxBard { get; set; } = false;
        public bool chBoxGlavier { get; set; } = false;
        public bool chBoxGunlancer2 { get; set; } = false;
        public bool chBoxSoulfist { get; set; } = false;
        public string txLeaveTimerFloor3 { get; set; } = "180";
        public string txLeaveTimerFloor2 { get; set; } = "165";
        public bool chBoxUnstuckF1 { get; set; } = true;

        public bool chBoxBerserker { get; set; } = false;

        public bool chBoxDeathblade { get; set; } = false;
        public bool chBoxDeathblade2 { get; set; } = false;
        public bool chBoxSorcerer { get; set; } = false;
        public bool chBoxSharpshooter { get; set; } = false;
        public bool chBoxActivateF3 { get; set; } = false;
        public bool chBoxGunlancer { get; set; } = false;

        public string txtLEFT { get; set; } = "LEFT";
        public bool chBoxChannelSwap { get; set; } = false;
        public bool chBoxSaveAll { get; set; } = false;
        public bool chBoxActivateF2 { get; set; } = false;
        public string txtDungeon2search { get; set; } = "5";
        public string txtDungeon3search { get; set; } = "10";
        public string txtDungeon3 { get; set; } = "20";
        public string txtDungeon2 { get; set; } = "15";
        public string cQ { get; set; } = "500";
        public string cW { get; set; } = "500";
        public string cE { get; set; } = "500";
        public string cR { get; set; } = "500";
        public string cA { get; set; } = "500";
        public string cS { get; set; } = "500";
        public string cD { get; set; } = "500";
        public string cF { get; set; } = "500";
        public string txPQ { get; set; } = "1";
        public string txPW { get; set; } = "2";
        public string txPE { get; set; } = "3";
        public string txPR { get; set; } = "4";
        public string txPA { get; set; } = "5";
        public string txPS { get; set; } = "6";
        public string txPD { get; set; } = "7";
        public string txPF { get; set; } = "8";
        public bool chBoxDoubleQ { get; set; } = false;
        public bool chBoxDoubleW { get; set; } = false;
        public bool chBoxDoubleE { get; set; } = false;
        public bool chBoxDoubleR { get; set; } = false;
        public bool chBoxDoubleA { get; set; } = false;
        public bool chBoxDoubleS { get; set; } = false;
        public bool chBoxDoubleD { get; set; } = false;
        public bool chBoxDoubleF { get; set; } = false;

        public int mouseButton { get; set; } = 0;
        public int cmbHealKey { get; set; } = 0;
        public int comboBox1 { get; set; } = 0;
        public int cmbHOUR { get; set; } = DateTime.Now.Hour;
        public int cmbMINUTE { get; set; } = DateTime.Now.Minute;

        public static string ConfigPath { get; set; } = Directory.GetCurrentDirectory() + @"\" + HWID.GetAsMD5();
        public static string ConfigFileName { get; set; }

        public void Save(string filename)
        {
            string output = JsonConvert.SerializeObject(this);
          //  using (StreamWriter writer = new StreamWriter(Path.Combine(ConfigPath, filename + ".ini")))
          //  {
          //      writer.Write(PixelAimbot.frmLogin.blow1.Encrypt_CTR(output));
          //  }

            /* Save all Configurations additional within Database for future changes */
            var config = Config.Load();
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            var webclient = new WebClient();
            webclient.CachePolicy = noCachePolicy;
            var values = new NameValueCollection
            {
                ["user"] = config.username,
                ["settings"] = PixelAimbot.frmLogin.blow1.Encrypt_CTR(output),
                ["name"] = filename,
            };
            webclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webclient.UploadValues(new Uri("https://admin.symbiotic.link/api/updateOrCreateRotation"), "POST", values);
        }

        public static Rotations Load(string filename)
        {

            var config = Config.Load();
            
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            var webclient = new WebClient();
            webclient.CachePolicy = noCachePolicy;
            var values = new NameValueCollection
            {
                ["user"] = config.username,
                ["name"] = filename,
            };
            webclient.Headers.Add("Content-Type","application/x-www-form-urlencoded");
            var result = webclient.UploadValues(new Uri("https://admin.symbiotic.link/api/getRotation"), "POST", values);
            return JsonConvert.DeserializeObject<Rotations>(PixelAimbot.frmLogin.blow1.Decrypt_CTR(Encoding.Default.GetString(result)));
            
            /*
            if (File.Exists(Path.Combine(ConfigPath, filename)))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(ConfigPath, filename)))
                {
                    string output = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Rotations>(PixelAimbot.frmLogin.blow1.Decrypt_CTR(output));
                }
            }
            else
            {
                Alert.Show("File \"" + filename + "\" not found", frmAlert.enmType.Error);
                return null;
            }*/
        }

    }
}