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
            try
            {
                var foundGathering = false;
                while (true)
                {
                    Thread.Sleep(threadSleep);
                    testform.Refresh();
                    var rawScreen = screenPrinter.CaptureScreen();
                    if (rawScreen.Height >= 1 && rawScreen.Width >= 1)
                    {
                        if (selectedIndex == 3)
                        {
                            enemyTemplate =
                                new Image<Bgr, byte>(
                                    @"G:\test\clone\PixelAimbot\Resources\wood_new2.png"); // icon of the enemy

                            debugDetector._enemyTemplate = enemyTemplate;
                            debugDetector._enemyMask = enemyTemplate;
                            debugDetector._threshold = 0.80f;
                            double distance;
                            using (bitmapImage = new Bitmap(rawScreen))
                            {
                                rawScreen.Dispose();
                                Point? enemy;

                                using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                                {
                                    enemy = null;
                                    screenCapture.ROI = new Rectangle(x, y, width * -1, height * -1);
                                    enemy = debugDetector.GetClosestEnemy(screenCapture, false, testform);

                                    var scaledImage = screenCapture.Copy();
                                    Image<Gray, Byte> image1 = scaledImage.Convert<Gray, Byte>();

                                    image1._Dilate(2);
                                    image1._Erode(2);
                                    image1._ThresholdBinary(new Gray(100), new Gray(255));
                                    //image1._ThresholdBinary(127, 255);
                                    Image<Bgr, Byte> image2 = image1.Convert<Bgr, Byte>();

                                    Pixel pix = new Pixel();
                                    Bitmap imageBitmap = image2.ToBitmap();

                                    BaseGrid grid = pix.PNGtoGrid(imageBitmap);
                                    List<GridPos> path = pix.findPath(
                                        grid,
                                        DiagonalMovement.OnlyWhenNoObstacles,
                                        new GridPos((image2.Width / 2), (image2.Height / 2)),
                                        new GridPos(enemy.Value.X + (enemyTemplate.Width / 2), enemy.Value.Y + 10)
                                    );
                                    /*Show on Image */
                                    image2 = pix.addPath(path, imageBitmap).ToImage<Bgr, Byte>();

                                    /*Show on Image */
                                    var i = 0;
                                    if (path.Count - 1 >= 0 && path.Count - 1 < path.Count)
                                    {
                                        foreach (var item in path)
                                        {
                                            CvInvoke.Circle(image2, new Point(item.x, item.y), 1,
                                                new MCvScalar(0, 200, 200), 2);
                                        }
                                    }

                                    CvInvoke.Circle(image2, new Point(image2.Width / 2, image2.Height / 2), 1,
                                        new MCvScalar(200, 0, 0), 2);
                                    CvInvoke.Circle(image2, new Point(enemy.Value.X + (enemyTemplate.Width / 2), enemy.Value.Y + 10), 1,
                                        new MCvScalar(0, 200, 0), 2);


                                    if (path.Count - 1 >= 2 && path.Count - 1 < path.Count)
                                    {
                                        int last_posx = 0;
                                        int last_posy = 0;

                                     
                                            imageBoxMinimap.Image = image2;
                                            
                                            var result = ChaosBot.MinimapToDesktop(path[2].x, path[2].y);
                                            VirtualMouse.MoveTo((int) result.Item1, (int) result.Item2);
                                            last_posx = (int)result.Item1;
                                            last_posy = (int)result.Item2;
                  //                          VirtualMouse.LeftClick();
                                           // VirtualMouse.LeftDown();
                                          //  Task.Delay(1000).Wait();
                                        


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
                                                   new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                                            {
                                                var item = detector.GetBest(sreenCapture, true);
                                                if (item.HasValue)
                                                {
                                                    // search for tree after walking path :);
                                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                                                    foundGathering = true;
                                                    //     VirtualMouse.LeftUp();
                                                    //     VirtualMouse.LeftUp();
                                                    //    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
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

                                    distance = Distance(new Point(enemy.Value.X, enemy.Value.Y),
                                        new Point((image2.Width / 2), (image2.Height / 2)));
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
                                            textBoxTextSearch.Text = ChaosBot.ReadArea(screenCapture, x, y, width * -1,
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

        private void Debugging_MouseMove(object sender, MouseEventArgs e)
        {
            

        }

        private void Debugging_Shown(object sender, EventArgs e)
        {
            Debugging.ActiveForm.Focus();
            GetMousePosition();
            //SaveMousePosition();
        }
        private bool searchMouse = true;
        private async void GetMousePosition()
        {
            while (searchMouse)
            {
                Cursor = new Cursor(Cursor.Current.Handle);
                Point cursor = new Point(Cursor.Position.X, Cursor.Position.Y);

                lbYCoord.Text = Convert.ToString(Cursor.Position.Y);
                lbXCoord.Text = Convert.ToString(Cursor.Position.X);
               
                await Task.Delay(10);
            }
        }

        private void Debugging_KeyPress(object sender, KeyPressEventArgs e)
        {
       
           
        }

        private void Debugging_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
        private int i = 0;
        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(checkBox1.Checked)
            {
                

                if (e.KeyCode == Keys.F10)
                {   
                    i++;
                    searchMouse = false;
                    checkBox1.Checked = true;
                    string Position = Convert.ToString(lbXCoord.Text) + "\t" + Convert.ToString(lbYCoord.Text);
                   
                    textBoxX.Text = lbXCoord.Text;
                    textBoxY.Text = lbYCoord.Text;
                   if(i == 2)
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

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
    }
}