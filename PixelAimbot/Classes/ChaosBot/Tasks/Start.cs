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

                /////////////// ANTI KICK ///////////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(screenWidth/2+3), Recalc(screenHeight/2-11, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(screenWidth/2-1), Recalc(screenHeight/2-8, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(humanizer.Next(10, 240) + 500, token);

                /////////////// PRESS G TO ENTER ///////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                /////////////// CLICK ON ENTER /////////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                VirtualMouse.MoveTo(Recalc(1467), Recalc(858, false), 10);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                /////////////// CLICK ON ACCEPT ///////////////
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1000);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 9000, token);
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