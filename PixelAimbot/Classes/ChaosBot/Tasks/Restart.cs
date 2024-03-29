﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private async Task Restart(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Restart in " + int.Parse(txtRestart.Text) + " seconds."));
                await Task.Delay(_humanizer.Next(10, 240) + int.Parse(txtRestart.Text) * 1000);

                _stopp = true;

                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _potions = false;
                _floor1 = false;
                _floor2 = false;
                _floor3 = false;


                _restart = true;
                if (chBoxChannelSwap.Checked)
                {
                    if (_swap == 3)
                    {


                        token.ThrowIfCancellationRequested();
                        Random random = new Random();
                        await Task.Delay(1, token);
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(_humanizer.Next(10, 240) + 1000);
                        VirtualMouse.MoveTo(Recalc(1844), Recalc(44, false), 10);

                        for (int i = 0; i < random.Next(2, 10); i++)
                        {
                            VirtualMouse.Scroll(-120);
                            await Task.Delay(100);
                        }
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap++;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(_humanizer.Next(10, 240) + 2000);
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(t9);
                    }
                    else if (_swap == 6)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        Random random = new Random();
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(_humanizer.Next(10, 240) + 1000);
                        VirtualMouse.MoveTo(Recalc(1844), Recalc(64, false), 10);

                        for (int i = 0; i < random.Next(2, 10); i++)
                        {
                            VirtualMouse.Scroll(-120);
                            await Task.Delay(100);
                        }
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        _swap = 0;
                        _restart = false;
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Channel-Swap activated..."));
                        await Task.Delay(_humanizer.Next(10, 240) + 2000);
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(t9);
                    }
                }
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                if (_restart)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        for (int i = 0; i < 50; i++)
                        {
                            Cts.Cancel();
                            CtsBoss.Cancel();
                            CtsSkills.Cancel();
                            await Task.Delay(100);
                        }
                        _start = false;
                        _stopp = false;
                        _stop = false;
                        _restart = false;
                        _logout = false;


                        _gunlancer = false;
                        _shadowhunter = false;
                        _berserker = false;
                        _paladin = false;
                        _deathblade = false;
                        _sharpshooter = false;
                        _bard = false;
                        _sorcerer = false;
                        _soulfist = false;

                        _floor1 = false;
                        _floor2 = false;
                        _floor3 = false;

                        _floorFight = false;
                        _searchboss = false;

                        _revive = false;
                        _portaldetect = false;
                        _ultimate = false;
                        _potions = false;

                        _q = false;
                        _w = false;
                        _e = false;
                        _r = false;
                        _a = false;
                        _s = false;
                        _d = false;
                        _f = false;
                        Cts = new CancellationTokenSource();
                        token = Cts.Token;
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                        await Task.Delay(_humanizer.Next(10, 240) + 1000);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_F9);
                        _stop = true;
                    }
                    catch (AggregateException)
                    {
                        Debug.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.WriteLine("Bug");
                    }

                    /*
                    await Task.Delay(1000, token);
                    var t1 = Task.Run(() => START(token));
                    await Task.WhenAny(new[] { t1 });*/
                }
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