﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

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
                    float threshold = 0.8f; // should be disabled
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

                            VirtualMouse.MoveTo(this.Location.X + Stones.Value.X, this.Location.Y + Stones.Value.Y - ChaosBot.Recalc(25, false));

                            
                            using (screenCapture)
                            {
                                var invStart = ChaosBot.ReadArea(ChaosBot.RemoveNoise(ChaosBot.SetGrayscale(screenCapture.ToBitmap())), Stones.Value.X, Stones.Value.Y - 15, 46, 15, "");
                                Debug.WriteLine((Location.X + Destructs.Value.X) + "-" + (Location.Y + Destructs.Value.Y - 15) + "-" + 46+ "-" + 15);

                                invGuardian = invStart;
                                lbChaosGuardian.Invoke((MethodInvoker)(() => lbChaosGuardian.Text = invGuardian));
                            }

                        }
                        if(Destructs.HasValue)
                        {
                            lbChaosGuardian.Invoke((MethodInvoker)(() => lbChaosDestruction.Text = invDestruct));
                            VirtualMouse.MoveTo(this.Location.X + Destructs.Value.X,this.Location.Y +  Destructs.Value.Y - ChaosBot.Recalc(25, false));

                            using (screenCapture)
                            {
                                var invStart2 = ChaosBot.ReadArea(ChaosBot.RemoveNoise(ChaosBot.SetGrayscale(screenCapture.ToBitmap())), Destructs.Value.X, Destructs.Value.Y - 15, 46, 15, "");
                                Debug.WriteLine(Location.X + Destructs.Value.X + "-" + (Location.Y + Destructs.Value.Y - 15) + "-" + 46+ "-" + 15);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }

        }

        private async void ChaosRunTimed_Load(object sender, EventArgs e)
        {
            TopMost = true;
            this.Size = new Size(ChaosBot.Recalc(Size.Width), ChaosBot.Recalc(Size.Height, false));
            
            splitContainer2.Size = new Size(ChaosBot.Recalc(splitContainer2.Size.Width), ChaosBot.Recalc(splitContainer2.Size.Height, false));
            splitContainer2.SplitterDistance = splitContainer2.Size.Width/2;
            
            picChaosStart.MaximumSize = picChaosStart.MinimumSize = picChaosStart.Size = new Size(ChaosBot.Recalc(544), ChaosBot.Recalc(640, false));
            
            picChaosEnd.MaximumSize = picChaosEnd.MinimumSize = picChaosEnd.Size = new Size(ChaosBot.Recalc(544), ChaosBot.Recalc(640, false));
            
            picChaosStart.Image = ChaosBot.StartInventar;
            picChaosEnd.Image = ChaosBot.EndInventar;

            long Full = ChaosBot.ChaosTime.Ticks+10000000;

            DateTime ChaosTimePlus1 = new DateTime(Full);
            ChaosBot.ChaosPerfectRounds = ChaosBot.ChaosAllRounds - ChaosBot.ChaosAllStucks;
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
            TopMost = false;

            bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width-20, bounds.Height-50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left+11, bounds.Top+40), Point.Empty, bounds.Size);
                }
                bitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "/SymbioticInv_" + DateTime.Now.ToString("HH.mm-[dd.MM.yyyy]") + ".jpg", ImageFormat.Jpeg);
                
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);
               
            }
        }
    
        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Text = DateTime.Now.ToString("HH:mm:ss");
        }
       
     
    }
}
