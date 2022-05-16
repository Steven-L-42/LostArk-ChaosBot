using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using EpPathFinding.cs;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;


namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task SearchNextPoint(CancellationToken token)
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

                _gatheringCounter++;
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Gathering (" + _gatheringCounter + ")..."));


                var template = new Image<Bgr, byte>(
                    @"G:\test\clone\PixelAimbot\Resources\wood_new2.png");

                var detector = new ScreenDetector(template, null, 0.80f, ChaosBot.Recalc(1593),
                    ChaosBot.Recalc(39, false), ChaosBot.Recalc(294, true, true), ChaosBot.Recalc(255, false, true));
                detector.setMatchingMethod(TemplateMatchingType.SqdiffNormed);

                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _rawScreen = _screenPrinter.CaptureScreen();
                    _bitmapImage = new Bitmap(_rawScreen);
                    Point? enemy;
                    using (_screenCapture = _bitmapImage.ToImage<Bgr, byte>())
                    {
                        _screenCapture.ROI = new Rectangle(1593, 39, 294, 255);
                        enemy = detector.GetClosest(_screenCapture, false);

                        var scaledImage = _screenCapture.Copy();
                        Image<Gray, Byte> image1 = scaledImage.Convert<Gray, Byte>();

                        image1._Dilate(2);
                        image1._Erode(2);
                        image1._ThresholdBinary(new Gray(100), new Gray(255));
                        //image1._ThresholdBinary(127, 255);
                        Image<Bgr, Byte> image2 = image1.Convert<Bgr, Byte>();

                        Pixel pix = new Pixel();
                        Bitmap imageBitmap = image2.ToBitmap();

                        BaseGrid grid = pix.PNGtoGrid(imageBitmap);
                        List<GridPos> path = pix.findPath(
                            grid,
                            DiagonalMovement.OnlyWhenNoObstacles,
                            new GridPos((image2.Width / 2), (image2.Height / 2)),
                            new GridPos(enemy.Value.X + (template.Width / 2), enemy.Value.Y + 10)
                        );
                        /*Show on Image */
                        pix.addPath(path, imageBitmap);

                        /*Show on Image */
                        var i = 0;

                        if (path.Count - 1 >= 2 && path.Count - 1 < path.Count)
                        {
                            int last_posx = 0;
                            int last_posy = 0;
                            
                            var result = ChaosBot.MinimapToDesktop(path[2].x, path[2].y);
                            VirtualMouse.MoveTo((int) result.Item1, (int) result.Item2);
                            last_posx = (int) result.Item1;
                            last_posy = (int) result.Item2;
                            //                          VirtualMouse.LeftClick();
                            // VirtualMouse.LeftDown();
                            //  Task.Delay(1000).Wait();

/*
                            try
                            {
                                template =
                                    new Image<Bgr, byte>(
                                        @"G:\test\clone\PixelAimbot\Resources\g.png"); // icon of the enemy
                                var mask = new Image<Bgr, byte>(
                                    @"G:\test\clone\PixelAimbot\Resources\g.png"); // icon of the enemy


                                detector = new ScreenDetector(template, mask, 0.96f,
                                    ChaosBot.Recalc(711),
                                    ChaosBot.Recalc(119, false), ChaosBot.Recalc(1073),
                                    ChaosBot.Recalc(956, false));
                                using (var sreenCapture =
                                       new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                                {
                                    var item = detector.GetBest(sreenCapture, true);
                                    if (item.HasValue)
                                    {
                                        // search for tree after walking path :);
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                                        foundGathering = true;
                                        //     VirtualMouse.LeftUp();
                                        //     VirtualMouse.LeftUp();
                                        //    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                                    }
                                    else
                                    {
                                        // Not Found
                                    }
                                }
                            }
                            catch
                            {
                            }
                            */
                        }
                    }
                }
                catch
                {
                    
                }

                await Task.Delay(100, token);

                Random rnd = new Random();

                await Task.Delay((5500 + rnd.Next(10, 200)), token);
                if (_canrepair)
                {
                    var t3 = Task.Run(() => RepairTask(token));
                    _canrepair = false;
                    await Task.WhenAny(new[] {t3});
                }
                else
                {
                    var t3 = Task.Run(() => Restart(token, false));
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