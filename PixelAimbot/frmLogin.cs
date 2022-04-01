using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using AutoItX3Lib;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Diagnostics;


namespace PixelAimbot
{
    public partial class frmLogin : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                        int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


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

        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader dr;

        public frmLogin()
        {
            InitializeComponent();
            con = new MySqlConnection("Server=sql4.freemysqlhosting.net;Database=sql4481891;user=sql4481891;Pwd=iKPWHYwrz5;SslMode=none");
            //Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;

            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
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
            cmd = new MySqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM users where username='" + tbUser.Text + "' AND password='" + tbPass.Text + "'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                ChaosBot Form = new ChaosBot();
                Form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Login please check username and password");
            }
            con.Close();
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
    }
}





