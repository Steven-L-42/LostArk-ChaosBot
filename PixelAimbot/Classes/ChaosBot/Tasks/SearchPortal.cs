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
        
        private async Task SEARCHPORTAL(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                _portaldetect = false;

                while (_portalIsDetected)

                {

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(_humanizer.Next(10, 240) + 100, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                  
                    var enemyTemplate = ImagePortalenter1;
                    var enemyMask = ImagePortalentermask1;
                    Point screenResolution = new Point(ScreenWidth, ScreenHeight);

                    // Main program loop
                    var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, 0.7f);

                    using (var screenCapture = _globalScreenPrinter.CaptureScreenImage())
                    {
                        var enemy = enemyDetector.GetClosestEnemy(screenCapture);
                        if (enemy.HasValue)
                        {
                            token.ThrowIfCancellationRequested();
                            if (_redStage >= 1)
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Portal found..."));
                            }
                            else
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Portal found..."));
                            }


                            CvInvoke.Rectangle(screenCapture,
                                new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                new MCvScalar(255));

                            double distanceX = (ScreenWidth - Recalc(296)) / 2.0;
                            double distanceY = (ScreenHeight - Recalc(255, false)) / 2.0;

                            var friendPosition = ((enemy.Value.X + distanceX), (enemy.Value.Y + distanceY));
                            double multiplier = 1;
                            var friendPositionOnMinimap = ((enemy.Value.X), (enemy.Value.Y));
                            var myPositionOnMinimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                            var dist = Math.Sqrt(
                                Math.Pow((myPositionOnMinimap.Item1 - friendPositionOnMinimap.Item1), 2) +
                                Math.Pow((myPositionOnMinimap.Item2 - friendPositionOnMinimap.Item2), 2));

                            if (dist < 180)
                            {
                                multiplier = 1.4;
                            }

                            double posx;
                            double posy;
                            if (friendPosition.Item1 < (ScreenWidth / 2.0))
                            {
                                posx = friendPosition.Item1 * (2 - multiplier);
                            }
                            else
                            {
                                posx = friendPosition.Item1 * multiplier;
                            }

                            if (friendPosition.Item2 < (ScreenHeight / 2.0))
                            {
                                posy = friendPosition.Item2 * (2 - multiplier);
                            }
                            else
                            {
                                posy = friendPosition.Item2 * multiplier;
                            }

                            var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);

                            //VirtualMouse.MoveTo(Between(absolutePositions.Item1, absolutePositions.Item1),
                            //          Between(absolutePositions.Item2, absolutePositions.Item2), 10);

                           VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);
                            if (_redStage >= 1)
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 2: Enter Portal..."));
                            }
                            else
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Enter Portal..."));
                            }


                            token.ThrowIfCancellationRequested();
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                            token.ThrowIfCancellationRequested();
                            KeyboardWrapper.PressKey(_currentMouseButton);
                            token.ThrowIfCancellationRequested();
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);


                        }
                        else
                        {
                            token.ThrowIfCancellationRequested();
                            VirtualMouse.MoveTo(((ScreenWidth + _windowX) / 2 + 50), ((ScreenHeight + _windowY) / 2 + 50), 10);
                            token.ThrowIfCancellationRequested();
                            KeyboardWrapper.PressKey(_currentMouseButton);
                            token.ThrowIfCancellationRequested();
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                            token.ThrowIfCancellationRequested();
                            VirtualMouse.MoveTo(((ScreenWidth + _windowX) / 2 + 50), ((ScreenHeight + _windowY) / 2 + 50), 10);
                            token.ThrowIfCancellationRequested();
                            KeyboardWrapper.PressKey(_currentMouseButton);
                            token.ThrowIfCancellationRequested();
                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                            //VirtualMouse.MoveTo(((screenWidth + windowX) / 2 + 3), ((screenHeight + windowY) / 2 - 11), 10);

                        }

                        token.ThrowIfCancellationRequested();
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                        
                    }
                    Random random = new Random();
                    var sleepTime = random.Next(500, 570);
                    await Task.Delay(sleepTime);
                }

                _searchSequence = 1;

                await Task.Delay(_humanizer.Next(10, 240) + 8000);
                _searchboss = true;
                token.ThrowIfCancellationRequested();
                var t12 = Task.Run(() => SEARCHBOSS(token));
                await Task.WhenAny(new[] { t12 });
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