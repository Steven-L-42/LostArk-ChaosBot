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

namespace PixelAimbot
{
    public partial class frmLogin : Form
    {

        private readonly string versionId = "1.2.8r";
        private string currentLauncherVersion = "";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                        int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static string hwid;

        private string currentFilename;

        private static readonly Random random = new Random();

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        static readonly IntPtr HWND_TOP = new IntPtr(0);

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        const UInt32 SWP_NOSIZE = 0x0001;

        const UInt32 SWP_NOMOVE = 0x0002;

        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );



        public frmLogin()
        {
            InitializeComponent();
            WebRequest.DefaultWebProxy = null;
            hwid = HWID.Get();

            currentFilename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            // Rename Application to a Custom Exe name for EAC Prevention / Security
            // Disable for Debug!
            /*
            if(currentFilename.Contains("Chaos-Bot"))
            {
                string newFilename = RandomString(15);
                System.IO.File.Move(currentFilename, newFilename + ".exe");
                Process.Start(newFilename + ".exe");
                Application.Exit();
            }*/
            CheckForUpdate();

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
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
            string user = tbUser.Text;
            string pass = tbPass.Text;

           // if (dr.Read())
           // {
                ChaosBot Form = new ChaosBot();
                Form.Show();
                this.Hide();
           // }
           // else
            //{
            //    MessageBox.Show("Invalid Login please check username and password");
           // }
          
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
            if (tbUser.Text !="")
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
                    btnLogin.Enabled = false;
                    progressBar1.Visible = true;
                    using (WebClient wc = new WebClient())
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Chaos-bot_" + currentLauncherVersion + ".exe"))
                        {
                            File.Delete(Directory.GetCurrentDirectory() + "\\Chaos-bot_" + currentLauncherVersion + ".exe");
                        }
                        wc.Proxy = null;
                        wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                        wc.DownloadFileCompleted += Wc_DownloadFileCompleted;

                        wc.DownloadFileAsync(new Uri("https://files.symbiotic.link/Chaos-bot.exe"), Directory.GetCurrentDirectory() + "\\Chaos-bot_" + currentLauncherVersion + ".exe");
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

            Process.Start("Chaos-bot_" + currentLauncherVersion + ".exe");
            System.IO.File.Move(currentFilename, currentFilename + ".bin");
            Application.Exit();

        }
    }
}





