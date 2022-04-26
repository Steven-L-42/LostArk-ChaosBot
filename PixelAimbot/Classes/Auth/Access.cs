using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Windows.Forms;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot.Classes.Auth
{
    public static class Access
    {
        public static void CheckAccessAsyncCall()
        {
            try
            {
                HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                var webClient = new WebClient();
                webClient.CachePolicy = noCachePolicy;

                var values = new NameValueCollection
                {
                    ["username"] = PixelAimbot.frmLogin.username,
                    ["password"] = PixelAimbot.frmLogin.password,
                    ["hwid"] = PixelAimbot.frmLogin.hwid
                };

                webClient.UploadValuesAsync(new Uri("https://admin.symbiotic.link/api/checkUser"), values);
                webClient.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        CheckAccess(e.Result);
                    }
                    catch (Exception ex)
                    {
                        
                        Alert.Show("Webserver currently not Available! Try Again later.", frmAlert.enmType.Error);
                    }
                };
            }
            catch (Exception)
            {
                Alert.Show("Webserver currently not Available! Try Again later.", frmAlert.enmType.Error);
            }
        }

        public static bool CheckAccess(byte[] response)
        {
            try
            {
                var values = new NameValueCollection();
                values["username"] = PixelAimbot.frmLogin.username;
                values["password"] = PixelAimbot.frmLogin.password;
                values["hwid"] = Misc.HWID.Get();
                //   Misc.Config config = new Misc.Config();
                //   config.username = PixelAimbot.frmLogin.blow1.Encrypt_CTR(PixelAimbot.frmLogin.username);
                //   config.password = PixelAimbot.frmLogin.blow1.Encrypt_CTR(PixelAimbot.frmLogin.password);
                //   config.hwid = PixelAimbot.frmLogin.blow1.Encrypt_CTR(PixelAimbot.frmLogin.hwid);

                //                config.Save();

                var responseString = Encoding.Default.GetString(response);
                
                PixelAimbot.frmLogin.LicenceInformations = JObject.Parse(responseString);
                if (PixelAimbot.frmLogin.LicenceInformations["message"].ToString() == "false")
                {
                    Alert.Show("Licence is not active. Please contact an Administrator.", frmAlert.enmType.Error);
                    return false;
                }
                if (PixelAimbot.frmLogin.LicenceInformations["message"].ToString() == "wrong_login")
                {
                    Alert.Show("Username or Password not known. Please contact an Administrator.", frmAlert.enmType.Error);
                    return false;
                }
                if (PixelAimbot.frmLogin.LicenceInformations["message"].ToString() == "hwid")
                {
                    Alert.Show("Your HWID seems changed, please reset it or contact an Administrator.", frmAlert.enmType.Error);
                    return false;
                }

                if (Application.OpenForms.OfType<PixelAimbot.ChaosBot>().Count() == 1)
                    Application.OpenForms.OfType<PixelAimbot.ChaosBot>().First().Close();

                ChaosBot Form = new ChaosBot();
                
                if (PixelAimbot.frmLogin.LicenceInformations["discorduser"].ToString() != "")
                {
                    Form.conf.discorduser = PixelAimbot.frmLogin.LicenceInformations["discorduser"].ToString();
                    Form.conf.Save();
                }

                Form.Show();
                Application.OpenForms.OfType<PixelAimbot.frmLogin>().First().Hide();

                return true;
            }
            catch (WebException)
            {
                Alert.Show("Server is not reachable, please try again later.", frmAlert.enmType.Error);
                return false;
            }
            catch (Exception ex)
            {
                Alert.Show(ex.Message, frmAlert.enmType.Error);
                return false;
            }
        }
    }
}