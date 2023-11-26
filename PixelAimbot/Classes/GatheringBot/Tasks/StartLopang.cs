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
        private async Task StartLopang(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                Process[] processName = Process.GetProcessesByName("LostArk");
                if (processName.Length == 1)
                {
                    handle = processName[0].MainWindowHandle;
                    SetForegroundWindow(handle);
                }
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Pressing Escape..."));

                KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                await Task.Delay(100, token);
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Swap Character..."));
                VirtualMouse.MoveTo(ChaosBot.Recalc(421), ChaosBot.Recalc(735, false), 10);
                await Task.Delay(100, token);
                VirtualMouse.LeftClick();
                foreach (var character in lopangCharacters)
                {
                    if (character.activated)
                    {
                        var t1 = Task.Run(() => DoLopangAction(token, character.characterPlace), token);

                        await Task.WhenAny(new[] { t1 });
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_ESCAPE);
                        await Task.Delay(100, token);
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Swap Character..."));
                        VirtualMouse.MoveTo(ChaosBot.Recalc(421), ChaosBot.Recalc(735, false), 10);
                        await Task.Delay(100, token);
                        VirtualMouse.LeftClick();
                    }
                }
                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Done..."));
                DiscordSendMessage("Lopang done");

                
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}