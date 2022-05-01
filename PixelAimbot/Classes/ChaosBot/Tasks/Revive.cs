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
        private async Task Revive(CancellationToken token)
        {
            try
            {
                if (chBoxRevive.Checked)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    while (_revive && _floorFight  && _stopp == false)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            float thresh = int.Parse(txtRevive.Text) * 0.01f;
                            var ReviveDeutschTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/revive1.png");
                            var ReviveDeutschMask =
                                new Image<Bgr, byte>(resourceFolder + "/revivemask1.png");

                            var ReviveEnglishTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/reviveEnglish.png");
                            var ReviveEnglishMask =
                                new Image<Bgr, byte>(resourceFolder + "/reviveEnglishmask.png");

                            var ReviveDeutschDetector =
                                new EnterDetectors(ReviveDeutschTemplate, ReviveDeutschMask, thresh);
                            var ReviveEnglishDetector =
                                new EnterDetectors(ReviveEnglishTemplate, ReviveEnglishMask, thresh);
                            var screenPrinter = new PrintScreen();
                            var rawScreen = screenPrinter.CaptureScreen();
                            Bitmap bitmapImage = new Bitmap(rawScreen);
                            var screenCapture = bitmapImage.ToImage<Bgr, byte>();
                            var ReviveDeutsch = ReviveDeutschDetector.GetClosestEnter(screenCapture);
                            var ReviveEnglish = ReviveEnglishDetector.GetClosestEnter(screenCapture);
                            if (ReviveDeutsch.HasValue || ReviveEnglish.HasValue && _floorFight == true)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                _floorFight = false;
                            
                                _potions = false;
                                DiscordSendMessage("Bot is reviving!");
                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "REVIVE!"));
                                VirtualMouse.MoveTo(Recalc(1374), Recalc(467, false), 10);
                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                                _floorFight = true;
                        
                                _potions = false;
                            }
                            var sleepTime = new Random().Next(450, 555);
                            Thread.Sleep(sleepTime);
                        }
                        catch (AggregateException)
                        {
                            Debug.WriteLine("Expected");
                        }
                        catch (ObjectDisposedException)
                        {
                            Debug.WriteLine("Bug");
                        }
                        catch
                        {
                        }
                    }
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
    }
}