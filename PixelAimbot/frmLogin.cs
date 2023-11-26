using Newtonsoft.Json.Linq;
using PixelAimbot.Classes.Misc;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PixelAimbot
{
    public partial class FrmLogin : Form
    {
        private readonly Config _config;
        private readonly string _versionId = Config.version;
        private string _currentLauncherVersion = "";

        private const int WmNclbuttondown = 0xA1;
        private const int HtCaption = 0x2;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd,
                        int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public static readonly BlowFish Blow1 = new BlowFish("1238150549789312");

        public static string Hwid;

        public static JObject LicenceInformations { get; set; }

        public static string Username;
        public static string Password;

        private readonly string _currentFilename;

        private static readonly Random Random = new Random();

        private readonly bool _isServerOnline = false;

        public FrmLogin()
        {
            
            InitializeComponent();
           btnClear.Enabled =
                chBoxRemember.Enabled = chBoxShowPassword.Enabled =
                tbUser.Enabled = tbPass.Enabled =
                    radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = false;
                this.Text = RandomString(15);
                DownloadResources();
                WebRequest.DefaultWebProxy = null;
                Hwid = HWID.Get();
                Config.Init();
                _config = Config.Load();

                if (Environment.GetCommandLineArgs().Length > 1)
                {
                    if (Environment.GetCommandLineArgs()[1] != null && Environment.GetCommandLineArgs()[2] != null)
                    {
                        tbUser.Text = Environment.GetCommandLineArgs()[1];
                        tbPass.Text = Environment.GetCommandLineArgs()[2];
                        Username = tbUser.Text;
                        Password = tbPass.Text;
                        _config.username = tbUser.Text;
                        _config.password = tbPass.Text;
                        _config.Save();

                        if (_isServerOnline)
                            Classes.Auth.Access.CheckAccessAsyncCall();
                        return;
                    }
                }

                var processModule = Process.GetCurrentProcess().MainModule;
                if (processModule != null)
                {
                    _currentFilename = processModule.FileName;
                }

                label15.Text = Config.version;
                // Rename Application to a Custom Exe name for EAC Prevention / Security
                // Disable for Debug!
                //if (!Debugger.IsAttached)
                //{
                //    if (_currentFilename.Contains("Chaos-Bot"))
                //    {
                //        string newFilename = RandomString(15);
                //        File.Move(_currentFilename, newFilename + ".exe");
                //        Process.Start(newFilename + ".exe");
                //        Environment.Exit(0);
                //    }
                //}

            if (_isServerOnline)
            {
                radioButton1.Checked = true;

                string versionCheck = "stable";
                if (_config.devversion)
                {
                    radioButton1.Checked = true;
                    versionCheck = "dev";
                }

                if (_config.oldversion)
                {
                    radioButton2.Checked = true;
                    versionCheck = "old";
                }

                if (!_config.oldversion && !_config.devversion)
                {
                    radioButton3.Checked = true;
                }
                // Check for a Update
                if (!Debugger.IsAttached)
                {
                    CheckForUpdate(versionCheck);
                }
                //     CheckForUpdate(config.devversion);
            }

            this.FormBorderStyle = FormBorderStyle.None;
        }

        public sealed override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        private static void DownloadResources()
        {
            try
            {
                if (!File.Exists(@"C:\windows\system32\cvextern.dll") || !File.Exists(@"C:\Windows\SysWOW64\cvextern.dll"))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile("https://about-steven.de/Download/cvextern.dll", @"C:\Windows\System32\cvextern.dll");
                        client.DownloadFile("https://about-steven.de/Download/cvextern.dll", @"C:\Windows\SysWOW64\cvextern.dll");
                    }
                }
           
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var applicationFolder = Path.Combine(folder, "cb_res");
                if (Directory.Exists(applicationFolder))
                {
                    Directory.Delete(applicationFolder, true);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        //Exit Button

        private void btnLogin_Click(object sender, EventArgs e)
        {
           
           

                if (Screen.PrimaryScreen.Bounds.Width != 1920)
                {
                    Alert.Show(@"Your Resolution " + Screen.PrimaryScreen.Bounds.Width + "x" + Screen.PrimaryScreen.Bounds.Height + " is Beta\r\nBest Resolution 1920x1080 Borderless", FrmAlert.EnmType.Warning);
                }

                if (chBoxRemember.Checked)
                {
                    _config.username = tbUser.Text;
                    _config.password = tbPass.Text;
                    _config.Save();
                }
                else
                {
                    _config.username = "";
                    _config.password = "";
                    _config.Save();
                }
                Username = tbUser.Text;
                Password = tbPass.Text;
            switch (_isServerOnline)
            {
                case false:
                {
                    if (Application.OpenForms.OfType<ChaosBot>().Count() == 1)
                        Application.OpenForms.OfType<ChaosBot>().First().Close();

                    var form = new ChaosBot();
                    form.Show();
                    Application.OpenForms.OfType<FrmLogin>().First().Hide();
                    break;
                }
                case true:
                    Classes.Auth.Access.CheckAccessAsyncCall();
                    break;
            }
        }

        private void lbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbUser.Text = tbPass.Text = "";
        }

        private void chBoxShowPassword_CheckedChanged(object sender)
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
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (_isServerOnline)
            {
                if (_config != null)
                {
                    tbUser.Text = _config.username;
                    tbPass.Text = _config.password;
                }

                chBoxRemember.Checked = tbUser.Text != "";
            }
            else
            {
                tbUser.Text = @"FreeForAll";
                tbPass.Text = @"DontDecryptThisShitPassword";
                radioButton1.Checked = chBoxRemember.Checked = true;
            }
        }

        private void CheckForUpdate(string version = "stable")
        {
            try
            {
                string versionLink;
                string launcherLink;
                switch (version)
                {
                    default:
                        launcherLink = "https://files.symbiotic.link/Chaos-Bot.exe";
                        versionLink = "https://files.symbiotic.link/version_new.php";
                        break;

                    case "dev":
                        launcherLink = "https://files.symbiotic.link/Chaos-Bot-Dev.exe";
                        versionLink = "https://files.symbiotic.link/version_dev.php";
                        break;

                    case "old":
                        launcherLink = "https://files.symbiotic.link/Chaos-Bot-Old.exe";
                        versionLink = "https://files.symbiotic.link/version_old.php";
                        break;
                }

                _currentLauncherVersion = new WebClient().DownloadString(versionLink);

                // Search old Shit
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.bin");

                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        //       ExceptionHandler.SendException(ex);
                    }
                }

                if (_currentLauncherVersion != _versionId)
                {
                    Alert.Show("New Version found. Updating...", FrmAlert.EnmType.Info);
                    btnLogin.Enabled = false;
                    progressBar1.Visible = true;
                    using (WebClient wc = new WebClient())
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + _currentLauncherVersion + ".exe"))
                        {
                            File.Delete(Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + _currentLauncherVersion + ".exe");
                        }
                        wc.Proxy = null;
                        wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += Wc_DownloadFileCompleted;

                        wc.DownloadFileAsync(new Uri(launcherLink),
                            Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + _currentLauncherVersion + ".exe");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                Alert.Show("Cannot check for Application Update. Please contact Developer", FrmAlert.EnmType.Error);
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
                Alert.Show("The download has been cancelled", FrmAlert.EnmType.Error);
                return;
            }

            if (e.Error != null) // We have an error! Retry a few times, then abort.
            {
                Alert.Show("An error occurred while trying to download file", FrmAlert.EnmType.Error);
                return;
            }

            Process.Start("Chaos-Bot_" + _currentLauncherVersion + ".exe");
            File.Move(_currentFilename, _currentFilename + ".bin");
            Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender)
        {
            if (!radioButton1.Checked || !_isServerOnline) return;
            _config.devversion = true;
            _config.oldversion = false;
            _config.Save();
            if (Debugger.IsAttached) return;
            CheckForUpdate("dev");
        }

        private void radioButton2_CheckedChanged(object sender)
        {
            if (!radioButton2.Checked || !_isServerOnline) return;
            _config.devversion = false;
            _config.oldversion = true;
            _config.Save();
            if (Debugger.IsAttached) return;
            CheckForUpdate("old");
        }

        private void radioButton3_CheckedChanged(object sender)
        {
            if (!radioButton3.Checked || !_isServerOnline) return;
            _config.devversion = false;
            _config.oldversion = false;
            _config.Save();
            if (Debugger.IsAttached) return;
            CheckForUpdate();
        }
        private void nightButtonPurchase_Click(object sender, EventArgs e)
        {
            if (_isServerOnline)
            {
                Clipboard.SetText("/purchase");
                Alert.Show("Command '/purchase' copied to Clipboard\n\n" +
                           "Paste this Command as the first Message\n" +
                           "to Symbiotic-Bot. ");
                Process.Start("https://discordapp.com/users/717411999859867789");
            }
            else
                Alert.Show("The bot remains free for life.",FrmAlert.EnmType.Info);
            
        }

        private void nightButtonChangeLogs_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.com/channels/954895591962464296/954899743757180979");
            Alert.Show("Redirected to Discord:\n Changelog-Page.",FrmAlert.EnmType.Info);
        }

        private void nightButtonRegister_Click(object sender, EventArgs e)
        {
            if (_isServerOnline)
                Process.Start("https://discord.com/channels/954895591962464296/955216217742917742");
            else
                Alert.Show("Account already registered.",FrmAlert.EnmType.Info);
            
        }


    }
}