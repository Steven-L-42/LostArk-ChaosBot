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

                    var template = new Image<Bgr, byte>(_resourceFolder + "/gatheringRepair.png");


                    var detector = new ScreenDetector(template, null, 0.85f, ChaosBot.Recalc(1456),
                        ChaosBot.Recalc(65, false), ChaosBot.Recalc(13), ChaosBot.Recalc(11, false));
                    using (_screenCapture = new Bitmap(_screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                    {
                        var item = detector.GetBest(_screenCapture, true);
                        if (item.HasValue)
                        {
                            _canrepair = true;
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