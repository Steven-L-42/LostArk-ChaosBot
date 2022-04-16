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
        private int x = 0;
        private int y = 0;
        private int width = 0;
        private int height = 0;
        private bool isLive = false;
        private Thread th;
        public Debugging()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(x, y, width, height, bitmap.Size);
            pictureBox1.Image = bitmap;
        }

        private void Debugging_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (!isLive)
            {
                byte[] buffer = { };
                th = new Thread(() => cap(buffer));
                th.Start();
                button2.Text = "Recording...";
                isLive = true;
            } else
            {
                button2.Text = "Live";
                isLive = false;
                th.Abort();
            }

        }

        private void cap(byte[] buffer)
        {
            // throw new NotImplementedException();
            Bitmap bitmap = new Bitmap(screenWidth, screenHeight);
            var _screenDrawer = new DrawScreen();
            while (true)
            {
                try
                {
                    _screenDrawer.Draw(x, y, width * -1, height * -1);

                    Graphics g = Graphics.FromImage(bitmap);
                    g.CopyFromScreen(x + width, y + height, width, height, bitmap.Size);
                    pictureBox1.Image = bitmap;
                    Thread.Sleep(10);
                } catch { }
            }
        }

        private void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            if (textBoxHeight.Text != "")
            {
                height = int.Parse(textBoxHeight.Text) * -1;
                pictureBox1.Height = height * -1;
            }
        }

        private void textBoxWidth_TextChanged(object sender, EventArgs e)
        {

            if (textBoxWidth.Text != "")
            {
                width = int.Parse(textBoxWidth.Text) * -1;
                pictureBox1.Width = width * -1;
            }
        }

        private void textBoxY_TextChanged(object sender, EventArgs e)
        {
            if (textBoxY.Text != "")
            {
                y = int.Parse(textBoxY.Text);
            }

        }

        private void textBoxX_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX.Text != "")
            {
                x = int.Parse(textBoxX.Text);
            }
            

        }
    }
}
