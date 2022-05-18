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
        private async Task Restart(CancellationToken token, bool fishing = true)
        {
            _checkEnergy = false;
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                _restart = true;
                if (chBoxChannelSwap.Checked == true)
                {
                     if (_swap == 15)
                    {
                        token.ThrowIfCancellationRequested();
                        Random random = new Random();
                        await Task.Delay(1, token);
                        VirtualMouse.MoveTo(ChaosBot.Recalc(1875), ChaosBot.Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(humanizer.Next(10, 240) + 1000);
                        VirtualMouse.MoveTo(ChaosBot.Recalc(1844), ChaosBot.Recalc(44, false), 10);

                        for (int i = 0; i < random.Next(2, 10); i++)
                        {
                            VirtualMouse.Scroll(-120);
                            await Task.Delay(100);
                        }
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap++;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(humanizer.Next(10, 240) + 2000);
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(t9);
                    }
                    else if (_swap == 30)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        Random random = new Random();
                        VirtualMouse.MoveTo(ChaosBot.Recalc(1875), ChaosBot.Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(humanizer.Next(10, 240) + 1000);
                        VirtualMouse.MoveTo(ChaosBot.Recalc(1844), ChaosBot.Recalc(64, false), 10);

                        for (int i = 0; i < random.Next(2, 10); i++)
                        {
                            VirtualMouse.Scroll(-120);
                            await Task.Delay(100);
                        }
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap = 0;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(humanizer.Next(10, 240) + 2000);
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(t9);
                    }
                }

                if (_restart == true)
                {
                    await Task.Delay(1000, token);
                    _minigameFound = false;
                    var t1 = Task.Run(() => CheckGathering(token, fishing));
                    await Task.WhenAny(new[] {t1});
                }
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