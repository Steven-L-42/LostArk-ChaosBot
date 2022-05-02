using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task Repaircheck(CancellationToken token)
        {
            while (chBoxAutoRepair.Checked)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    var template = ChaosBot.byteArrayToImage(PixelAimbot.Images.gatheringRepair);


                    var detector = new ScreenDetector(template, null, 0.85f, ChaosBot.Recalc(1456),
                        ChaosBot.Recalc(65, false), ChaosBot.Recalc(13, true, true ), ChaosBot.Recalc(11, false, true));
                    using (_screenCapture = new Bitmap(_screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                    {
                        var item = detector.GetBest(_screenCapture, false);
                        if (item.HasValue)
                        {
                            _canrepair = true;
                            _checkEnergy = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                    Debug.WriteLine("[" + line + "]" + ex.Message);
                }

                await Task.Delay(2000, token);
            }
        }
    }
}