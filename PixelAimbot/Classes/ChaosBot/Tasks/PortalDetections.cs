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

        private async Task Portaldetect(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                while (_portaldetect && _stopp == false)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        object fight = Pixel.PixelSearch(Recalc(114), Recalc(208, false), Recalc(168), Recalc(220, false),
                            0xDBC7AC, 7);
                        if (fight.ToString() != "0")
                        {
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Portal detected!"));

                            if (chBoxActivateF2.Checked)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                _potions = false;
                                _revive = false;
                                _ultimate = false;
                                _floor1 = false;
                                _floorFight = false;


                                if (_floorint2 == 2)
                                {
                                    _floor2 = false;
                                }

                                _leavetimer1++;
                                if (_leavetimer1 == 1 && chBoxUnstuckF1.Checked)
                                {

                                    Task t6 = Task.Run(() => Leavetimerfloor1());
                                }
                               
                                _portalIsDetected = true;
                                _portalIsNotDetected = false;
                                token.ThrowIfCancellationRequested();
                                cts.Cancel();
                                cts.Dispose();
                                cts = new CancellationTokenSource();
                                token = cts.Token;
                                Task t5 = Task.Run(() => PORTALISDETECTED(tokenDetections), tokenDetections);
                                Task t7 = Task.Run(() => SEARCHPORTAL(tokenDetections), tokenDetections);

                            }
                            else if (!chBoxActivateF2.Checked)
                            {
                              
                                lbStatus.Invoke(
                                    (MethodInvoker)(() => lbStatus.Text = "Floor 1 Complete..."));

                                var leave = Task.Run(() => Leavedungeon());
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

        private async Task PORTALISDETECTED(CancellationToken tokenDetections)
        {
            try
            {
                _portaldetect = false;
                tokenDetections.ThrowIfCancellationRequested();
                await Task.Delay(1, tokenDetections);
                while (_portalIsDetected == true)
                {
                    try
                    {
                        object health10 = Pixel.PixelSearch(Recalc(1898), Recalc(10, false), Recalc(1911),
                            Recalc(22, false), 0x000000, 15);

                        if (health10.ToString() != "0")
                        {
                            _portalIsDetected = false;
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Portalsearch done!"));
                            await Task.Delay(humanizer.Next(10, 240) + 8000, tokenDetections);
                            _searchSequence = 1;
                            _searchboss = true;
                            var t12 = Task.Run(() => SEARCHBOSS(tokenSearchBoss), tokenSearchBoss);
                            tokenDetections.ThrowIfCancellationRequested();
                            ctsDetections.Cancel();
                            ctsDetections.Dispose();
                            ctsDetections = new CancellationTokenSource();
                            tokenDetections = ctsDetections.Token;
                        }

                        await Task.Delay(humanizer.Next(10, 240) + 100, tokenDetections);
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
                    await Task.Delay(100, tokenDetections);
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