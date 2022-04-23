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
        private async Task Restart(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                _restart = true;
                if (chBoxChannelSwap.Checked == true)
                {
                    if (_swap == 15)
                    {
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(1000);
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(43, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        _swap++;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(2000);
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(new[] {t9});
                    }
                    else if (_swap == 30)
                    {
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(1000);
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(63, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        _swap++;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(2000);
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(new[] {t9});
                    }
                    else if (_swap == 45)
                    {
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(1000);
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(83, false), 10);
                        KeyboardWrapper.HoldKey(KeyboardWrapper.VK_LBUTTON, 2000);
                        _swap = 0;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(2000);
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(new[] {t9});
                    }
                }

                if (_restart == true)
                {
                    await Task.Delay(1000, token);
                    var t1 = Task.Run(() => CheckGathering(token));
                    await Task.WhenAny(new[] {t1});
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