using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task ThrowFishingRod(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                if (_logout)
                {
                    var t3 = Task.Run(() => Logout(token));
                    await Task.WhenAny(new[] {t3});
                }
                var fishing = true;
                _rodCounter++;
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Fishing (" + _rodCounter + ")..."));

                VirtualMouse.MoveTo(_x + (_width / 2), _y + (_height / 2), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_Q);
                await Task.Delay(3000, token);
                
                int failCounter = 0;

                var template = Image_attention2;

                var detector = new ScreenDetector(template, null, 0.91f, ChaosBot.Recalc(950),
                    ChaosBot.Recalc(465, false), ChaosBot.Recalc(20, true, true), ChaosBot.Recalc(44, false, true));
                detector.setMatchingMethod(TemplateMatchingType.SqdiffNormed);
                while (fishing)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        _rawScreen = _screenPrinter.CaptureScreen();
                        _bitmapImage = new Bitmap(_rawScreen);
                        using (_screenCapture = _bitmapImage.ToImage<Bgr, byte>())
                        {
                            var item = detector.GetClosest(_screenCapture, false);
                            if (item.HasValue)
                            {
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_Q);
                                await Task.Delay(500, token);
                                _swap++;
                                fishing = false;
                                lbStatus.Invoke((MethodInvoker) (() =>
                                    lbStatus.Text = "Fishing (" + _rodCounter + ") success..."));
                            }
                            else
                            {
                                failCounter++;
                                if (failCounter > 80)
                                {
                                    fishing = false;
                                    lbStatus.Invoke((MethodInvoker) (() =>
                                        lbStatus.Text = "Fishing (" + _rodCounter + ") failed..."));
                                }
                            }
                        }
                    }
                    catch
                    {
                        fishing = false;
                    }

                    await Task.Delay(100, token);
                }
                Random rnd = new Random();

                await Task.Delay((5500 + rnd.Next(10, 200)), token);
                if (_canrepair)
                {
                    var t3 = Task.Run(() => RepairTask(token));
                    _canrepair = false;
                    await Task.WhenAny(new[] {t3});
                }
                else if (_buff)
                {
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Use Buff..."));
                    VirtualMouse.MoveTo(_x + (_width / 2), _y + (_height / 2), 5);

                    await Task.Delay(1000, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_W);
                    await Task.Delay(5000, token);
                    _buff = false;
                    var t3 = Task.Run(() => Restart(token));
                    await Task.WhenAny(new[] {t3});
                }
                else if (_minigameFound)
                {
                    
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Do Minigame ..."));
                    VirtualMouse.MoveTo(_x + (_width / 2), _y + (_height / 2), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_E);
                    await Task.Delay(4000);
                    
                    var runTime = DateTime.Now.AddSeconds(10);
                    while (runTime > DateTime.Now)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            var minigametemplate = Image_minigame;

                            var minigamedetector = new ScreenDetector(minigametemplate, null, 0.90f,
                                ChaosBot.Recalc(488), ChaosBot.Recalc(109, false), ChaosBot.Recalc(71, true, true),
                                ChaosBot.Recalc(439, false, true));
                            var screenPrinter = new PrintScreen();
                            using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                            {

                                var item = minigamedetector.GetBest(screenCapture, false, false);
                                if (item.HasValue)
                                {
                                    
                                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_SPACE);
                                    await Task.Delay(65);
                                    
                                }
                            }

                        }
                        catch
                        {
                        }
                    }
                    var t3 = Task.Run(() => Restart(token));
                    await Task.WhenAny(new[] {t3});

                }
                else
                {
                    var t3 = Task.Run(() => Restart(token));
                    await Task.WhenAny(new[] {t3});
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