using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task FishingMinigame(CancellationToken token)
        {
            while (true)
            {
                if (_canrepair == false)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);

                        var template = Image_fishing_minigame;
                        
                        var detector = new ScreenDetector(template, null, 0.96f, ChaosBot.Recalc(844), ChaosBot.Recalc(499, false), ChaosBot.Recalc(219, true,true), ChaosBot.Recalc(215, false, true));
                        var screenPrinter = new PrintScreen();
                        using(var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>()) {

                            var item = detector.GetBest(screenCapture, false);
                            if (item.HasValue)
                            {
                                _minigameFound = true;
                                _checkEnergy = false;
                            }
                        }

                    }
                    catch { }
                }
            }
        }
    }
}