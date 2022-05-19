using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using PixelAimbot.Classes.Misc;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PixelAimbot
{
    public partial class frmLogin : Form
    {
        private Config config = new Config();
        private readonly string versionId = Config.version;
        private string currentLauncherVersion = "";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                        int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static BlowFish blow1 = new BlowFish("1238150549789312");

        public static string hwid;

        public static JObject licenceInformations;

        public static JObject LicenceInformations { get => licenceInformations; set => licenceInformations = value; }
        public static string username;
        public static string password;
        


        private string currentFilename;

        private static readonly Random random = new Random();

        public frmLogin()
        {
            InitializeComponent();
            this.Text = RandomString(15);
            downloadResources();
            WebRequest.DefaultWebProxy = null;
            hwid = HWID.Get();
            Config.init();
            config = Config.Load();
            currentFilename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            label15.Text = Config.version;
            // Rename Application to a Custom Exe name for EAC Prevention / Security
            // Disable for Debug!
            if (!Debugger.IsAttached) {
                if (currentFilename.Contains("Chaos-Bot"))
                {
                    string newFilename = RandomString(15);
                    System.IO.File.Move(currentFilename, newFilename + ".exe");
                    Process.Start(newFilename + ".exe");
                    Environment.Exit(0);
                    Application.Exit();
                }
            }

            checkBoxOldVersion.Checked = config.oldversion;
            //Check for a Update
            CheckForUpdate(config.oldversion);


            this.FormBorderStyle = FormBorderStyle.None;
        }

        public static void downloadResources()
        {
        

            if (!File.Exists("cvextern.dll"))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://files.symbiotic.link/resources/cvextern.dll", "cvextern.dll");
                }
            }

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationFolder = Path.Combine(folder, "cb_res");
            if (Directory.Exists(applicationFolder))
            {
                Directory.Delete(applicationFolder, true);
            }
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        //Exit Button

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (Screen.PrimaryScreen.Bounds.Width != 1920)
            {
                Alert.Show(@"Your Resolution " + Screen.PrimaryScreen.Bounds.Width + "x" + Screen.PrimaryScreen.Bounds.Height + " is Beta\r\nBest Resolution 1920x1080 Borderless", frmAlert.enmType.Warning);
            }

            if (chBoxRemember.Checked == true)
            {
                
                config.username = tbUser.Text;
                config.password = tbPass.Text;
                config.Save();
            }
            else
            {
                config.username = "";
                config.password = "";
                config.Save();
            }
            username = tbUser.Text;
            password = tbPass.Text;

            PixelAimbot.Classes.Auth.Access.CheckAccessAsyncCall();
          
        }

        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbUser.Text = tbPass.Text = "";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (tbPass.PasswordChar == (char)0)
            {
                tbPass.PasswordChar = '*';
            }
            else
            {
                tbPass.PasswordChar = (char)0;
            }
        }

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("/purchase");
            MessageBox.Show("Command '/purchase' copied to Clipboard\n\n" +
                            "Paste this Command as the first Message\n" +
                            "to Symbiotic-Bot. ");
            System.Diagnostics.Process.Start("https://discordapp.com/users/717411999859867789");
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (config != null)
            {
                tbUser.Text = config.username;
                tbPass.Text = config.password;
            }
            if (tbUser.Text !="")
            {
                chBoxRemember.Checked = true;
            }
            else
            {
                chBoxRemember.Checked = false;
            }

        }
        

        public void CheckForUpdate(bool old_version = false)
        {

            try
            {
                if (old_version)
                {
                    currentLauncherVersion = new WebClient().DownloadString("https://files.symbiotic.link/version_old.php");
                }
                else
                {
                    currentLauncherVersion = new WebClient().DownloadString("https://files.symbiotic.link/version_new.php");
                }

                // Search old Shit
                string[] files = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory(), "*.bin");

                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.SendException(ex);
                    }
                }

                if (currentLauncherVersion != versionId)
                {
                    Alert.Show("New Version found. Updating...", frmAlert.enmType.Info);
                    btnLogin.Enabled = false;
                    progressBar1.Visible = true;
                    using (WebClient wc = new WebClient())
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + currentLauncherVersion + ".exe"))
                        {
                            File.Delete(Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + currentLauncherVersion + ".exe");
                        }
                        wc.Proxy = null;
                        wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                        if (old_version)
                        {
                            wc.DownloadFileAsync(new Uri("https://files.symbiotic.link/Chaos-Bot-Old.exe"),
                                Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + currentLauncherVersion + ".exe");
                        }
                        else
                        {
                            wc.DownloadFileAsync(new Uri("https://files.symbiotic.link/Chaos-Bot.exe"),
                                Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + currentLauncherVersion + ".exe");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                Alert.Show("Cannot check for Application Update. Please contact Developer", frmAlert.enmType.Error);
            }
        }
        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            progressBar1.Value = 0;

            if (e.Cancelled)
            {
                Alert.Show("The download has been cancelled", frmAlert.enmType.Error);
                return;
            }

            if (e.Error != null) // We have an error! Retry a few times, then abort.
            {
                Alert.Show("An error ocurred while trying to download file", frmAlert.enmType.Error);
                return;
            }

            Process.Start("Chaos-Bot_" + currentLauncherVersion + ".exe");
            System.IO.File.Move(currentFilename, currentFilename + ".bin");
            Application.Exit();

        }
        

        private void checkBoxEarlyAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOldVersion.Checked)
            {
                
                config.oldversion = true;
                config.Save();
            }
            else
            {
                
                config.oldversion = false;
                config.Save();
            }
            CheckForUpdate(config.oldversion);

        }

       
    }
}





