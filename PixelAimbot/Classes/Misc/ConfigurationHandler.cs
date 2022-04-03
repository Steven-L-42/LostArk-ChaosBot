using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelAimbot.Classes.Misc
{
    public static class ConfigurationHandler
    {
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
            try
            {
                if (Config.Load().username != null)
                {
                    PixelAimbot.frmLogin.username = PixelAimbot.frmLogin.blow1.Decrypt_CTR(Config.Load().username);
                }
                if (Config.Load().password != null)
                {
                    PixelAimbot.frmLogin.password = PixelAimbot.frmLogin.blow1.Decrypt_CTR(Config.Load().password);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
