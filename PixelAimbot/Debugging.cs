using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;
using System;
using System.Drawing;
using System.Threading;
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
        private string picturePath = "";
        private string maskPath = "";
        private Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        private DebugDetector debugDetector = new DebugDetector(null, null, 0.7f, 0, 0, 0, 0);

        public Debugging()
        {
            InitializeComponent();
            _Debugging = this;
            this.DoubleBuffered = true;
        }

        public static Debugging _Debugging;

        public void updateArea(int x, int y, int width, int height)
        {
            textBoxX.Text = x.ToString();
            textBoxY.Text = y.ToString();
            textBoxWidth.Text = width.ToString();
            textBoxHeight.Text = height.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var enemyTemplate =
       new Image<Bgr, byte>(this.picturePath); // icon of the enemy
            var enemyMask =
               new Image<Bgr, byte>(this.maskPath); // make white what the important parts are, other parts should be black
            debugDetector._enemyTemplate = enemyTemplate;
            debugDetector._enemyMask = enemyMask;
            debugDetector.rectangleX = x;
            debugDetector.rectangleY = y;
            debugDetector.rectangleWidth = width * -1;
            debugDetector.rectangleHeight = height * -1;
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(x, y, width, height, bitmap.Size);
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
            debugDetector._enemyTemplate = enemyTemplate;
            debugDetector._enemyMask = enemyMask;
            debugDetector.rectangleX = x;
            debugDetector.rectangleY = y;
            debugDetector.rectangleWidth = width * -1;
            debugDetector.rectangleHeight = height * -1;

            // throw new NotImplementedException();

            while (true)
            {
                try
                {
                    var rawScreen = screenPrinter.CaptureScreen();
                    Bitmap bitmapImage = new Bitmap(rawScreen);
                    var screenCapture = bitmapImage.ToImage<Bgr, byte>();

                    //   CvInvoke.Rectangle(bitmap.ToImage<Bgr, byte>(), new Rectangle(new Point(x, y), new Size(width, height)), new MCvScalar(255));
                    screenDrawer.Draw(x, y, width * -1, height * -1);
                    var enemy = debugDetector.GetClosestEnemy(screenCapture, true);
                    if (enemy.HasValue)
                    {
                        screenDrawer.Draw(enemy.Value.X + x, enemy.Value.Y + y, enemyTemplate.Size.Width, enemyTemplate.Size.Height, new Pen(Color.Blue, 3));
                        /*CvInvoke.Rectangle(bitmap.ToImage<Bgr, byte>(),
                            new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                            new MCvScalar(255));*/
                    }
                    Thread.Sleep(1);
                }
                catch { }
            }
        }

        private void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            if (textBoxHeight.Text != "")
            {
                height = int.Parse(textBoxHeight.Text) * -1;
            }
        }

        private void textBoxWidth_TextChanged(object sender, EventArgs e)
        {
            if (textBoxWidth.Text != "")
            {
                width = int.Parse(textBoxWidth.Text) * -1;
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
            debugDetector._threshold = tresh;
            labelTreshold.Text = "Treshold (" + tresh + ")";
        }

        private void buttonSelectArea_Click(object sender, EventArgs e)
        {
            this.Hide();
            SelectArea form1 = new SelectArea();
            form1.InstanceRef = this;
            form1.Show();
        }
    }
}