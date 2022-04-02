using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
              //      ["username"] = PixelAimbot.frmLogin.tbUser.Text,
               //     ["password"] = PixelAimbot.frmLogin.tbPass.Text,
                    ["hwid"] = PixelAimbot.frmLogin.hwid
                };
                webClient.UploadValuesAsync(new Uri("https://admin.symbiotic.link/api/checkUser"), values);
                webClient.UploadValuesCompleted += (s, e) =>
                {
                    try
                    {
                        //CheckAccess(e.Result);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Webserver currently not Available! Try Again later.");
                    }
                };
            }
            catch (Exception)
            {
                MessageBox.Show("Webserver currently not Available! Try Again later.");
                //MessageBox.Show(ex.Message);
            }
        }
        /*
        public static bool CheckAccess(byte[] response)
        {
            try
            {

                var values = new NameValueCollection();
                values["username"] = Gui.MainForm.textBoxUsername.Text;
                values["password"] = Gui.MainForm.textBoxPassword.Text;
                values["hwid"] = Misc.HWID.Get();
                Misc.Config config = new Misc.Config();
                config.username = Gui.MainForm.blow1.Encrypt_CTR(Gui.MainForm.textBoxUsername.Text);
                config.password = Gui.MainForm.blow1.Encrypt_CTR(Gui.MainForm.textBoxPassword.Text);
                config.hwid = Gui.MainForm.blow1.Encrypt_CTR(Gui.MainForm.hwid);


                if (Gui.MainForm.checkBoxAutoLogin.Checked)
                {
                    config.autologin = true;
                }
                config.Save();
                Gui.MainForm.RegistryHandler();

                var responseString = Encoding.Default.GetString(response);
                Gui.MainForm.LicenceInformations = JObject.Parse(responseString);
                if (Gui.MainForm.LicenceInformations["message"].ToString() == "false")
                {
                    MessageBox.Show("Licence is not active. Please contact an Administrator.");
                    return false;
                }
                if (Gui.MainForm.LicenceInformations["message"].ToString() == "hwid")
                {
                    MessageBox.Show("Your HWID seems changed, please reset it or contact an Administrator.");
                    return false;
                }
                Gui.MainForm.LicenceInformations["username"] = Gui.MainForm.textBoxUsername.Text;
                Gui.MainForm.LicenceInformations["password"] = Gui.MainForm.textBoxPassword.Text;

                if (Application.OpenForms.OfType<Gui.GamesForm>().Count() == 1)
                    Application.OpenForms.OfType<Gui.GamesForm>().First().Close();

                Gui.MainForm.GamesForm = new Gui.GamesForm
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = Application.OpenForms[0].Location
                };
                Gui.MainForm.GamesForm.Show();
                Application.OpenForms[0].Hide();
                return true;
            }
            catch (WebException)
            {
                MessageBox.Show("Server is not reachable, please try again later.");
                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("Please control username and password");
                return false;
            }
        }
        */
    }
}
