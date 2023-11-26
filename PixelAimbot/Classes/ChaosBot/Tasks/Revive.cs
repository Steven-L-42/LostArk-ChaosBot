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
                    while (_floorFight && _stopp == false)
                    {

                        try
                        {
                            if (_revive)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);

                                    if (radioGerman.Checked)
                                    {
                                        var template = ImageDeath;
                                        var detector = new ScreenDetector(template, null, float.Parse(txtDeath.Text) * 0.01f, ChaosBot.Recalc(1196), ChaosBot.Recalc(77, false), ChaosBot.Recalc(366), ChaosBot.Recalc(587, false));
                                        using (var screenCapture = _globalScreenPrinter.CaptureScreenImage())
                                        {

                                            var item = detector.GetBest(screenCapture, true);
                                            screenCapture.Dispose();
                                            if (item.HasValue && _floorFight)
                                            {
                                                token.ThrowIfCancellationRequested();
                                                await Task.Delay(1, token);

                                                _doUltimateAttack = true;
                                                _potions = false;

                                                DiscordSendMessage("Bot is reviving!");
                                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "REVIVE!"));
                                                VirtualMouse.MoveTo(Recalc(1374), Recalc(467, false), 7);
                                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                                                _doUltimateAttack = false;
                                                _potions = true;
                                            }
                                        }
                                    }
                                    else if (radioEnglish.Checked)
                                    {
                                        var template = ImageDeathEn;
                                        var detector = new ScreenDetector(template, null, float.Parse(txtDeath.Text) * 0.01f, ChaosBot.Recalc(1196), ChaosBot.Recalc(75, false), ChaosBot.Recalc(364), ChaosBot.Recalc(549, false));
                                        using (var screenCapture = _globalScreenPrinter.CaptureScreenImage())
                                        {

                                            var item = detector.GetBest(screenCapture, true);
                                            screenCapture.Dispose();
                                            if (item.HasValue && _floorFight)
                                            {
                                                token.ThrowIfCancellationRequested();
                                                await Task.Delay(1, token);

                                                _doUltimateAttack = true;
                                                _potions = false;

                                                DiscordSendMessage("Bot is reviving!");
                                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "REVIVE!"));
                                                VirtualMouse.MoveTo(Recalc(1379), Recalc(429, false), 7);
                                                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                                                _doUltimateAttack = false;
                                                _potions = true;
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
                        var sleepTime = new Random().Next(450, 555);
                        await Task.Delay(sleepTime);
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