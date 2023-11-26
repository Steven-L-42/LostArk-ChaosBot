using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private async Task Floortime(CancellationToken token)
        {
            try
            {
                Process[] processName = Process.GetProcessesByName("LostArk");
                if (processName.Length == 0 && chBoxCrashDetection.Checked)
                {
                    GameCrashed();
                    return;
                }
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                #region Floor1
                if (_floor1 && _stopp == false)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _walktopUTurn = 0;

                    // START TASK BOOLS //
                    _floorFight = true;
                    _revive = true;
                    _ultimate = true;
                    _portaldetect = true;
                    _potions = true;
                    // CLASSES //
                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
                    _paladin = true;
                    _destroyer = true;
                    _deathblade = true;
   

                    _sharpshooter = true;
                    _sorcerer = true;
                    _soulfist = true;
                    _glavier = true;
                    var t14 = Task.Run(() => UltimateAttack(token));
                    var t11 = Task.Run(() => SearchNearEnemys(token));
                    var t12 = Task.Run(() => Floorfight(token));
                    var t16 = Task.Run(() => Revive(token));
                    var t18 = Task.Run(() => Portaldetect(token));
                    var t20 = Task.Run(() => Potions(token));
                    await Task.WhenAny(t11, t12, t14, t16, t18, t20);
                }
                #endregion
                #region Floor2
                if (_floor2 && _stopp == false && _redStage == 0)
                {
                    if (processName.Length == 0 && chBoxCrashDetection.Checked)
                    {
                        GameCrashed();
                        return;
                    }
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _fightSequence++;
                    _leavetimer++;
                    // START TASK BOOLS //
                    _floorFight = true;
                    _revive = true;
                    _ultimate = true;
                    _potions = true;

                    // CLASSES //
                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
                    _sharpshooter = true;
                    _bard = true;
                    _sorcerer = true;
                    _soulfist = true;
                    _glavier = true;


                    var t11 = Task.Run(() => SearchNearEnemys(token));
                    var t12 = Task.Run(() => Floorfight(token));
                    var t16 = Task.Run(() => Revive(token));
                    var t20 = Task.Run(() => Potions(token));

                    await Task.Delay(_humanizer.Next(10, 240) + int.Parse(txtDungeon2.Text) * 1000);

                    if (_redStage == 0)
                    {
                        _floorFight = false;
                        _potions = false;
                        _revive = false;
                        _searchboss = true;
                        var t13 = Task.Run(() => SEARCHBOSS(token));
                        await Task.WhenAny(t13);
                    }

                    await Task.WhenAny(t11, t12, t16, t20);
                }
                #endregion
                if (_floor3 && _stopp == false && _redStage >= 0)
                {
                    if (processName.Length == 0 && chBoxCrashDetection.Checked)
                    {
                        GameCrashed();
                        return;
                    }
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _fightSequence++;
                    _leavetimer++;

                    // START TASK BOOLS //
                    _floorFight = true;
                    _revive = true;
                    _ultimate = true;
                    _potions = true;

             
                    // CLASSES //
                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
             
                    _sharpshooter = true;
                    _bard = true;
                    _sorcerer = true;
                    _soulfist = true;
                    _glavier = true;


                    var t11 = Task.Run(() => SearchNearEnemys(token));
                    var t12 = Task.Run(() => Floorfight(token));
                    var t16 = Task.Run(() => Revive(token));
                    var t20 = Task.Run(() => Potions(token));
                    await Task.WhenAny(t11, t12, t16, t20);
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

        private async Task SearchNearEnemys(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                var template = ImageRedHp;
                var detector = new ScreenDetector(template, null, 0.94f,
                    ChaosBot.Recalc(460),
                    ChaosBot.Recalc(120, false),
                    ChaosBot.Recalc(1000, true, true),
                    ChaosBot.Recalc(780, false, true));

                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
            
                while (_floorFight && _stopp == false)
                {
                    if (_canSearchEnemys)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            using (var screenCapture = _globalScreenPrinter.CaptureScreenImage())
                            {
                                var item = detector.GetClosest(screenCapture);
                                if (item.HasValue)
                                {
                                    if (item.Value.X > 0 && item.Value.Y > 0)
                                    {
                                        Point position = calculateFromCenter(item.Value.X, item.Value.Y);
                                        // correct mouse down
                                        int correction = 0;
                                        if (item.Value.Y > Recalc(383, false) && item.Value.Y < Recalc(435, false))
                                        {
                                            correction = Recalc(80, false);
                                        }

                                        VirtualMouse.MoveTo(position.X+100, position.Y + correction, 10);
                                    }
                                }
                                else
                                {
                                    // Not found Swirl around with Mouse
                                    VirtualMouse.MoveTo(Between(Recalc(460), Recalc(1460)),
                                        Between(Recalc(120, false), Recalc(900, false)), 10);
                                }
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
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        private async Task Floorfight(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                while (_floorFight && _stopp == false)
                {
                    try
                    {
                        if (!_doUltimateAttack)
                        {
                            foreach (KeyValuePair<byte, int> skill in _skills.skillset.OrderBy(x => x.Value))
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                if (_floorFight && !_stopp)
                                {
                                    if (chBoxCooldownDetection.Checked)
                                    {
                                        if (!isKeyOnCooldownGray(skill.Key))
                                        {
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                            KeyboardWrapper.AlternateHoldKey(skill.Key, CasttimeByKey(skill.Key));

                                            if (IsDoubleKey(skill.Key))
                                            {
                                                KeyboardWrapper.PressKey(skill.Key);
                                            }

                                            SetKeyCooldownGray(skill.Key); // Set Cooldown

                                            await Task.Delay(50, token);
                                            _walktopUTurn++;
                                        }
                                        else
                                        {
                                            if (int.Parse(textBoxAutoAttack.Text) >= 1 && _q && _w && _e && _r && _a && _s && _d && _f)
                                            {
                                                lbStatus.Invoke(
                                                    (MethodInvoker)(() => lbStatus.Text = "Bot is autoattacking..."));
                                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_C,
                                                    int.Parse(textBoxAutoAttack.Text));
                                                _walktopUTurn++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!isKeyOnCooldown(skill.Key))
                                        {
                                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Bot is fighting..."));
                                            KeyboardWrapper.AlternateHoldKey(skill.Key, CasttimeByKey(skill.Key));

                                            if (IsDoubleKey(skill.Key))
                                            {
                                                KeyboardWrapper.PressKey(skill.Key);
                                            }

                                            SetKeyCooldown(skill.Key); // Set Cooldown
                                            await Task.Run(() => SkillCooldown(token, skill.Key));
                                            await Task.Delay(50, token);
                                            _walktopUTurn++;
                                        }
                                        else
                                        {
                                            if (int.Parse(textBoxAutoAttack.Text) >= 1 && _q && _w && _e && _r && _a && _s && _d && _f)
                                            {
                                                lbStatus.Invoke(
                                                    (MethodInvoker)(() => lbStatus.Text = "Bot is autoattacking..."));
                                                KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_C,int.Parse(textBoxAutoAttack.Text));
                                                _walktopUTurn++;
                                            }
                                        }
                                    }
                                }



                                if (_walktopUTurn == 4 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(240, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(_currentMouseButton, 1500);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(566, false), 10);
                                    KeyboardWrapper.PressKey(_currentMouseButton);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 11 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    VirtualMouse.MoveTo(Recalc(523), Recalc(840, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(_currentMouseButton, 2200);
                                    VirtualMouse.MoveTo(Recalc(1007), Recalc(494, false), 10);
                                    KeyboardWrapper.PressKey(_currentMouseButton);
                                    await Task.Delay(1, token);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 18 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);

                                    VirtualMouse.MoveTo(Recalc(1578), Recalc(524, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(_currentMouseButton, 2000);
                                    VirtualMouse.MoveTo(Recalc(905), Recalc(531, false), 10);
                                    KeyboardWrapper.PressKey(_currentMouseButton);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 24 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);

                                    VirtualMouse.MoveTo(Recalc(523), Recalc(850, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(_currentMouseButton, 2200);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(500, false), 10);
                                    KeyboardWrapper.PressKey(_currentMouseButton);
                                    await Task.Delay(1, token);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 24 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _walktopUTurn = 1;
                                    await Task.Delay(1, token);
                                }

                                //         await Task.Delay(humanizer.Next(10, 40), token);
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


       






     
        private async Task Portaldetect(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                while (_portaldetect && _stopp == false)
                {
                    try
                    {
                        Process[] processName = Process.GetProcessesByName("LostArk");
                        if (processName.Length == 0 && chBoxCrashDetection.Checked)
                        {
                            GameCrashed();
                            return;
                        }
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);
                       object fight = Pixel.PixelSearch(Recalc(114), Recalc(208, false), Recalc(168), Recalc(220, false), 
                            0xDBC7AC, 7);
                        if (fight.ToString() != "0" && _stopp == false)
                        {
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Portal detected!"));

                            if (chBoxActivateF2.Checked && _stopp == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                _potions = false;
                                _revive = false;
                                _ultimate = false;
                                _floor1 = false;
                                _floorFight = false;

                                _stopp = false;
                               

                                _leavetimer1++;
                                if (_leavetimer1 == 1 && chBoxUnstuckF1.Checked)
                                {
                                    var t6 = Task.Run(() => Leavetimerfloor1(token));
                                    await Task.WhenAny(t6);
                                }

                                _portalIsDetected = true;
                                _portalIsNotDetected = false;
                                var t5 = Task.Run(() => PORTALISDETECTED(token));
                                var t7 = Task.Run(() => SEARCHPORTAL(token));
                                await Task.WhenAny(t5, t7);
                            }
                            else if (!chBoxActivateF2.Checked && _stopp == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);

                                lbStatus.Invoke(
                                    (MethodInvoker) (() => lbStatus.Text = "Floor 1 Complete..."));

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
                                _floor3 = false;

                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                var leave = Task.Run(() => Leavedungeon(token));
                                await Task.WhenAny(leave);
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
    

        private async Task PORTALISDETECTED(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (_portalIsDetected)
                {
                    try
                    {
                        object health10 = Pixel.PixelSearch(Recalc(1898), Recalc(10, false), Recalc(1911),
                            Recalc(22, false), 0x000000,15);

                        if (health10.ToString() != "0")
                        {
                            _portalIsDetected = false;
                            token.ThrowIfCancellationRequested();
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Portalsearch done!"));
                        }

                        await Task.Delay(_humanizer.Next(10, 240) + 100, token);
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