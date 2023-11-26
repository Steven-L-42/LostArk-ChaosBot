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
                await Task.Delay(_humanizer.Next(10, 240) + int.Parse(txtRestart.Text) * 1000);
                
                // KLICK UNTEN RECHTS (RATGEBER)
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                VirtualMouse.MoveTo(Recalc(1743), Recalc(1042, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(_humanizer.Next(10, 240) + 2000, token);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
              
                // KLICK AUF BEGLEITER
                await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1743), Recalc(830, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                
                // KLICK AUF AMBOSS
                await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1205), Recalc(705, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // KLICK AUF REPARIEREN
                await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1085), Recalc(429, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(1085), Recalc(429, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // 2x ESCAPE REPARATUR UND BEGLEITER FENSTER SCHLIEßEN
                await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1822), Recalc(1028, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);

                
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Repair done!"));


                await Task.Delay(_humanizer.Next(10, 240) + 2000, token);
                
                _repairReset = true;

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

        private async Task NPCRepair(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                DiscordSendMessage("Bot Repairs now!");
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Repair starts in " + int.Parse(txtRestart.Text) + " seconds..."));
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay(_humanizer.Next(10, 240) + (int.Parse(txtRestart.Text) * 1000), token);

                // Klickt auf NPC
                //
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                await Task.Delay(_humanizer.Next(10, 240) + 2000, token);

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);


                // KLICK AUF REPARIEREN
                //
                await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1085), Recalc(429, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(1085), Recalc(429, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                // ESCAPE REPARATUR FENSTER SCHLIEßEN
                await Task.Delay(_humanizer.Next(10, 240) + 1500, token);
                VirtualMouse.MoveTo(Recalc(1822), Recalc(1028, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                //await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                //KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);




                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Repair done!"));


                await Task.Delay(_humanizer.Next(10, 240) + 2000, token);

                _repairReset = true;

                var t10 = Task.Run(() => Restart(token));
                await Task.WhenAny(new[] { t10 });

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