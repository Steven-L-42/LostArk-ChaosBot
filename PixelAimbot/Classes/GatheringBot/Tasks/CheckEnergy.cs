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
        private async Task CheckEnergy(CancellationToken token)
        {
            try
            {
                var haveEnergy = true;
                while (haveEnergy)
                {
                    if (_checkEnergy)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);

                        var template = Image_energy_fish;

                        var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(683),
                            ChaosBot.Recalc(979, false), ChaosBot.Recalc(45, true, true), ChaosBot.Recalc(33, false, true));

                        using (var screencap = _screenPrinter.CaptureScreen())
                        {
                            using (_screenCapture = new Bitmap(screencap).ToImage<Bgr, byte>())
                            {
                                var item = detector.GetBest(_screenCapture);
                                if (item.HasValue)
                                {
                                    DiscordSendMessage("No more Energy, Bot stopped!");
                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "No more Energy, Stopping"));
                                    Invoke((MethodInvoker)(() => btnPause_Click(null, null)));

                                    _cts.Cancel();
                                    haveEnergy = false;

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}