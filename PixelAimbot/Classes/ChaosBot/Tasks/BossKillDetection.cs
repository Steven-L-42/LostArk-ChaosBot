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
        public bool starten;
        public bool awakening;
        public bool gefunden;
        public bool _bossKillDetection;

        private async Task BossKillDetection(CancellationToken tokenBossUndTimer)
        {


            if (!chBoxLeavetimer.Checked && !_stopp && _bossKillDetection)
            {
                _bossKillDetection = false;

                try
                {
                    tokenBossUndTimer.ThrowIfCancellationRequested();
                    await Task.Delay(1, tokenBossUndTimer);


                    while (starten && !_stopp)
                    {

                        tokenBossUndTimer.ThrowIfCancellationRequested();
                        await Task.Delay(humanizer.Next(10, 240) + 100, tokenBossUndTimer);
                        float threshold = 0.8f;

                        var BossTemplate = Image_bossHP;
                        var BossMask = Image_bossHPmask;

                        Point myPosition = new Point(Recalc(148), Recalc(127, false));
                        Point screenResolution = new Point(screenWidth, screenHeight);

                        var BossDetector = new BossDetector(BossTemplate, BossMask, threshold);
                        var screenPrinter = new PrintScreen();

                        var rawScreen = screenPrinter.CaptureScreen();
                        Bitmap bitmapImage = new Bitmap(rawScreen);
                        using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                        {
                            var Boss = BossDetector.GetClosestEnemy(screenCapture, false);

                            if (Boss.HasValue && _stopp == false)
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "BOSS FIGHT!"));
                                gefunden = false;

                                while (!gefunden && !_stopp && chBoxAwakening.Checked)
                                {
                                    tokenBossUndTimer.ThrowIfCancellationRequested();
                                    await Task.Delay(1, tokenBossUndTimer);
                                    object Awakening = Pixel.PixelSearch(Recalc(1161), Recalc(66, false), Recalc(1187),
                                        Recalc(83, false), 0x9C1B16, 20);
                                    if (Awakening.ToString() == "0")
                                    {
                                        _doUltimateAttack = true;
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "AWAKENING..."));
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_V);
                                        await Task.Delay(500, tokenBossUndTimer);
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_V);
                                        await Task.Delay(500, tokenBossUndTimer);
                                        _doUltimateAttack = false;
                                        gefunden = true;
                                    }
                                    Random random2 = new Random();
                                    var sleepTime2 = random2.Next(100, 150);
                                    await Task.Delay(sleepTime2);
                                }
                                gefunden = true;

                            }
                            else if (!Boss.HasValue && gefunden == true && _stopp == false)
                            {
                                tokenBossUndTimer.ThrowIfCancellationRequested();
                                await Task.Delay(1, tokenBossUndTimer);
                                await Task.Delay(humanizer.Next(10, 240) + 3000, tokenBossUndTimer);

                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor2 Complete..."));
                                starten = false;
                                gefunden = false;
                                _stopp = true;
                                _portalIsDetected = false;

                                _portalIsNotDetected = false;
                                _floorFight = false;
                                _searchboss = false;
                                _revive = false;
                                _ultimate = false;
                                _portaldetect = false;
                                _potions = false;
                                _floor1 = false;
                                _floor2 = false;

                                _bard = false;
                                _gunlancer = false;
                                _shadowhunter = false;
                                _paladin = false;
                                _Glavier = false;
                                _deathblade = false;
                                _sharpshooter = false;
                                _sorcerer = false;
                                _soulfist = false;
                                _sharpshooter = false;
                                _berserker = false;

                                _doUltimateAttack = true;
                                _Q = true;
                                _W = true;
                                _E = true;
                                _R = true;
                                _A = true;
                                _S = true;
                                _D = true;
                                _F = true;

                                tokenBossUndTimer.ThrowIfCancellationRequested();
                                await Task.Delay(1, tokenBossUndTimer);


                                ctsBossUndTimer.Cancel();
                                ctsBossUndTimer.Dispose();
                                ctsBossUndTimer = new CancellationTokenSource();
                                tokenBossUndTimer = ctsBossUndTimer.Token;
                                cts.Cancel();
                                cts.Dispose();
                                cts = new CancellationTokenSource();
                                token = cts.Token;
                                var leave = Task.Run(() => Leavedungeon(token), token);
                            }

                            Random random = new Random();
                            var sleepTime = random.Next(100, 150);
                            await Task.Delay(sleepTime);
                        }
                    }
                }
                catch (AggregateException)
                {
                    Console.WriteLine("Expected");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Bug");
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
}