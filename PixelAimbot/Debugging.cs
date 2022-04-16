using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.OpenCV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        private DrawScreen screenDrawer = new DrawScreen();
        private PrintScreen screenPrinter = new PrintScreen();

        private int x = 0;
        private int y = 0;
        private int width = 0;
        private int height = 0;
        private float treshold = 0.7f;
        private bool isLive = false;
        private Thread th;
        private string picturePath;
        private string maskPath;
        private Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

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
            }
            else
            {
                button2.Text = "Live";
                isLive = false;
                th.Abort();
            }

        }

        private void cap(byte[] buffer)
        {

            var enemyTemplate =
              new Image<Bgr, byte>(this.picturePath); // icon of the enemy
            var enemyMask =
                new Image<Bgr, byte>(this.maskPath); // make white what the important parts are, other parts should be black






            // throw new NotImplementedException();
            
            while (true)
            {
                try
                {
                    Graphics g = Graphics.FromImage(bitmap);
                    g.CopyFromScreen(x + width, y + height, width, height, bitmap.Size);
                    var enemyDetector = new DebugDetector(enemyTemplate, enemyMask, this.treshold, 0, 0, width * -1, height * -1);

                    var enemy = enemyDetector.GetClosestEnemy(bitmap.ToImage<Bgr, byte>(), true);
                    if (enemy.HasValue)
                    {
                        CvInvoke.Rectangle(bitmap.ToImage<Bgr, byte>(),
                            new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                            new MCvScalar(255));
                    }
                    pictureBox1.Image = bitmap;



                    Thread.Sleep(100);
                }
                catch { }
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

        private void btnGetMinimap_Click(object sender, EventArgs e)
        {
            textBoxX.Text = ChaosBot.recalc(1593).ToString();
            textBoxY.Text = ChaosBot.recalc(40).ToString();
            textBoxHeight.Text = ChaosBot.recalc(255).ToString();
            textBoxWidth.Text = ChaosBot.recalc(296).ToString();
        }

        private void buttonSelectPicture_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.picturePath = openFileDialog1.FileName;
                    pictureBoxPicture.Image = Image.FromFile(this.picturePath);
                    if (this.picturePath != "" && this.maskPath != "")
                    {
                        button2.Enabled = true;
                    }
                }
                catch { }
            }
        }

        private void buttonSelectMask_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.maskPath = openFileDialog1.FileName;
                    pictureBoxMask.Image = Image.FromFile(this.maskPath);
                    if (this.picturePath != "" && this.maskPath != "")
                    {
                        button2.Enabled = true;
                    }

                }
                catch { }
            }
        }

        private void trackBarTreshold_Changed(object sender, EventArgs e)
        {

            float tresh = trackBarTreshold.Value * 0.01f;
            this.treshold = tresh;
            labelTreshold.Text = "Treshold (" + tresh + ")";


        }
    }
}
