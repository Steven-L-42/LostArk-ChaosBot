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
    partial class ChaosBot
    {
        private async Task StartMove(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Set Transparency and Scale..."));

                VirtualMouse.MoveTo(Recalc(1900), Recalc(50, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                VirtualMouse.MoveTo(Recalc(1871), Recalc(260, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                VirtualMouse.MoveTo(Recalc(1902), Recalc(87, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                VirtualMouse.MoveTo(Recalc(1871), Recalc(260, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    var template = ChaosBot.byteArrayToImage(PixelAimbot.Images.questmarker);

                    var detector = new ScreenDetector(template, null, 0.92f, ChaosBot.Recalc(1890),
                        ChaosBot.Recalc(378, false), ChaosBot.Recalc(28, true, true), ChaosBot.Recalc(31, false, true));
                    var screenPrinter = new PrintScreen();
                    using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                    {
                        var item = detector.GetBest(screenCapture, false);
                        if (item.HasValue)
                        {
                            VirtualMouse.MoveTo(Recalc(1901), Recalc(389, false), 10);
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        }
                    }
                }
                catch (Exception ex)
                {
                    int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                    Debug.WriteLine("[" + line + "]" + ex.Message);
                }


                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot moves to start the Dungeon..."));

                VirtualMouse.MoveTo(Recalc(960), Recalc(529, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                if (chBoxBerserker.Checked)
                {
                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                  
                    _berserker = false;
                    await Task.Delay(humanizer.Next(10, 240) + 1000);
                    var Bersi = Task.Run(() => BerserkerSecond(token));
                }

                _floor1 = true;
                _stopp = false;
                _portalIsNotDetected = true;
                await Task.Delay(3200);
                var t16 = Task.Run(() => Floor1Detectiontimer(token));
                var t12 = Task.Run(() => Floortime(token));
                await Task.WhenAny(new[] {t12, t16});
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
    }
}