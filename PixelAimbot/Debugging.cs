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
        private DrawScreenWin screenDrawer = new DrawScreenWin();
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
        private int threadSleep = 100;
        Bitmap bitmapImage;
        private DebugDetector debugDetector = new DebugDetector(null, null, 0.7f, 0, 0, 0, 0);
        Image<Bgr, byte> enemyTemplate = null;
        Image<Bgr, byte> enemyMask = null;

        public Debugging()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
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
            enemyTemplate = new Image<Bgr, byte>(this.picturePath); // icon of the enemy
            enemyMask = new Image<Bgr, byte>(this.maskPath); // make white what the important parts are, other parts should be black
            debugDetector._enemyTemplate = enemyTemplate;
            debugDetector._enemyMask = enemyMask;
            debugDetector.rectangleX = x;
            debugDetector.rectangleY = y;
            debugDetector.rectangleWidth = width * -1;
            debugDetector.rectangleHeight = height * -1;

 
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
            enemyTemplate =
              new Image<Bgr, byte>(this.picturePath); // icon of the enemy
            enemyMask =
                new Image<Bgr, byte>(this.maskPath); // make white what the important parts are, other parts should be black
            debugDetector._enemyTemplate = enemyTemplate;
            debugDetector._enemyMask = enemyMask;
            debugDetector.rectangleX = x;
            debugDetector.rectangleY = y;
            debugDetector.rectangleWidth = width * -1;
            debugDetector.rectangleHeight = height * -1;
            Form testform = new Form();

            testform.Size = new Size(width * -1, height * -1);
            testform.StartPosition = FormStartPosition.Manual;
            testform.Location = new Point(x, y);
            testform.BackColor = Color.White;
            testform.TopMost = true;
            testform.FormBorderStyle = FormBorderStyle.None;
            testform.TransparencyKey = Color.White;
            testform.Show();
            Application.EnableVisualStyles();
            try
            {
                while (true)
                {
                    Thread.Sleep(threadSleep);
                    testform.Refresh();
                    var rawScreen = screenPrinter.CaptureScreen();
                    if (rawScreen.Height >= 1 && rawScreen.Width >= 1)
                    {
                        using (bitmapImage = new Bitmap(rawScreen))
                        {
                            rawScreen.Dispose();
                            var screenCapture = bitmapImage.ToImage<Bgr, byte>();
                            Point? enemy = null;
                            screenDrawer.Draw(testform, 0, 0, (width * -1), (height * -1));
                            if (radioButtonGetBest.Checked)
                            {
                               enemy = debugDetector.GetBestEnemy(screenCapture, !checkBoxShowAll.Checked, testform);
                            }
                            if(radioButtonGetClosest.Checked)
                            {
                                enemy = debugDetector.GetClosestEnemy(screenCapture, !checkBoxShowAll.Checked, testform);
                            }
                            if(radioButtonGetClosestBest.Checked)
                            {
                                enemy = debugDetector.GetClosestBest(screenCapture, !checkBoxShowAll.Checked, testform);
                            }

                            if (enemy.HasValue)
                            {
                                screenDrawer.Draw(testform, enemy.Value.X - 15, enemy.Value.Y - 5, FishBot.recalc(enemyTemplate.Size.Width), FishBot.recalc(enemyTemplate.Size.Height, false), new Pen(Color.Blue, 3));
                            }
                            
                        }
                    }
                }
            } catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message);
            } 
            // throw new NotImplementedException();

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
            textBoxX.Text = FishBot.recalc(1593).ToString();
            textBoxY.Text = FishBot.recalc(40).ToString();
            textBoxHeight.Text = FishBot.recalc(255).ToString();
            textBoxWidth.Text = FishBot.recalc(296).ToString();
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

        private void trackBarThreadSleep_ValueChanged(object sender, EventArgs e)
        {
            
            this.threadSleep = trackBarThreadSleep.Value;
            
            labelRefresh.Text = "Refresh (" + threadSleep + "ms)";
        }
    }
}