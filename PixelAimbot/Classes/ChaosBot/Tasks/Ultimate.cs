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
        private async Task UltimateAttack(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (_ultimate && _floorFight && _stopp == false)
                {
                    try
                    {



                        if (cmbBard.InvokeRequired)
                        {
                            cmbBard.Invoke(new Action(() =>
                            {
                                if (cmbBard.SelectedIndex == 1 && _bard ||
                                    cmbBard.SelectedIndex == 2 && _bard)
                                {
                                    _doUltimateAttack = true;
                                    token.ThrowIfCancellationRequested();
                             
                                    if (cmbBard.SelectedIndex == 1 || cmbBard.SelectedIndex == 2)
                                    {
                                       
                                        KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                    }

                                    if (cmbBard.SelectedIndex == 2)
                                    {
                                        Task.Delay(_humanizer.Next(10, 240) + 1000, token).Wait();
                                        KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                    }

                                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Bard try to heal..."));

                                }
                            }));
                        }

                        if (cmbGunlancer.InvokeRequired)
                        {
                            cmbGunlancer.Invoke(new Action(() =>
                            {
                                if (cmbGunlancer.SelectedIndex == 1 && _gunlancer ||
                                    cmbGunlancer.SelectedIndex == 2 && _gunlancer)
                                {
                                    _doUltimateAttack = true;

                                    Task.Delay(1000, token).Wait();
                                    KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                    _gunlancer = false;

                                    lbStatus.Invoke((MethodInvoker) (() =>
                                        lbStatus.Text = "Activate: Gunlancer Ultimate"));
                                }
                            }));
                        }



                        if (chBoxY.Checked && _shadowhunter)
                        {
                            _doUltimateAttack = true;
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            await Task.Delay(1000, token);
                            object d = Pixel.PixelSearch(Recalc(982), Recalc(1014, false), Recalc(1000),
                                Recalc(1029, false),
                                0xFFA0FF, 20);

                            if (d.ToString() != "0")
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _shadowhunter = false;
                                _q = false;
                                _w = false;
                                _e = false;
                                _r = false;
                                _a = false;
                                _s = false;
                                _d = false;
                                _f = false;
                                GetSkillQ();
                                GetSkillW();
                                GetSkillE();
                                GetSkillR();
                                GetSkillA();
                                GetSkillS();
                                Task Deathblade = Task.Run(() => ShadowhunterSecond(token), token);
                                lbStatus.Invoke(
                                    (MethodInvoker) (() => lbStatus.Text = "Activate: Shadowhunter Ultimate"));

                            }
                        }

                        if (chBoxPaladin.Checked && _paladin)
                        {
                            _doUltimateAttack = true;
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            await Task.Delay(1000, token);
                            object d = Pixel.PixelSearch(Recalc(892), Recalc(1027, false), Recalc(934),
                                Recalc(1060, false), 0x75D6FF, 10);
                            if (d.ToString() != "0")
                            {
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _paladin = false;
                                Task Deathblade = Task.Run(() => PaladinTimer(token), token);
                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Paladin Ultimate"));
                            }
                        }

                        if (chBoxGlavier.Checked && _glavier)
                        {
                            _doUltimateAttack = true;
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            await Task.Delay(1000, token);
                            object d = Pixel.PixelSearch(Recalc(993), Recalc(971, false), Recalc(1005),
                                Recalc(981, false), 0xF2F2F2, 10);
                            if (d.ToString() != "0")
                            {
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _glavier = false;

                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Glavier Ultimate"));
                            }
                        }

                        if (cmbDeathblade.InvokeRequired)
                        {
                            cmbDeathblade.Invoke(new Action(() =>
                            {
                                if (cmbDeathblade.SelectedIndex == 1 && _deathblade ||
                                    cmbDeathblade.SelectedIndex == 2 && _deathblade ||
                                    cmbDeathblade.SelectedIndex == 3 && _deathblade)
                                {
                                    _doUltimateAttack = true;
                                    token.ThrowIfCancellationRequested();
                                    Task.Delay(1, token).Wait();
                                    Task.Delay(1000, token).Wait();
                                    object d = Pixel.PixelSearch(Recalc(986), Recalc(1029, false), Recalc(1017),
                                        Recalc(1035, false), 0xDAE7F3, 10);
                                    if (d.ToString() != "0")
                                    {

                                        KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                        _deathblade = false;
                                        token.ThrowIfCancellationRequested();
                                        //    await Task.Delay(1, token);
                                        if (cmbDeathblade.SelectedIndex == 2 || cmbDeathblade.SelectedIndex == 3)
                                        {
                                            Task.Delay(_humanizer.Next(10, 240) + 500, token).Wait();
                                            KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                            KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                        }

                                        if (cmbDeathblade.SelectedIndex == 1 || cmbDeathblade.SelectedIndex == 3)
                                        {
                                            var Deathblade = Task.Run(() => DeathbladeSecondPress(token), token);
                                        }

                                        lbStatus.Invoke((MethodInvoker) (() =>
                                            lbStatus.Text = "Activate: Deathblade Ultimate"));
                                    }
                                }
                            }));
                        }
                        
                        if (cmbDestroyer.InvokeRequired)
                        {
                            cmbDestroyer.Invoke(new Action(() =>
                            {
                                if (cmbDestroyer.SelectedIndex == 1 && _destroyer ||
                                    cmbDestroyer.SelectedIndex == 2 && _destroyer)
                                {

                                    token.ThrowIfCancellationRequested();
                                    Task.Delay(1, token).Wait();
                                    Task.Delay(1000, token).Wait();
                                    object d = Pixel.PixelSearch(Recalc(947), Recalc(970, false), Recalc(979),
                                        Recalc(999, false), 0x54C8CD, 10);

                                    //bool DestroyerCounted = false;

                                    if (d.ToString() != "0")
                                    {
                                        _destroyer = false;
                                        for (int i = 0; i <= 5; i++)
                                        {
                                            if (d.ToString() != "0")
                                            {
                                                _destroyerCounter++;
                                                //DestroyerCounted = true;
                                            }

                                            Task.Delay(1000, token).Wait();
                                        }

                                        if (_destroyerCounter >= 6)
                                        {
                                            _doUltimateAttack = true;

                                            _destroyer = false;
                                            token.ThrowIfCancellationRequested();
                                            Task.Delay(1, token).Wait(token);
                                            Task.Delay(1000, token).Wait(token);
                                            KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                            KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                            KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));


                                            token.ThrowIfCancellationRequested();
                                            Task.Delay(1, token).Wait(token);
                                            if (cmbDestroyer.SelectedIndex == 2)
                                            {
                                                Task.Delay(_humanizer.Next(10, 240) + 1000, token).Wait(token);
                                                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                            }

                                            if (cmbDestroyer.SelectedIndex == 1 || cmbDestroyer.SelectedIndex == 2)
                                            {
                                                var Destroyer = Task.Run(() => DestroyerTimer(token), token);
                                            }

                                            lbStatus.Invoke((MethodInvoker) (() =>
                                                lbStatus.Text = "Activate: Destroyer Ultimate"));
                                        }
                                        else if (_destroyerCounter < 6)
                                        {
                                            _destroyerCounter = 0;
                                            _destroyer = true;
                                        }

                                    }
                                }
                            }));
                        }


                        if (chBoxSharpshooter.Checked && _sharpshooter)
                        {
                            _doUltimateAttack = true;
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            await Task.Delay(1000, token);
                            object d = Pixel.PixelSearch(Recalc(1006), Recalc(1049, false), Recalc(1019),
                                Recalc(1068, false), 0x09B4EB, 10);
                            if (d.ToString() != "0")
                            {
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _sharpshooter = false;
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token), token);

                                lbStatus.Invoke(
                                    (MethodInvoker)(() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
                            }
                        }

                        if (chBoxSorcerer.Checked && _sorcerer)
                        {
                            _doUltimateAttack = true;
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            await Task.Delay(1000, token);
                            object d = Pixel.PixelSearch(Recalc(1006), Recalc(1038, false), Recalc(1010),
                                Recalc(1042, false), 0x8993FF, 10);
                            if (d.ToString() != "0")
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _sorcerer = false;

                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
                            }
                        }

                        if (chBoxSoulfist.Checked && _soulfist)
                        {
                            _doUltimateAttack = true;
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            await Task.Delay(1000, token);
                            KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                            _soulfist = false;

                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Activate: Soulfist Ultimate"));
                        }

                        if (cmbDestroyer.InvokeRequired)
                        {
                            cmbDestroyer.Invoke(new Action(() =>
                            {
                                if (cmbDestroyer.SelectedIndex != 1 || cmbDestroyer.SelectedIndex != 2)
                                {
                                    _doUltimateAttack = false;
                                }
                            }));
                        }
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