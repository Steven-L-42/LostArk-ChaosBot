using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
      
        private async Task Start(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot is starting..."));
                _berserker = true;
                _leavetimer = 0;
                _leavetimer1 = 0;
                _leavetimer2 = 0;
                _searchSequence = 0;
                _fightSequence = 0;
                _fightSequence2 = 0;
                _Q = false;
                _W = false;
                _E = false;
                _R = false;
                _A = false;
                _S = false;
                _D = false;
                _F = false;
               


                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                Process[] processName = Process.GetProcessesByName("LostArk");
                if (processName.Length == 1)
                {
                    handle = processName[0].MainWindowHandle;
                    SetForegroundWindow(handle);
                }

                await Task.Delay(humanizer.Next(10, 240) + 500, token);
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // Für VALTAN Update
                if(chBoxValtanAltQ.Checked)
                {
                    await Task.Delay(2000, token);
                    KeyboardWrapper.MultiplePressKey(KeyboardWrapper.VK_ALT, KeyboardWrapper.VK_Q);
                    await Task.Delay(1500, token);
                    VirtualMouse.MoveTo(Recalc(866), Recalc(284, false), 10);
                    await Task.Delay(1500, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                }
                else
                {  /////////////// PRESS G TO ENTER ///////////////
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                }
            

                /////////////// CLICK ON ENTER /////////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1467), Recalc(858, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                /////////////// CLICK ON ACCEPT ///////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 2500, token);

                //cts.Cancel();
                //cts.Dispose();
                //cts = new CancellationTokenSource();
                //token = cts.Token;



              bool _ChaosStartDetect = true;
               
                while (_ChaosStartDetect == true)
                {
                    try
                    {
                        object StartDetect = Pixel.PixelSearch(Recalc(1898), Recalc(10, false), Recalc(1911),
                            Recalc(22, false), 0x000000, 15);

                        if (StartDetect.ToString() == "0")
                        {
                            _ChaosStartDetect = false;
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
                    await Task.Delay(100,token);
                }

                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                var t3 = Task.Run(() => StartMove(token), token);
                await Task.WhenAny(new[] { t3 });

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