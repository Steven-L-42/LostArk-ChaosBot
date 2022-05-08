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
        private async Task Repair(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                DiscordSendMessage("Bot Repairs now!");
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Repair starts in " + int.Parse(txtRestart.Text) + " seconds..."));
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txtRestart.Text) * 1000);

                // KLICK UNTEN RECHTS (RATGEBER)
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                VirtualMouse.MoveTo(Recalc(1741), Recalc(1040, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(humanizer.Next(10, 240) + 2000, token);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // KLICK AUF BEGLEITER
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1684), Recalc(823, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // KLICK AUF AMBOSS
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1256), Recalc(693, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // KLICK AUF REPARIEREN
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1085), Recalc(429, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(1085), Recalc(429, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // 2x ESCAPE REPARATUR UND BEGLEITER FENSTER SCHLIEßEN
                await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);

                
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Repair done!"));


                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                _repair = false;
                _RepairReset = true;

                var t10 = Task.Run(() => Restart(token));
                await Task.WhenAny(new[] {t10});
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