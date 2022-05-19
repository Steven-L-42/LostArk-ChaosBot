using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Cyotek.Windows.Forms;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Ocl;
using Emgu.CV.Util;
using EpPathFinding.cs;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

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
            enemyMask = new Image<Bgr, byte>(this
                .maskPath); // make white what the important parts are, other parts should be black
            debugDetector._enemyTemplate = enemyTemplate;
            debugDetector._enemyMask = enemyMask;
            debugDetector.rectangleX = x;
            debugDetector.rectangleY = y;
            debugDetector.rectangleWidth = width * -1;
            debugDetector.rectangleHeight = height * -1;
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

        public int TrackbartresholdValue = 1;

        private double Distance(Point enemy, Point _myPosition)
        {
            return Math.Sqrt((Math.Pow(enemy.X - _myPosition.X, 2) + Math.Pow(enemy.Y - _myPosition.Y, 2)));
        }

        public int trackBarDiladeValue, trackBarErodeValue, trackBarTreshBinaryValue = 1;
        public double CannyFirstValue, CannySecondValue = 1;

        private Image<Rgb, byte> inRange(Image<Bgr, byte> master, int r, int g, int b, int t)
        {
            return master.Convert<Rgb, Byte>().InRange(new Rgb(r - t, g - t, b - t), new Rgb(r + t, g + t, b + t))
                .Convert<Rgb, byte>();
        }

        private void cap(byte[] buffer)
        {
            if (this.picturePath != "")
            {
                enemyTemplate =
                    new Image<Bgr, byte>(this.picturePath); // icon of the enemy
            }

            if (this.maskPath != "")
            {
                enemyMask =
                    new Image<Bgr, byte>(this
                        .maskPath); // make white what the important parts are, other parts should be black
            }

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
            int selectedIndex = 0;

            tabControl1.Invoke((MethodInvoker) (() => { selectedIndex = tabControl1.SelectedIndex; }));
            trackBarVariant.Invoke((MethodInvoker) (() => { TrackbartresholdValue = trackBarVariant.Value; }));
            trackBarDilade.Invoke((MethodInvoker) (() => { trackBarDiladeValue = trackBarDilade.Value; }));
            trackBarErode.Invoke((MethodInvoker) (() => { trackBarErodeValue = trackBarErode.Value; }));
            trackBarTreshBinary.Invoke(
                (MethodInvoker) (() => { trackBarTreshBinaryValue = trackBarTreshBinary.Value; }));
            try
            {
                var foundGathering = false;
                while (true)
                {
                    Thread.Sleep(threadSleep);
                    testform.Refresh();
                    var rawScreen = screenPrinter.CaptureScreen();
                    Image<Rgb, Byte> image2;
                    if (rawScreen.Height >= 1 && rawScreen.Width >= 1)
                    {
                        if (selectedIndex == 3)
                        {
                            enemyTemplate =
                                new Image<Bgr, byte>(
                                    @"G:\test\clone\PixelAimbot\Resources\wood.png"); // icon of the enemy

                            debugDetector._enemyTemplate = enemyTemplate;
                            debugDetector._enemyMask = enemyTemplate;
                            debugDetector._threshold = 0.77f;
                            double distance;
                            using (bitmapImage = new Bitmap(rawScreen))
                            {
                                rawScreen.Dispose();
                                Point? enemy;
                                using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                                {
                                    screenCapture.ROI = new Rectangle(x, y, width * -1, height * -1);

                                    enemy = debugDetector.GetClosestEnemy(screenCapture, false, testform);
                                    screenCapture.Draw(new CircleF(new Point(394 / 2 + 2, 340 / 2 - 2), 18),
                                        new Bgr(142, 157, 162), -1);
                                    var scaledImage = screenCapture.Convert<Rgb, Byte>();

                                    var t = 10;
                                    var color1 = inRange(screenCapture, 162, 157, 142, t);
                                    var color2 = inRange(screenCapture, 142, 137, 121, t);
                                    var color3 = color1; //inRange(screenCapture, 218, 197, 110, t);
                                    //   var color4 = inRange(screenCapture, 70,184,195,t);
                                    var colorMeIcon = color1;/*inRange(screenCapture, 88, 239, 107, t) +
                                                      inRange(screenCapture, 0, 0, 0, t);*/
                                    var colorMeIcon2 = inRange(screenCapture, 35, 56, 46, t);
                                    var colorMeCircle = inRange(screenCapture, 71, 205, 214, t)
                                                        + inRange(screenCapture, 102, 144, 143, t)
                                                        + inRange(screenCapture, 112, 130, 130, t)
                                                        + inRange(screenCapture, 122, 146, 143, t)
                                                        + inRange(screenCapture, 132, 152, 143, t)
                                                        + inRange(screenCapture, 142, 156, 142, t)
                                                        + inRange(screenCapture, 151, 156, 142, t)
                                                        + inRange(screenCapture, 139, 155, 142, t)
                                                        + inRange(screenCapture, 127, 153, 142, t)
                                                        + inRange(screenCapture, 109, 142, 145, t)
                                                        + inRange(screenCapture, 98, 139, 145, t)
                                                        + inRange(screenCapture, 82, 132, 147, t)
                                                        + inRange(screenCapture, 77, 163, 174, t)
                                                        + inRange(screenCapture, 79, 185, 195, t)
                                                        + inRange(screenCapture, 77, 164, 177, t)
                                                        + inRange(screenCapture, 83, 158, 165, t)
                                                        + inRange(screenCapture, 90, 138, 145, t)
                                                        + inRange(screenCapture, 103, 143, 144, t)
                                                        + inRange(screenCapture, 117, 139, 126, t)
                                                        + inRange(screenCapture, 109, 139, 128, t)
                                                        + inRange(screenCapture, 111, 140, 127, t)
                                                        + inRange(screenCapture, 103, 136, 130, t)
                                                        + inRange(screenCapture, 88, 131, 136, t)
                                                        + inRange(screenCapture, 92, 134, 133, t)
                                                        + inRange(screenCapture, 80, 135, 147, t)
                                                        + inRange(screenCapture, 84, 127, 138, t)
                                                        + inRange(screenCapture, 74, 183, 191, t)
                                                        + inRange(screenCapture, 76, 149, 162, t)
                                                        + inRange(screenCapture, 73, 156, 167, t)
                                                        + inRange(screenCapture, 73, 211, 214, t)
                                                        + inRange(screenCapture, 82, 126, 138, t)
                                                        + inRange(screenCapture, 77, 126, 140, t)
                                                        + inRange(screenCapture, 91, 130, 134, t)
                                                        + inRange(screenCapture, 87, 129, 136, t)
                                                        + inRange(screenCapture, 102, 135, 130, t)
                                                        + inRange(screenCapture, 96, 134, 132, t)
                                                        + inRange(screenCapture, 111, 135, 128, t)
                                                        + inRange(screenCapture, 106, 135, 130, t)
                                                        + inRange(screenCapture, 120, 137, 125, t)
                                                        + inRange(screenCapture, 116, 137, 127, t)
                                                        + inRange(screenCapture, 127, 137, 124, t)
                                                        + inRange(screenCapture, 124, 137, 124, t)
                                                        + inRange(screenCapture, 131, 137, 122, t)
                                                        + inRange(screenCapture, 129, 137, 123, t)
                                                        + inRange(screenCapture, 135, 136, 122, t)
                                                        + inRange(screenCapture, 134, 137, 122, t)
                                                        + inRange(screenCapture, 129, 155, 141, t)
                                                        + inRange(screenCapture, 155, 156, 141, t)
                                                        + inRange(screenCapture, 157, 156, 142, t)
                                                        + inRange(screenCapture, 164, 173, 120, t)
                                                        + inRange(screenCapture, 79, 163, 173, t)
                                                        + inRange(screenCapture, 79, 209, 211, t)
                                                        + inRange(screenCapture, 75, 188, 192, t)
                                                        + inRange(screenCapture, 85, 135, 146, t)
                                                        + inRange(screenCapture, 95, 138, 144, t)
                                                        + inRange(screenCapture, 133, 151, 143, t)
                                                        + inRange(screenCapture, 132, 150, 144, t)
                                                        + inRange(screenCapture, 123, 157, 155, t)
                                                        + inRange(screenCapture, 122, 164, 160, t)
                                                        + inRange(screenCapture, 122, 169, 164, t)
                                                        + inRange(screenCapture, 123, 176, 171, t)
                                                        + inRange(screenCapture, 121, 180, 174, t)
                                                        + inRange(screenCapture, 122, 176, 171, t)
                                                        + inRange(screenCapture, 122, 167, 163, t)
                                                        + inRange(screenCapture, 122, 163, 159, t)
                                                        + inRange(screenCapture, 125, 145, 144, t)
                                                        + inRange(screenCapture, 126, 146, 144, t)
                                                        + inRange(screenCapture, 131, 148, 143, t)
                                                        + inRange(screenCapture, 131, 149, 143, t)
                                                        + inRange(screenCapture, 122, 149, 143, t)
                                                        + inRange(screenCapture, 128, 152, 143, t)
                                                        + inRange(screenCapture, 132, 153, 143, t)
                                                        + inRange(screenCapture, 118, 148, 144, t)
                                                        + inRange(screenCapture, 124, 150, 143, t)
                                                        + inRange(screenCapture, 128, 151, 143, t)
                                                        + inRange(screenCapture, 113, 149, 146, t)
                                                        + inRange(screenCapture, 119, 149, 143, t)
                                                        + inRange(screenCapture, 123, 150, 143, t)
                                                        + inRange(screenCapture, 108, 168, 168, t)
                                                        + inRange(screenCapture, 114, 150, 146, t)
                                                        + inRange(screenCapture, 119, 148, 143, t)
                                                        + inRange(screenCapture, 105, 165, 166, t)
                                                        + inRange(screenCapture, 108, 174, 174, t)
                                                        + inRange(screenCapture, 111, 146, 149, t)
                                                        + inRange(screenCapture, 102, 133, 136, t)
                                                        + inRange(screenCapture, 103, 162, 163, t)
                                                        + inRange(screenCapture, 106, 176, 175, t)
                                                        + inRange(screenCapture, 106, 133, 132, t)
                                                        + inRange(screenCapture, 102, 132, 135, t)
                                                        + inRange(screenCapture, 103, 157, 158, t)
                                                        + inRange(screenCapture, 110, 134, 130, t)
                                                        + inRange(screenCapture, 105, 132, 132, t)
                                                        + inRange(screenCapture, 103, 130, 134, t)
                                                        + inRange(screenCapture, 113, 135, 129, t)
                                                        + inRange(screenCapture, 110, 133, 130, t)
                                                        + inRange(screenCapture, 106, 130, 132, t)
                                                        + inRange(screenCapture, 118, 136, 127, t)
                                                        + inRange(screenCapture, 115, 135, 128, t)
                                                        + inRange(screenCapture, 112, 133, 129, t)
                                        ;


                                    scaledImage = color1 + color2 + color3 + colorMeIcon + colorMeIcon2 + colorMeCircle;


                                    //    scaledImage._Dilate(trackBarDiladeValue); // 1
                                    //    scaledImage._Erode(trackBarErodeValue); // 1
                                    /// scaledImage._ThresholdBinary(new Gray(trackBarTreshBinaryValue), new Gray(255)); // 174
                                    //        image1 = image1.Canny(CannyFirstValue, CannySecondValue); 

                                    //   CvInvoke.BitwiseNot(image1, image1);
                                    Pixel pix = new Pixel();
                                    using (Bitmap imageBitmap = scaledImage.ToBitmap())
                                    {
                                        BaseGrid grid = pix.PNGtoGrid(imageBitmap);

                                        if (enemy != null)
                                        {
                                            List<GridPos> path = pix.findPath(
                                                grid,
                                                DiagonalMovement.OnlyWhenNoObstacles,
                                                new GridPos((scaledImage.Width / 2), (scaledImage.Height / 2)),
                                                new GridPos(enemy.Value.X + (enemyTemplate.Width / 2), enemy.Value.Y)
                                            );
                                            //    CvInvoke.BitwiseNot(image1, image1);


                                            scaledImage = pix.addPath(path, imageBitmap).ToImage<Rgb, Byte>();
                                            image2 = scaledImage;
                                            //     CvInvoke.BitwiseNot(image2, image2);

                                            var i = 0;


                                            if (path.Count - 1 >= 0 && path.Count - 1 < path.Count)
                                            {
                                                foreach (var item in path)
                                                {
                                                    CvInvoke.Circle(image2, new Point(item.x, item.y), 1,
                                                        new MCvScalar(0, 200, 200), 2);
                                                }
                                            }

                                            CvInvoke.Circle(image2,
                                                new Point(scaledImage.Width / 2, scaledImage.Height / 2), 1,
                                                new MCvScalar(200, 0, 0), 2);
                                            CvInvoke.Circle(image2,
                                                new Point(enemy.Value.X + (enemyTemplate.Width / 2),
                                                    enemy.Value.Y + 10), 1,
                                                new MCvScalar(0, 200, 0), 2);

                                            imageBoxMinimap.Image = image2;
                                            if (path.Count - 1 >= 2 && path.Count - 1 < path.Count)
                                            {
                                                int last_posx = 0;
                                                int last_posy = 0;

                                                distance = Distance(new Point(path[path.Count-1].x, path[path.Count-1].y),
                                                                  new Point((image2.Width / 2), (image2.Height / 2)));
                                                Debug.WriteLine(distance);
                                                var additionalPercent = 2;
                                                if (distance < 30)
                                                {
                                                    additionalPercent = 2;
                                                }
                                                var result = ChaosBot.MinimapToDesktop(path[2].x, path[2].y, additionalPercent);
                                                VirtualMouse.MoveTo((int) result.Item1, (int) result.Item2);
                                                last_posx = (int) result.Item1;
                                                last_posy = (int) result.Item2;
                                              //  VirtualMouse.LeftClick();
                                                 VirtualMouse.LeftDown();
                                                //Task.Delay(100).Wait();


                                                try
                                                {
                                                    var template =
                                                        new Image<Bgr, byte>(
                                                            @"G:\test\clone\PixelAimbot\Resources\g.png"); // icon of the enemy
                                                    var mask = new Image<Bgr, byte>(
                                                        @"G:\test\clone\PixelAimbot\Resources\g.png"); // icon of the enemy


                                                    var detector = new ScreenDetector(template, mask, 0.96f,
                                                        ChaosBot.Recalc(711),
                                                        ChaosBot.Recalc(119, false), ChaosBot.Recalc(1073),
                                                        ChaosBot.Recalc(956, false));
                                                    using (var sreenCapture =
                                                           new Bitmap(screenPrinter.CaptureScreen())
                                                               .ToImage<Bgr, byte>())
                                                    {
                                                        var item = detector.GetBest(sreenCapture, true);
                                                        if (item.HasValue)
                                                        {
                                                            // search for tree after walking path :);
                                                              KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                                                            //  foundGathering = true;
                                                                 VirtualMouse.LeftUp();
                                                            //     VirtualMouse.LeftUp();
                                                            //    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                                                            Task.Delay(15000).Wait();
                                                        }
                                                        else
                                                        {
                                                            // Not Found
                                                        }
                                                    }
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }

                                    //       distance = Distance(new Point(enemy.Value.X, enemy.Value.Y),
                                    //              new Point((image1.Width / 2), (image1.Height / 2)));
                                    //    Debug.WriteLine(distance);
                                }
                            }
                        }
                        else if (selectedIndex == 2)
                        {
                            object result = Pixel.PixelSearch(x, y, (width * -1), (height * -1),
                                screenColorPicker1.Color.ToArgb(), TrackbartresholdValue);
                            Debug.WriteLine(result);
                            if (result.ToString() != "0")
                            {
                                object[] resultCoord = (object[]) result;
                                screenDrawer.Draw(testform, 0, 0, (width * -1) - x, (height * -1) - y);
                                screenDrawer.Draw(testform, (int) resultCoord[0] - x, (int) resultCoord[1] - y, 5, 5,
                                    new Pen(Color.Red, 3));
                            }
                        }
                        else
                        {
                            using (bitmapImage = new Bitmap(rawScreen))
                            {
                                rawScreen.Dispose();
                                Point? enemy;
                                using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                                {
                                    enemy = null;
                                    screenDrawer.Draw(testform, 0, 0, (width * -1), (height * -1));
                                    if (selectedIndex == 1)
                                    {
                                        textBoxTextSearch.Invoke((MethodInvoker) (() =>
                                            textBoxTextSearch.Text = ChaosBot.ReadArea(screenCapture.ToBitmap(), x, y, width * -1,
                                                height * -1, "")));
                                    }
                                    else
                                    {
                                        if (radioButtonGetBest.Checked)
                                        {
                                            enemy = debugDetector.GetBestEnemy(screenCapture, !checkBoxShowAll.Checked,
                                                testform);
                                        }

                                        if (radioButtonGetClosest.Checked)
                                        {
                                            enemy = debugDetector.GetClosestEnemy(screenCapture,
                                                !checkBoxShowAll.Checked,
                                                testform);
                                        }

                                        if (radioButtonGetClosestBest.Checked)
                                        {
                                            enemy = debugDetector.GetClosestBest(screenCapture,
                                                !checkBoxShowAll.Checked,
                                                testform);
                                        }

                                        if (enemy.HasValue)
                                        {
                                            screenDrawer.Draw(testform, enemy.Value.X, enemy.Value.Y,
                                                enemyTemplate.Size.Width,
                                                enemyTemplate.Size.Height,
                                                new Pen(Color.Blue, 3));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.Message);
                Debug.WriteLine(ex.Message);
            }

            // throw new NotImplementedException();
        }

        private static VectorOfPoint ProcessImage(Image<Gray, byte> template, Image<Gray, byte> templateMask,
            Image<Gray, byte> sceneImage)
        {
            try
            {
                // initializations done
                VectorOfPoint finalPoints = null;
                Mat homography = null;
                VectorOfKeyPoint templateKeyPoints = new VectorOfKeyPoint();
                VectorOfKeyPoint sceneKeyPoints = new VectorOfKeyPoint();
                Mat tempalteDescriptor = new Mat();
                Mat sceneDescriptor = new Mat();

                Mat mask;
                int k = 2;
                double uniquenessthreshold = 0.80;
                VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();

                // feature detectino and description
                KAZE featureDetector = new KAZE();
                featureDetector.DetectAndCompute(template, templateMask, templateKeyPoints, tempalteDescriptor, false);
                featureDetector.DetectAndCompute(sceneImage, null, sceneKeyPoints, sceneDescriptor, false);


                // Matching

                // KdTreeIndexParams ip = new KdTreeIndexParams();
                //  var ip = new AutotunedIndexParams();
                var ip = new LinearIndexParams();
                SearchParams sp = new SearchParams();
                FlannBasedMatcher matcher = new FlannBasedMatcher(ip, sp);


                matcher.Add(tempalteDescriptor);
                matcher.KnnMatch(sceneDescriptor, matches, k);

                mask = new Mat(matches.Size, 1, Emgu.CV.CvEnum.DepthType.Cv8U, 1);
                mask.SetTo(new MCvScalar(255));

                Features2DToolbox.VoteForUniqueness(matches, uniquenessthreshold, mask);

                int count = Features2DToolbox.VoteForSizeAndOrientation(templateKeyPoints, sceneKeyPoints, matches,
                    mask, 1.5, 20);

                if (count >= 2)
                {
                    homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(templateKeyPoints,
                        sceneKeyPoints, matches, mask, 5);
                }

                if (homography != null)
                {
                    Rectangle rect = new Rectangle(Point.Empty, template.Size);
                    PointF[] pts = new PointF[]
                    {
                        new PointF(rect.Left, rect.Bottom),
                        new PointF(rect.Right, rect.Bottom),
                        new PointF(rect.Right, rect.Top),
                        new PointF(rect.Left, rect.Top)
                    };

                    pts = CvInvoke.PerspectiveTransform(pts, homography);
                    Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
                    finalPoints = new VectorOfPoint(points);
                }

                return finalPoints;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            textBoxX.Text = ChaosBot.Recalc(1593).ToString();
            textBoxY.Text = ChaosBot.Recalc(40).ToString();
            textBoxHeight.Text = ChaosBot.Recalc(255).ToString();
            textBoxWidth.Text = ChaosBot.Recalc(296).ToString();
        }

        private void buttonSelectPicture_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.picturePath = openFileDialog1.FileName;
                    pictureBoxPicture.Image = Image.FromFile(this.picturePath);
                    if (this.picturePath != "")
                    {
                        button2.Enabled = true;
                        buttonGenerateCode.Enabled = true;
                    }
                }
                catch
                {
                }
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
                    if (this.picturePath != "")
                    {
                        button2.Enabled = true;
                        buttonGenerateCode.Enabled = true;
                    }
                }
                catch
                {
                }
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
            form1.TopMost = true;
        }

        private void trackBarThreadSleep_ValueChanged(object sender, EventArgs e)
        {
            this.threadSleep = trackBarThreadSleep.Value;

            labelRefresh.Text = "Refresh (" + threadSleep + "ms)";
        }

        private void buttonGenerateCode_Click(object sender, EventArgs e)
        {
            string method = "";
            string mask = "";
            string maskBool = "null";
            if (radioButtonGetBest.Checked)
            {
                method = "var item = detector.GetBest(screenCapture, true);";
            }

            if (radioButtonGetClosest.Checked)
            {
                method = "var item = detector.GetClosestEnemy(screenCapture, true);";
            }

            if (radioButtonGetClosestBest.Checked)
            {
                method = "var item = detector.GetClosestBest(screenCapture, true);";
            }

            if (maskPath != "")
            {
                mask = "var mask = ChaosBot.byteArrayToImage(PixelAimbot.Images." + Path.GetFileName(maskPath) + ");";
                maskBool = "mask";
            }

            String text = @"try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                var template = ChaosBot.byteArrayToImage(PixelAimbot.Images." + Path.GetFileName(picturePath) + @");
                " + mask + @"


                var detector = new ScreenDetector(template, " + maskBool + @", " +
                          treshold.ToString().Replace(",", ".") +
                          @"f, ChaosBot.Recalc(" + x + @"), ChaosBot.Recalc(" + y + @", false), ChaosBot.Recalc(" +
                          width * -1 +
                          @"), ChaosBot.Recalc(" + height * -1 + @", false));
                var screenPrinter = new PrintScreen();
                using(var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>()) {

                    " + method + @"
                    if (item.HasValue)
                    {
                        // Found
                    }
                    else
                    {
                        // Not Found
                    }
                }

            }
            catch { }";
            Clipboard.SetText(text.Replace("'", "\""));
        }

        private void buttonRecalc_Click(object sender, EventArgs e)
        {
            textBoxX.Text = ChaosBot.Recalc(int.Parse(textBoxX.Text)).ToString();
            textBoxY.Text = ChaosBot.Recalc(int.Parse(textBoxY.Text), true).ToString();
            textBoxWidth.Text = ChaosBot.Recalc(int.Parse(textBoxWidth.Text)).ToString();
            textBoxHeight.Text = ChaosBot.Recalc(int.Parse(textBoxHeight.Text), true).ToString();
        }

        public int recalcToBotResolution(int value, bool horizontal = true)
        {
            decimal oldResolution;
            decimal newResolution;
            if (horizontal)
            {
                oldResolution = 1920;
                newResolution = Screen.PrimaryScreen.Bounds.Width;
            }
            else
            {
                oldResolution = 1080;
                newResolution = Screen.PrimaryScreen.Bounds.Height;
            }


            decimal normalized = (decimal) value * oldResolution;
            decimal rescaledPosition = (decimal) normalized / newResolution;

            int returnValue = Decimal.ToInt32(rescaledPosition);
            return returnValue;
        }

        private void buttonRecalcToBotresolution_Click(object sender, EventArgs e)
        {
            textBoxX.Text = recalcToBotResolution(int.Parse(textBoxX.Text)).ToString();
            textBoxY.Text = recalcToBotResolution(int.Parse(textBoxY.Text), true).ToString();
            textBoxWidth.Text = recalcToBotResolution(int.Parse(textBoxWidth.Text)).ToString();
            textBoxHeight.Text = recalcToBotResolution(int.Parse(textBoxHeight.Text), true).ToString();
        }

        private void comboBoxMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            TemplateMatchingType type;

            switch (comboBoxMethod.SelectedIndex)
            {
                case 0:
                default:
                    type = TemplateMatchingType.SqdiffNormed;
                    break;
                case 1:
                    type = TemplateMatchingType.Sqdiff;
                    break;
                case 2:
                    type = TemplateMatchingType.Ccoeff;
                    break;
                case 3:
                    type = TemplateMatchingType.CcoeffNormed;
                    break;
                case 4:
                    type = TemplateMatchingType.Ccorr;
                    break;
                case 5:
                    type = TemplateMatchingType.CcorrNormed;
                    break;
            }

            debugDetector.setMatchingMethod(type);
        }


        private void radioButtonGetText_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            //        colorDialog1.ShowDialog();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = true;
            }
        }

        private void screenColorPicker1_ColorChanged(object sender, EventArgs e)
        {
            pictureBox1.BackColor = screenColorPicker1.Color;
            labelColorARGB.Text = "#" + pictureBox1.BackColor.R.ToString("X2") +
                                  pictureBox1.BackColor.G.ToString("X2") + pictureBox1.BackColor.B.ToString("X2");
            ;
        }

        private void trackBarVariant_ValueChanged(object sender, EventArgs e)
        {
            labelVariantShade.Text = "Variant Shade (" + trackBarVariant.Value + ")";
            TrackbartresholdValue = trackBarVariant.Value;
        }


        private void Debugging_Shown(object sender, EventArgs e)
        {
            GetMousePosition();
            //SaveMousePosition();
        }

        private bool searchMouse = true;

        private async void GetMousePosition()
        {
            while (searchMouse)
            {
                if (Cursor.Current != null)
                {
                    Cursor = new Cursor(Cursor.Current.Handle);
                    lbYCoord.Text = Cursor.Position.Y.ToString();
                    lbXCoord.Text = Cursor.Position.X.ToString();
                }

                await Task.Delay(10);
            }
        }


        private int i = 0;

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (e.KeyCode == Keys.F10)
                {
                    i++;
                    searchMouse = false;
                    checkBox1.Checked = true;
                    string Position = Convert.ToString(lbXCoord.Text) + "\t" + Convert.ToString(lbYCoord.Text);

                    textBoxX.Text = lbXCoord.Text;
                    textBoxY.Text = lbYCoord.Text;
                    if (i == 2)
                    {
                        i = 0;
                        searchMouse = true;
                        textBoxX.Text = string.Empty;
                        textBoxY.Text = string.Empty;
                        GetMousePosition();
                    }
                }
            }
        }


        private void lbXCoord_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetText(lbXCoord.Text);
        }

        private void lbYCoord_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetText(lbYCoord.Text);
        }

        private void lbXCoord_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.IsBalloon = true;
            toolTip.Show("Double Click to Copy X Coord", lbXCoord);
            toolTip.SetToolTip(lbXCoord, "Double Click to Copy X Coord");

            lbXCoord.BackColor = Color.DarkGray;
        }

        private void lbYCoord_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.IsBalloon = true;
            toolTip.Show("Double Click to Copy Y Coord", lbYCoord);
            toolTip.SetToolTip(lbYCoord, "Double Click to Copy Y Coord");
            lbYCoord.BackColor = Color.DarkGray;
        }

        private void lbYCoord_MouseLeave(object sender, EventArgs e)
        {
            lbYCoord.BackColor = BackColor;
        }

        private void lbXCoord_MouseLeave(object sender, EventArgs e)
        {
            lbXCoord.BackColor = BackColor;
        }

        private void trackBarTreshBinary_ValueChanged(object sender, EventArgs e)
        {
            trackBarTreshBinaryValue = trackBarTreshBinary.Value;
        }

        private void trackBarDilade_ValueChanged(object sender, EventArgs e)
        {
            trackBarDiladeValue = trackBarDilade.Value;
        }

        private void trackBarErode_ValueChanged(object sender, EventArgs e)
        {
            trackBarErodeValue = trackBarErode.Value;
        }

        private void trackBarCannyFirst_ValueChanged(object sender, EventArgs e)
        {
            CannyFirstValue = trackBarCannyFirst.Value;
        }

        private void trackBarCannySecond_ValueChanged(object sender, EventArgs e)
        {
            CannySecondValue = trackBarCannySecond.Value;
        }
    }
}