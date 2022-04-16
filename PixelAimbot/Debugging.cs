using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelAimbot
{
    public partial class Debugging : Form
    {
        private int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        private int screenHeight = Screen.PrimaryScreen.Bounds.Height; 
        public Debugging()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(1593, 40, 296, 255, bitmap.Size);
            pictureBox1.Image = bitmap;
        }

        private void Debugging_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] buffer = {};
            Thread th = new Thread(() => cap(buffer));
            th.Start();

        }

        private void cap(byte[] buffer)
        {


            // throw new NotImplementedException();
            Bitmap bitmap = new Bitmap(screenWidth, screenHeight);
            while (true)
            {
                try
                {
                    
                    Graphics g = Graphics.FromImage(bitmap);
                    g.CopyFromScreen(1500, 0, 0, 0, bitmap.Size);
                    pictureBox1.Image = bitmap;
                    Thread.Sleep(10);
                } catch { }
            }
        }
    }
}
