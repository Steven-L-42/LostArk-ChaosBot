using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task CheckGathering(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                var template = new Image<Bgr, byte>(_resourceFolder + "/gathering.png");
                var mask = new Image<Bgr, byte>(_resourceFolder + "/gathering.png");


                var Detector = new ScreenDetector(template, null, 0.78f, ChaosBot.Recalc(550),
                    ChaosBot.Recalc(997, false), ChaosBot.Recalc(56), ChaosBot.Recalc(54, false));
                using (_screenCapture = new Bitmap(_screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                {


                    var item = Detector.GetBest(_screenCapture, false);
                    if (item.HasValue)
                    {
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Switch to Gathering"));
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_B);
                    }
                }

                if (chBoxAutoBuff.Checked && _buff == true)
                {
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Use Buff..."));
                    VirtualMouse.MoveTo(_x + (_width / 2), _y + (_height / 2), 5);

                    await Task.Delay(1000, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_W);
                    await Task.Delay(3000, token);
                    _buff = false;
                    var bufftimer = Task.Run(() => BUFFTIMER(token));
                }

                var t3 = Task.Run(() => ThrowFishingRod(token));
                await Task.WhenAny(new[] {t3});
            }
            catch(Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}