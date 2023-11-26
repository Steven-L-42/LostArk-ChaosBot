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
                    ["username"] = FrmLogin.Username,
                    ["password"] = FrmLogin.Password,
                    ["hwid"] = FrmLogin.Hwid
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
                        ExceptionHandler.SendException(ex);
                        Alert.Show("Webserver currently not Available! Try Again later.", FrmAlert.EnmType.Error);
                    }
                };
            }
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                Alert.Show("Webserver currently not Available! Try Again later.", FrmAlert.EnmType.Error);
            }
        }

        public static bool CheckAccess(byte[] response)
        {
            try
            {
                var responseString = Encoding.Default.GetString(response);

                FrmLogin.LicenceInformations = JObject.Parse(responseString);
                if (FrmLogin.LicenceInformations["message"]?.ToString() == "false")
                {
                    Alert.Show("Licence is not active. Please contact an Administrator.", FrmAlert.EnmType.Error);
                    return false;
                }

                if (FrmLogin.LicenceInformations["message"]?.ToString() == "wrong_login")
                {
                    Alert.Show("Username or Password not known. Please contact an Administrator.",
                        FrmAlert.EnmType.Error);
                    return false;
                }

                if (FrmLogin.LicenceInformations["message"]?.ToString() == "hwid")
                {
                    Alert.Show("Your HWID seems changed, please reset it or contact an Administrator.",
                        FrmAlert.EnmType.Error);
                    return false;
                }

                if (Application.OpenForms.OfType<ChaosBot>().Count() == 1)
                    Application.OpenForms.OfType<ChaosBot>().First().Close();

                ChaosBot form = new ChaosBot();

                if (FrmLogin.LicenceInformations["discorduser"]?.ToString() != "")
                {
                    form.Conf.discorduser = FrmLogin.LicenceInformations["discorduser"]?.ToString();
                    form.Conf.Save();
                }

                form.Show();
                Application.OpenForms.OfType<FrmLogin>().First().Hide();

                return true;
            }
            catch (WebException)
            {
                Alert.Show("Server is not reachable, please try again later.", FrmAlert.EnmType.Error);
                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                Alert.Show(ex.Message, FrmAlert.EnmType.Error);
                return false;
            }
        }
    }
}