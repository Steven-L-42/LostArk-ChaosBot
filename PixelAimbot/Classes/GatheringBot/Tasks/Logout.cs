using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task Logout(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                for (int i = 0; i < 1; i++)
                {
                    await Task.Delay(2000, token);

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "LOGOUT Process starts..."));
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                    await Task.Delay(2000, token);
                    VirtualMouse.MoveTo(ChaosBot.Recalc(1238), ChaosBot.Recalc(728, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    await Task.Delay(2000, token);
                    VirtualMouse.MoveTo(ChaosBot.Recalc(906), ChaosBot.Recalc(575, false), 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                    await Task.Delay(1000, token);

                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "You are logged out!"));
                    _start = false;
                    _cts.Cancel();
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