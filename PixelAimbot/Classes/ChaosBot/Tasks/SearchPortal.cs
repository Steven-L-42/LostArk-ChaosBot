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

                while (_portalIsDetected == true)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(humanizer.Next(10, 240) + 100, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                    var enemyTemplate = Image_portalenter1;
                    var enemyMask = Image_portalentermask1;
                    Point screenResolution = new Point(screenWidth, screenHeight);

                    // Main program loop
                    var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, 0.7f);
                    var screenPrinter = new PrintScreen();

                    using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                    {
                        var enemy = enemyDetector.GetClosestEnemy(screenCapture, false);
                        if (enemy.HasValue)
                        {
                            token.ThrowIfCancellationRequested();
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Portal found..."));

                            CvInvoke.Rectangle(screenCapture,
                                new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                new MCvScalar(255));

                            double distance_x = (screenWidth - Recalc(296)) / 2;
                            double distance_y = (screenHeight - Recalc(255, false)) / 2;

                            var friend_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                            double multiplier = 1;
                            var friend_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                            var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                            var dist = Math.Sqrt(
                                Math.Pow((my_position_on_minimap.Item1 - friend_position_on_minimap.Item1), 2) +
                                Math.Pow((my_position_on_minimap.Item2 - friend_position_on_minimap.Item2), 2));

                            if (dist < 180)
                            {
                                multiplier = 1.2;
                            }

                            double posx;
                            double posy;
                            if (friend_position.Item1 < (screenWidth / 2))
                            {
                                posx = friend_position.Item1 * (2 - multiplier);
                            }
                            else
                            {
                                posx = friend_position.Item1 * multiplier;
                            }

                            if (friend_position.Item2 < (screenHeight / 2))
                            {
                                posy = friend_position.Item2 * (2 - multiplier);
                            }
                            else
                            {
                                posy = friend_position.Item2 * multiplier;
                            }

                            var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                            token.ThrowIfCancellationRequested();
                            VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Enter Portal..."));

                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                            KeyboardWrapper.PressKey(currentMouseButton);

                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                        }
                    }

                    token.ThrowIfCancellationRequested();
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                    Random random = new Random();
                    var sleepTime = random.Next(500, 570);
                    await Task.Delay(sleepTime);
                }

                _searchSequence = 1;

                await Task.Delay(humanizer.Next(10, 240) + 8000, token);
                _searchboss = true;
                token.ThrowIfCancellationRequested();

                cts.Cancel();
                cts.Dispose();
                cts = new CancellationTokenSource();
                token = cts.Token;
                var t12 = Task.Run(() => SEARCHBOSS(token), token);
                await Task.WhenAny(new[] { t12 });

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