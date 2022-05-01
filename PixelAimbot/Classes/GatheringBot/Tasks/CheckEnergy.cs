using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task CheckEnergy(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    if (!_canrepair && !_minigameFound )
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);

                        var template = new Image<Bgr, byte>(_resourceFolder + "/energy_fish.png");
                        
                        var detector = new ScreenDetector(template, null, 0.9f, ChaosBot.Recalc(683),
                            ChaosBot.Recalc(979, false), ChaosBot.Recalc(45), ChaosBot.Recalc(33, false));


                        using (_screenCapture = new Bitmap(_screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                        {
                            var item = detector.GetBest(_screenCapture, false);
                            if (item.HasValue)
                            {
                                ChaosBot.DiscordSendMessage("No more Energy, Bot stopped!");
                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "No more Energy, Stopping"));
                                btnPause_Click(null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}