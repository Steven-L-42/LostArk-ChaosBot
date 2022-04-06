using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoItX3Lib;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using PixelAimbot.Classes.Misc;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PixelAimbot
{
    public partial class frmLogin : Form
    {

        private readonly string versionId = Properties.Settings.Default.version;
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
        public static string[] picturesDownload =
        {
                "boss.png",
                "bossmask.png",
                "enemy.png",
                "mask.png",
                "portalenter1.png",
                "portalentermask1.png",
                "revive.png"
        };

        private string currentFilename;

        private static readonly Random random = new Random();

        public frmLogin()
        {
            InitializeComponent();
            this.Text = RandomString(15);
            downloadResources();
            WebRequest.DefaultWebProxy = null;
            hwid = HWID.Get();

            currentFilename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            label15.Text = Properties.Settings.Default.version;
            // Rename Application to a Custom Exe name for EAC Prevention / Security
            // Disable for Debug!
            if (!Debugger.IsAttached)
            {
                if (currentFilename.Contains("Chaos-Bot"))
                {
                    string newFilename = RandomString(15);
                    System.IO.File.Move(currentFilename, newFilename + ".exe");
                    Process.Start(newFilename + ".exe");
                    Environment.Exit(0);
                    Application.Exit();
                }
            }
            // Try Generate Configuration
            /*
            try
            {
                Classes.Misc.ConfigurationHandler.init();
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot create Configuration file");
            }
            */
            //Check for a Update
            CheckForUpdate();


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

            foreach (string picture in picturesDownload)
            {
                // The folder for the roaming current user 
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                // Combine the base folder with your specific folder....
                string applicationFolder = Path.Combine(folder, "cb_res");

                // CreateDirectory will check if every folder in path exists and, if not, create them.
                // If all folders exist then CreateDirectory will do nothing.


                if (!Directory.Exists(applicationFolder))
                {
                    Directory.CreateDirectory(applicationFolder);
                }

                if (!File.Exists(applicationFolder + "\\" + picture))
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile("https://files.symbiotic.link/resources/" + picture, applicationFolder + "\\" + picture);
                    }
                }
            }

        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        //Exit Button
        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (chBoxRemember.Checked == true)
            {
                Properties.Settings.Default.username = tbUser.Text;
                Properties.Settings.Default.password = tbPass.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.username = "";
                Properties.Settings.Default.password = "";
                Properties.Settings.Default.Save();
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
            System.Diagnostics.Process.Start("https://discord.gg/XDYkVKXxCS");
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            tbUser.Text = Properties.Settings.Default.username;
            tbPass.Text = Properties.Settings.Default.password;
            if (tbUser.Text != "")
            {
                chBoxRemember.Checked = true;
            }
            else
            {
                chBoxRemember.Checked = false;
            }

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        public void CheckForUpdate()
        {

            try
            {
                currentLauncherVersion = new WebClient().DownloadString("https://files.symbiotic.link/version.php");
                // Search old Shit
                string[] files = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory(), "*.bin");

                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception)
                    {

                    }
                }

                if (currentLauncherVersion != versionId)
                {
                    MessageBox.Show("New Version found. Process is Updating now. Please press Ok");
                    btnLogin.Enabled = false;
                    progressBar1.Visible = true;
                    foreach (string picture in picturesDownload)
                    {
                        string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        string applicationFolder = Path.Combine(folder, "cb_res");

                        if (!Directory.Exists(applicationFolder))
                        {
                            Directory.CreateDirectory(applicationFolder);
                        }
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile("https://files.symbiotic.link/resources/" + picture, applicationFolder + "\\" + picture);
                        }

                    }
                    using (WebClient wc = new WebClient())
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + currentLauncherVersion + ".exe"))
                        {
                            File.Delete(Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + currentLauncherVersion + ".exe");
                        }
                        wc.Proxy = null;
                        wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += Wc_DownloadFileCompleted;

                        wc.DownloadFileAsync(new Uri("https://files.symbiotic.link/Chaos-Bot.exe"), Directory.GetCurrentDirectory() + "\\Chaos-Bot_" + currentLauncherVersion + ".exe");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot check for Application Update. Please contact Developer");
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
                MessageBox.Show("The download has been cancelled");
                return;
            }

            if (e.Error != null) // We have an error! Retry a few times, then abort.
            {
                MessageBox.Show("An error ocurred while trying to download file");
                return;
            }

            Process.Start("Chaos-Bot_" + currentLauncherVersion + ".exe");
            System.IO.File.Move(currentFilename, currentFilename + ".bin");
            Application.Exit();

        }
    }
}