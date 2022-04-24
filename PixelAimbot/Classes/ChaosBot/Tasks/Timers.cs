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
    partial class ChaosBot
    {
        public async void Repairtimer()
        {
            try
            {
                await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txtRepair.Text) * 1000) * 60);
                for(int i = 0; i < 1; i++)
                { _repair = true; }
                
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

        public async void Logouttimer()
        {
            try
            {
                await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txtLOGOUT.Text) * 1000) * 60);
                for (int i = 0; i < 1; i++)
                { _logout = true; }
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
            // timer.Elapsed += OnTimedEvent2;
            //timer.AutoReset = false;
            //timer.Enabled = true;
            //cts.Cancel();
        }

        public async void Leavetimerfloor1(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 30000, token);
                for (int i = 0; i < 1; i++)
                {
                    float threshold = 0.69f;

                    var enemyTemplate =
                        new Image<Bgr, byte>(resourceFolder + "/enemy.png");
                    var enemyMask =
                        new Image<Bgr, byte>(resourceFolder + "/mask.png");
                    var BossTemplate =
                                new Image<Bgr, byte>(resourceFolder + "/boss1.png");
                    var BossMask =
                        new Image<Bgr, byte>(resourceFolder + "/bossmask1.png");


                    var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                    var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);

                    var screenPrinter = new PrintScreen();

                    var rawScreen = screenPrinter.CaptureScreen();
                    Bitmap bitmapImage = new Bitmap(rawScreen);
                    var screenCapture = bitmapImage.ToImage<Bgr, byte>();

                    var enemy = enemyDetector.GetClosestEnemy(screenCapture, true);
                    var Boss = BossDetector.GetClosestEnemy(screenCapture, true);

                    if (enemy.HasValue)
                    {
                        continue;
                    }
                    else
                    if (Boss.HasValue)
                    {
                        continue;
                    }
                    else
                    {
                        _stopp = true;
                        _portalIsNotDetected = false;
                        _floorFight = false;
                        _searchboss = false;
                        _revive = false;
                        _ultimate = false;
                        _portaldetect = false;
                        _potions = false;
                        _floor1 = false;
                        _floor2 = false;
                        _floor3 = false;
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Failed to Enter Portal!"));
                        var t12 = Task.Run(() => Leavedungeon(token));
                        await Task.WhenAny(new[] { t12 });
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

        public async void Leavetimerfloor2(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1, token);

                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txLeaveTimerFloor2.Text) * 1000, token);

                _stopp = true;
                _portalIsNotDetected = false;
                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _potions = false;
                _floor1 = false;
                _floor2 = false;
                _floor3 = false;

                var t12 = Task.Run(() => Leavedungeon(token));
                await Task.WhenAny(new[] {t12});
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

        public async void Leavetimerfloor3(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txLeaveTimerFloor3.Text) * 1000, token);
                _stopp = true;
                _portalIsNotDetected = false;
                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _potions = false;
                _floor1 = false;
                _floor2 = false;
                _floor3 = false;
                var t12 = Task.Run(() => Leavedungeoncomplete(token));
                await Task.WhenAny(new[] {t12});
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