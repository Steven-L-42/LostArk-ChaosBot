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
        private async Task Restart(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Restart in " + int.Parse(txtRestart.Text) + " seconds."));
                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txtRestart.Text) * 1000);
                starten = false;
                gefunden = false;
                _stopp = true;
                _portalIsDetected = false;

                _portalIsNotDetected = false;
                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _potions = false;
                _floor1 = false;
                _floor2 = false;

                _bard = false;
                _gunlancer = false;
                _shadowhunter = false;
                _paladin = false;
                _deathblade = false;
                _sharpshooter = false;
                _sorcerer = false;
                _soulfist = false;
                _sharpshooter = false;
                _berserker = false;
                _doUltimateAttack = true;
                _Q = true;
                _W = true;
                _E = true;
                _R = true;
                _A = true;
                _S = true;
                _D = true;
                _F = true;
                if (chBoxChannelSwap.Checked)
                {
                    if (_swap == 3)
                    {


                        token.ThrowIfCancellationRequested();
                        Random random = new Random();
                        await Task.Delay(1, token);
                        VirtualMouse.MoveTo(Recalc(1875), Recalc(16, false), 10);
                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                        await Task.Delay(humanizer.Next(10, 240) + 1000);
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
                        await Task.Delay(humanizer.Next(10, 240) + 2000);
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
                        await Task.Delay(humanizer.Next(10, 240) + 1000);
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
                        await Task.Delay(humanizer.Next(10, 240) + 2000);
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(t9);
                    }
                }

                if (_restart)
                {
                    try
                    {
                     
                        token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            ChaosAllRounds++;
                            cts.Cancel();
                            starten = false;
                            gefunden = false;
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

                            _floorFight = false;
                            _searchboss = false;

                            _revive = false;
                            _portaldetect = false;
                            _ultimate = false;
                            _potions = false;

                            _Q = false;
                            _W = false;
                            _E = false;
                            _R = false;
                            _A = false;
                            _S = false;
                            _D = false;
                            _F = false;
                            await Task.Delay(humanizer.Next(10, 240) + 1000);
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