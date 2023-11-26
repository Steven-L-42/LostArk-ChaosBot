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
                _redStage = 0;
                _floorint2 = 1;
                _leave = false;
                _fightSequence2 = 0;
                _q = false;
                _w = false;
                _e = false;
                _r = false;
                _a = false;
                _s = false;
                _d = false;
                _f = false;



                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                Process[] processName = Process.GetProcessesByName("LostArk");
                if (processName.Length == 1)
                {
                    handle = processName[0].MainWindowHandle;
                    SetForegroundWindow(handle);
                }

                await Task.Delay(_humanizer.Next(10, 240) + 500, token);
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // Für VALTAN Update
                if (chBoxValtanAltQ.Checked)
                {
                    await Task.Delay(2000, token);
                    KeyboardWrapper.MultiplePressKey(KeyboardWrapper.VK_ALT, KeyboardWrapper.VK_Q);
                    await Task.Delay(3000, token);
                    VirtualMouse.MoveTo(Recalc(866), Recalc(284, false), 10);
                    await Task.Delay(2000, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                }
                else
                {  /////////////// PRESS G TO ENTER ///////////////
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                }


                /////////////// CLICK ON ENTER /////////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(_humanizer.Next(10, 240) + 1000, token);
                VirtualMouse.MoveTo(Recalc(1467), Recalc(858, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                /////////////// CLICK ON ACCEPT ///////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(_humanizer.Next(10, 240) + 1000);
                VirtualMouse.MoveTo(Recalc(889), Recalc(619, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                token.ThrowIfCancellationRequested();
                await Task.Delay(_humanizer.Next(10, 240) + 2500);


                CheckIfLoadScreen();

                await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                var t3 = Task.Run(() => StartMove(token));
                await Task.WhenAny(new[] {t3});
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