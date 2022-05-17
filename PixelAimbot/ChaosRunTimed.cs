using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IronOcr;




namespace PixelAimbot
{
    public partial class ChaosRunTimed : Form
    {

        public ChaosRunTimed()
        {
            InitializeComponent();
        }
       
        private async Task Test()
        {
            int iter = 0;
            await Task.Delay(2000);
            try
            {
                while (iter <=5)
                {
                    iter++;
                    float threshold = 0.8f;
                    var Guardian1 = ChaosBot.Image_GuardianStone1;
                    var Guardian1Mask = ChaosBot.Image_GuardianStone1Mask;
                    var Destruct3 = ChaosBot.Image_DestructStone3;
                    var Destruct3Mask = ChaosBot.Image_DestructStone3Mask;


                    var GuardianDetector = new StoneDetector(Guardian1, Guardian1Mask, threshold);
                    var DestructDetector = new StoneDetector(Destruct3, Destruct3Mask, threshold);
                    var screenPrinter = new PrintScreen();

                    var rawScreen = screenPrinter.CaptureScreen();
                    Bitmap bitmapImage = new Bitmap(rawScreen);
                    using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                    {
                        var Stones = GuardianDetector.GetBestEnemy(screenCapture, false);
                        var Destructs = DestructDetector.GetBestEnemy(screenCapture, false);

                        string invGuardian = "";
                        string invDestruct = "";
                        var screenPrinter2 = new PrintScreen();
                        if (Stones.HasValue)
                        {
                            lbChaosGuardian.Invoke((MethodInvoker)(() => lbChaosGuardian.Text = invGuardian));

                            VirtualMouse.MoveTo(Stones.Value.X, Stones.Value.Y - 15);

                            
                            using (var screenCapture2 = new Bitmap(screenPrinter2.CaptureScreen()).ToImage<Bgr, byte>())
                            {
                                var invStart = ChaosBot.ReadArea(screenCapture2, Stones.Value.X, Stones.Value.Y - 15, 46, 15, "");
                                invGuardian = invStart;
                                lbChaosGuardian.Invoke((MethodInvoker)(() => lbChaosGuardian.Text = invGuardian));
                            }

                        }
                        if(Destructs.HasValue)
                        {
                            lbChaosGuardian.Invoke((MethodInvoker)(() => lbChaosDestruction.Text = invDestruct));
                            VirtualMouse.MoveTo(Destructs.Value.X, Destructs.Value.Y - 15);

                            using (var screenCapture2 = new Bitmap(screenPrinter2.CaptureScreen()).ToImage<Bgr, byte>())
                            {
                                var invStart2 = ChaosBot.ReadArea(screenCapture2, Destructs.Value.X, Destructs.Value.Y - 15, 46, 15, "");
                                invDestruct = invStart2;
                                lbChaosGuardian.Invoke((MethodInvoker)(() => lbChaosDestruction.Text = invDestruct));
                            }
                        }

                    }

                    Random random = new Random();
                    var sleepTime = random.Next(250, 500);
                    await Task.Delay(sleepTime);
                }
            }
            catch (AggregateException)
            {
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }
        Bitmap startInv;
        Bitmap endInv;
        private async void ChaosRunTimed_Load(object sender, EventArgs e)
        {
           

            Bitmap start = new Bitmap(Application.UserAppDataPath + "/ChaosStartInv.jpg");
            startInv = new Bitmap(start.Width, start.Height);
            Bitmap end = new Bitmap(Application.UserAppDataPath + "/ChaosEndInv.jpg");
            endInv = new Bitmap(end.Width, end.Height);

            for (int i = 0; i < start.Width; i++)
            {
                for (int x = 0; x < start.Height; x++)
                {
                    Color oc = start.GetPixel(i, x);
                    int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                    startInv.SetPixel(i, x, nc);
                }
            }
         
            for (int j = 0; j < end.Width; j++)
            {
                for (int y = 0; y < end.Height; y++)
                {
                    Color oc = end.GetPixel(j, y);
                    int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                    Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                    endInv.SetPixel(j, y, nc);
                }
            }
            picChaosStart.Image = startInv;
            picChaosEnd.Image = endInv;

            long Full = ChaosBot.ChaosTime.Ticks+10000000;

            DateTime ChaosTimePlus1 = new DateTime(Full);
           
            this.lbChaosRunStart.Text = ChaosBot.ChaosStart.ToString("HH:mm:ss");
            this.lbChaosRunStop.Text = ChaosBot.ChaosStop.ToString("HH:mm:ss");
            this.lbChaosRunTime.Text = ChaosTimePlus1.ToString("HH:mm:ss");
            this.lbChaosAllRounds.Text = ChaosBot.ChaosAllRounds.ToString();
            this.lbChaosAllStucks.Text = ChaosBot.ChaosAllStucks.ToString();
            this.lbChaosPerfectRounds.Text = ChaosBot.ChaosPerfectRounds.ToString();
            this.lbChaosGameCrashed.Text = ChaosBot.ChaosGameCrashed.ToString();



           

            ChaosBot.cts = new CancellationTokenSource();
            var token = ChaosBot.cts.Token;
            await Task.Delay(1, token);
            var leave = Task.Run(() => Test());

        }


        public static Rectangle bounds;
       

        private void button1_Click_1(object sender, EventArgs e)
        {
            
            int Top = this.Bounds.Top;
            int Left = this.Bounds.Left;
            int Width = this.Bounds.Width;
            int Height = this.Bounds.Height;

            bounds = this.Bounds;
            Top = this.Right;
            using (Bitmap bitmap = new Bitmap(bounds.Width-20, bounds.Height-50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left+11, bounds.Top+40), Point.Empty, bounds.Size);
                }
                bitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "/SymbioticInv_" + DateTime.Now.ToString("HH.mm-[dd.MM.yyyy]") + ".jpg", ImageFormat.Jpeg);
            }
        }

      

        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Text = DateTime.Now.ToString("HH:mm:ss");
        }

       
    }
}
