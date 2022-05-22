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

                                    ctsBossUndTimer.Cancel();
                                    ctsBossUndTimer.Dispose();
                                    ctsBossUndTimer = new CancellationTokenSource();
                                    tokenBossUndTimer = ctsBossUndTimer.Token;
                                    Task t6 = Task.Run(() => Leavetimerfloor1(tokenBossUndTimer), tokenBossUndTimer);
                                }

                                _portalIsDetected = true;
                                _portalIsNotDetected = false;
                                cts.Cancel();
                                cts.Dispose();
                                cts = new CancellationTokenSource();
                                token = cts.Token;
                                Task t5 = Task.Run(() => PORTALISDETECTED(token), token);
                                Task t7 = Task.Run(() => SEARCHPORTAL(token), token);

                            }
                            else if (!chBoxActivateF2.Checked)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                lbStatus.Invoke(
                                    (MethodInvoker)(() => lbStatus.Text = "Floor 1 Complete..."));

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

                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                cts.Cancel();
                                cts.Dispose();
                                cts = new CancellationTokenSource();
                                token = cts.Token;

                                var leave = Task.Run(() => Leavedungeon(token), token);
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

        private async Task PORTALISDETECTED(CancellationToken token)
        {
            try
            {
                _portalIsDetected = true;
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
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
                        }

                        await Task.Delay(humanizer.Next(10, 240) + 100, token);
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

    }
}