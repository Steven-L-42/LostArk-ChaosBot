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
        private async Task Logout(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "LOGOUT starts in 20 Seconds..."));
                await Task.Delay(humanizer.Next(10, 240) + 20000, token);
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "LOGOUT Process starts..."));
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                VirtualMouse.MoveTo(Recalc(1427), Recalc(723, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                VirtualMouse.MoveTo(Recalc(906), Recalc(575, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                DiscordSendMessage("Bot logged you out!");
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "You are logged out!"));
                _start = false;
                cts.Cancel();
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}