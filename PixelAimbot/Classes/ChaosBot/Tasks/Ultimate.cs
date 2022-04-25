﻿using System;
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
                        if (chBoxBard.Checked && _bard)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);

                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Bard try to heal..."));
                        }

                        if (chBoxGunlancer.Checked && _gunlancer ||
                            chBoxGunlancer2.Checked && _gunlancer)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                            _gunlancer = false;

                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Gunlancer Ultimate"));
                        }

                        if (chBoxY.Checked && _shadowhunter)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            object d = au3.PixelSearch(Recalc(948), Recalc(969, false), Recalc(968), Recalc(979, false),
                                0xBC08F0, 5);

                            if (d.ToString() != "0")
                            {
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _shadowhunter = false;
                                _Q = false;
                                _W = false;
                                _E = false;
                                _R = false;
                                _A = false;
                                _S = false;
                                _D = false;
                                _F = false;
                                lbStatus.Invoke(
                                    (MethodInvoker) (() => lbStatus.Text = "Activate: Shadowhunter Ultimate"));
                            }
                        }

                        if (chBoxPaladin.Checked && _paladin)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object d = au3.PixelSearch(Recalc(892), Recalc(1027, false), Recalc(934),
                                Recalc(1060, false), 0x75D6FF, 10);
                            if (d.ToString() != "0")
                            {
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _paladin = false;

                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Paladin Ultimate"));
                            }
                        }

                        if (chBoxDeathblade.Checked && _deathblade ||
                            chBoxDeathblade2.Checked && _deathblade)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object d = au3.PixelSearch(Recalc(986), Recalc(1029, false), Recalc(1017),
                                Recalc(1035, false), 0xDAE7F3, 10);
                            if (d.ToString() != "0")
                            {

                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _deathblade = false;

                                if (chBoxDeathblade2.Checked)
                                {
                                    await Task.Delay(humanizer.Next(10, 240) + 500);
                                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                                }

                                var Deathblade = Task.Run(() => DeathbladeSecondPress(token));
                                lbStatus.Invoke((MethodInvoker) (() =>
                                    lbStatus.Text = "Activate: Deathblade Ultimate"));
                            }
                        }

                        if (chBoxSharpshooter.Checked && _sharpshooter)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object d = au3.PixelSearch(Recalc(1006), Recalc(1049, false), Recalc(1019),
                                Recalc(1068, false), 0x09B4EB, 10);
                            if (d.ToString() != "0")
                            {
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _sharpshooter = false;

                                var Sharpshooter = Task.Run(() => SharpshooterSecondPress(token));

                                lbStatus.Invoke(
                                    (MethodInvoker) (() => lbStatus.Text = "Activate: Sharpshooter Ultimate"));
                            }
                        }

                        if (chBoxSorcerer.Checked && _sorcerer)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            object d = au3.PixelSearch(Recalc(1006), Recalc(1038, false), Recalc(1010),
                                Recalc(1042, false), 0x8993FF, 10);
                            if (d.ToString() != "0")
                            {
                                KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                                _sorcerer = false;

                                lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Sorcerer Ultimate"));
                            }
                        }

                        if (chBoxSoulfist.Checked && _soulfist)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);

                            KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 2000);
                            _soulfist = false;

                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Activate: Soulfist Ultimate"));
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