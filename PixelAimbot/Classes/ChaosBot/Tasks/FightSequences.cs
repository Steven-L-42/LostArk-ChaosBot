using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.Misc;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private async Task Floortime(CancellationToken token)
        {
            try
            { 
                Process[] process1Name = Process.GetProcessesByName("LostArk");
                if (process1Name.Length == 0 && chBoxCrashDetection.Checked)
                {
                    ChaosGameCrashed++;

                    _stop = true;

                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_F10);
                    await Task.Delay(5000);
                    DiscordSendMessage("Game Crashed - Bot Stopped!");
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "GAME CRASHED - BOT STOPPED!"));
                    
                }
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                #region Floor1
                if (_floor1 && _stopp == false && !token.IsCancellationRequested)
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
                    _deathblade = true;
                    _Glavier = true;
                    _sharpshooter = true;
                    _sorcerer = true;
                    _soulfist = true;
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
                if (_floor2 && _stopp == false && !token.IsCancellationRequested)
                {
                    Process[] process2Name = Process.GetProcessesByName("LostArk");
                    if (process2Name.Length == 0 && chBoxCrashDetection.Checked)
                    {
                        ChaosGameCrashed++;

                        _stop = true;

                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_F10);
                        await Task.Delay(5000);
                        DiscordSendMessage("Game Crashed - Bot Stopped!");
                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "GAME CRASHED - BOT STOPPED!"));
                        
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

                    // PORTAL DETECT BOOL is on a lower place
                    // CLASSES //
                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
                    _paladin = true;
                    _sharpshooter = true;
                    _bard = true;
                    _sorcerer = true;
                    _soulfist = true;


                    if (_leavetimer == 1 && chBoxLeavetimer.Checked)
                    {
                        _leavetimer++;
                        var t36 = Task.Run(() => Leavetimerfloor2(token));
                    }
                    if (_leavetimer == 1 && !chBoxLeavetimer.Checked)
                    {
                        _leavetimer++;
                        _bossKillDetection = true; // Für BossKillDetection
                        starten = true; // Für BossKillDetection
                        var t36 = Task.Run(() => GlobalLeavetimerfloor2(token));
                    }
                   
                    var t18 = Task.Run(() => BossKillDetection(token));
                    var t11 = Task.Run(() => SearchNearEnemys(token));
                    var t12 = Task.Run(() => Floorfight(token));
                    var t16 = Task.Run(() => Revive(token));
                    var t20 = Task.Run(() => Potions(token));

                    await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txtDungeon2.Text) * 1000), token);

                  

                        _floorFight = false;
                        _potions = false;
                        _revive = false;
                        _searchboss = true;
                        var t13 = Task.Run(() => SEARCHBOSS(token));
                        await Task.WhenAny(t13);
                  

                    await Task.WhenAny(t11, t12, t16, t18, t20);
                }
                #endregion
                #region Floor3
                //if (_floor3 && _stopp == false)
                //{
                //    Process[] process3Name = Process.GetProcessesByName("LostArk");
                //    if (process3Name.Length == 0 && chBoxCrashDetection.Checked)
                //    {
                //                                     ChaosGameCrashed++;

                //            _stop = true;
                //
                //        KeyboardWrapper.PressKey(KeyboardWrapper.VK_F10);
                //        await Task.Delay(5000);
                //        DiscordSendMessage("Game Crashed - Bot Stopped!");
                //        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "GAME CRASHED - BOT STOPPED!"));
                //    }
                //    token.ThrowIfCancellationRequested();
                //    await Task.Delay(1, token);

                //    _leavetimer2++;
                //    _fightSequence2++;
                //    // START TASK BOOLS //
                //    _floorFight = true;
                //    _revive = true;
                //    _ultimate = true;
                //    _potions = true;
                //    // CLASSES //
                //    _gunlancer = true;
                //    _shadowhunter = true;
                //    _berserker = true;
                //    _paladin = true;
                //    _deathblade = true;
                //    _sharpshooter = true;
                //    _bard = true;
                //    _sorcerer = true;
                //    _soulfist = true;

                //    if (_leavetimer2 == 1)
                //    {
                //        var t36 = Task.Run(() => Leavetimerfloor3(token));
                //        await Task.WhenAny(t36);
                //    }

                //    var t11 = Task.Run(() => SearchNearEnemys(token));
                //    var t12 = Task.Run(() => Floorfight(token));
                //    var t16 = Task.Run(() => Revive(token));
                //    var t20 = Task.Run(() => Potions(token));
                //    await Task.Delay(humanizer.Next(10, 240) + int.Parse(txtDungeon3.Text) * 1000, token);

                //    _floorFight = false;

                //    if (_fightSequence2 < 7 && _stopp == false)
                //    {
                //        _floorFight = false;
                //        _potions = false;
                //        _revive = false;
                //        _searchboss = true;
                //        var t13 = Task.Run(() => SEARCHBOSS(token));
                //        await Task.WhenAny(t13);
                //    }

                //    await Task.WhenAny(t11, t12, t16, t20);
                //}
                #endregion
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
                ExceptionHandler.SendException(ex);
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
                var template = Image_red_hp;
                var detector = new ScreenDetector(template, null, 0.94f, ChaosBot.Recalc(460),
                    ChaosBot.Recalc(120, false), ChaosBot.Recalc(1000, true, true), ChaosBot.Recalc(780, false, true));
                detector.setMyPosition(new Point(ChaosBot.Recalc(500), ChaosBot.Recalc(390, false)));
                var screenPrinter = new PrintScreen();
 /*               if (currentMouseButton == KeyboardWrapper.VK_LBUTTON)
                {
                    VirtualMouse.LeftDown();
                }
                else
                {
                    VirtualMouse.RightDown();
                }*/
                while (_floorFight && _stopp == false && !token.IsCancellationRequested)
                {
                    if (_canSearchEnemys)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(1, token);
                            using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                            {
                                var item = detector.GetClosest(screenCapture, false);
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

                                        VirtualMouse.MoveTo(position.X, position.Y + correction, 10);
                                    }
                                }
                                else
                                {
                                    // Not found Swirl around with Mouse
                                    VirtualMouse.MoveTo(Between(Recalc(460), Recalc(1000)),
                                        Between(Recalc(120, false), Recalc(780, false)), 10);
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
            /*    if (currentMouseButton == KeyboardWrapper.VK_LBUTTON)
                {
                    VirtualMouse.LeftUp();
                }
                else
                {
                    VirtualMouse.RightUp();
                }*/
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

                while (_floorFight && _stopp == false && !token.IsCancellationRequested)
                {
                    try
                    {
                        if (!_doUltimateAttack && !token.IsCancellationRequested)
                        {
                            foreach (KeyValuePair<byte, int> skill in _skills.skillset.OrderBy(x => x.Value))
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                if (_floorFight && !_stopp && !token.IsCancellationRequested)
                                {
                                    
                                    if (!isKeyOnCooldownGray(skill.Key))
                                    {
                                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Bot is fighting..."));
                                        KeyboardWrapper.AlternateHoldKey(skill.Key, CasttimeByKey(skill.Key));

                                        if (IsDoubleKey(skill.Key))
                                        {
                                            KeyboardWrapper.PressKey(skill.Key);
                                        }

                                        SetKeyCooldownGray(skill.Key); // Set Cooldown
                                      //  var td = Task.Run(() => SkillCooldown(token, skill.Key));
                                        await Task.Delay(humanizer.Next(10, 40), token);
                                        _walktopUTurn++;
                                    }
                                    else
                                    {
                                        if (int.Parse(textBoxAutoAttack.Text) >= 1 && _Q && _W && _E && _R && _A && _S && _D && _F )
                                        { 
                                            lbStatus.Invoke(
                                                (MethodInvoker) (() => lbStatus.Text = "Bot is autoattacking..."));
                                            KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_C,
                                                int.Parse(textBoxAutoAttack.Text));
                                            _walktopUTurn++;
                                        }
                                    }
                                }


                                if (_walktopUTurn == 4 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(240, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 1500);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(566, false), 10);
                                    KeyboardWrapper.PressKey(currentMouseButton);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 11 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);
                                    VirtualMouse.MoveTo(Recalc(523), Recalc(840, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 2200);
                                    VirtualMouse.MoveTo(Recalc(1007), Recalc(494, false), 10);
                                    KeyboardWrapper.PressKey(currentMouseButton);
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
                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 2000);
                                    VirtualMouse.MoveTo(Recalc(905), Recalc(531, false), 10);
                                    KeyboardWrapper.PressKey(currentMouseButton);
                                    _canSearchEnemys = true;
                                    _walktopUTurn++;
                                }

                                if (_walktopUTurn == 24 && chBoxAutoMovement.Checked && _floor1 && _stopp == false)
                                {
                                    _canSearchEnemys = false;
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token);

                                    VirtualMouse.MoveTo(Recalc(523), Recalc(850, false), 10);
                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 2200);
                                    VirtualMouse.MoveTo(Recalc(960), Recalc(500, false), 10);
                                    KeyboardWrapper.PressKey(currentMouseButton);
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
                        Debug.WriteLine("Expected");
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.WriteLine("Bug");
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
                Debug.WriteLine("Expected");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Bug");
            }
            catch (Exception ex)
            {
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        private async Task Floor1Detectiontimer(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 180000, token);


                if (_portalIsNotDetected && !token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _portalIsNotDetected = false;

                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "ChaosDungeon Floor 1 Abort!"));

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
                    
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    var leave = Task.Run(() => Leavedungeon(token));
                    await Task.WhenAny(leave);
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

                while (_portaldetect && _stopp == false && !token.IsCancellationRequested)
                {
                    try
                    {
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

                          
                                if (_floorint2 == 2)
                                {
                                    _floor2 = false;
                                }

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
                        ExceptionHandler.SendException(ex);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
        public bool starten;
        public bool awakening;
        public bool gefunden;
        public bool _bossKillDetection;
        private async Task BossKillDetection(CancellationToken token)
        {
            if (!chBoxLeavetimer.Checked && !_stopp && _bossKillDetection && !token.IsCancellationRequested)
            {
                _bossKillDetection = false;
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    

                    while (starten && !_stopp && !token.IsCancellationRequested)
                    {

                        token.ThrowIfCancellationRequested();
                        await Task.Delay(humanizer.Next(10, 240) + 100, token);
                        float threshold = 0.8f;

                        var BossTemplate = Image_bossHP;
                        var BossMask = Image_bossHPmask;

                        Point myPosition = new Point(Recalc(148), Recalc(127, false));
                        Point screenResolution = new Point(screenWidth, screenHeight);

                        var BossDetector = new BossDetector(BossTemplate, BossMask, threshold);
                        var screenPrinter = new PrintScreen();

                        var rawScreen = screenPrinter.CaptureScreen();
                        Bitmap bitmapImage = new Bitmap(rawScreen);
                        using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                        {
                            var Boss = BossDetector.GetClosestEnemy(screenCapture, false);

                            if (Boss.HasValue && _stopp == false)
                            {
                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "BOSS FIGHT!"));
                                gefunden = false;

                                while (!gefunden && !_stopp && chBoxAwakening.Checked)
                                {
                                    token.ThrowIfCancellationRequested();
                                    await Task.Delay(1, token); 
                                    object Awakening = Pixel.PixelSearch(Recalc(1161), Recalc(66, false), Recalc(1187),
                                        Recalc(83, false), 0x9C1B16, 20);
                                    if (Awakening.ToString() == "0")
                                    {
                                        _doUltimateAttack = true;
                                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "AWAKENING..."));
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_V);
                                        await Task.Delay(500, token);
                                        KeyboardWrapper.PressKey(KeyboardWrapper.VK_V);
                                        await Task.Delay(500, token);
                                        _doUltimateAttack = false;
                                        gefunden = true;
                                    }
                                    Random random2 = new Random();
                                    var sleepTime2 = random2.Next(100, 150);
                                    Thread.Sleep(sleepTime2);
                                }
                                gefunden = true;

                            }
                            else if(!Boss.HasValue && gefunden == true && _stopp == false)
                            {
                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                await Task.Delay(humanizer.Next(10, 240) + 3000, token);

                                lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor2 Complete..."));
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
                                _Glavier = false;
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

                                token.ThrowIfCancellationRequested();
                                await Task.Delay(1, token);
                                var leave = Task.Run(() => Leavedungeon(token));
                                await Task.WhenAny(leave);
                            }

                        }

                        Random random = new Random();
                        var sleepTime = random.Next(100, 150);
                        Thread.Sleep(sleepTime);
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
                    ExceptionHandler.SendException(ex);
                    int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                    Debug.WriteLine("[" + line + "]" + ex.Message);
                }
            }
        }

        private async Task PORTALISDETECTED(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                while (_portalIsDetected == true && !token.IsCancellationRequested)
                {
                    try
                    {
                        object health10 = Pixel.PixelSearch(Recalc(1898), Recalc(10, false), Recalc(1911),
                            Recalc(22, false), 0x000000,15);

                        if (health10.ToString() != "0")
                        {
                            _portalIsDetected = false;
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Portalsearch done!"));
                        }

                        await Task.Delay(humanizer.Next(10, 240) + 100, token);
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
                        ExceptionHandler.SendException(ex);
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
     

        private async Task SEARCHPORTAL(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);

                _portaldetect = false;

                while (_portalIsDetected == true && !token.IsCancellationRequested)
                    //  for (int i = 0; i <= int.Parse(txtPortalSearch.Text); i++)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(humanizer.Next(10, 240) + 100, token);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                    var enemyTemplate = Image_portalenter1;
                    var enemyMask = Image_portalentermask1;
                    Point screenResolution = new Point(screenWidth, screenHeight);

                    // Main program loop
                    var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, 0.7f);
                    var screenPrinter = new PrintScreen();
                    
                    using (var screenCapture = new Bitmap(screenPrinter.CaptureScreen()).ToImage<Bgr, byte>())
                    {
                        var enemy = enemyDetector.GetClosestEnemy(screenCapture, false);
                        if (enemy.HasValue)
                        {
                            lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 1: Portal found..."));

                            CvInvoke.Rectangle(screenCapture,
                                new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                new MCvScalar(255));

                            double distance_x = (screenWidth - Recalc(296)) / 2;
                            double distance_y = (screenHeight - Recalc(255, false)) / 2;

                            var friend_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                            double multiplier = 1;
                            var friend_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                            var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                            var dist = Math.Sqrt(
                                Math.Pow((my_position_on_minimap.Item1 - friend_position_on_minimap.Item1), 2) +
                                Math.Pow((my_position_on_minimap.Item2 - friend_position_on_minimap.Item2), 2));

                            if (dist < 180)
                            {
                                multiplier = 1.2;
                            }

                            double posx;
                            double posy;
                            if (friend_position.Item1 < (screenWidth / 2))
                            {
                                posx = friend_position.Item1 * (2 - multiplier);
                            }
                            else
                            {
                                posx = friend_position.Item1 * multiplier;
                            }

                            if (friend_position.Item2 < (screenHeight / 2))
                            {
                                posy = friend_position.Item2 * (2 - multiplier);
                            }
                            else
                            {
                                posy = friend_position.Item2 * multiplier;
                            }

                            var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);

                            VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);
                            lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Floor 1: Enter Portal..."));

                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);

                            KeyboardWrapper.PressKey(currentMouseButton);

                            KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                        }
                    }


                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_G);
                    Random random = new Random();
                    var sleepTime = random.Next(500, 570);
                    await Task.Delay(sleepTime);
                }

                _searchSequence = 1;

                await Task.Delay(humanizer.Next(10, 240) + 8000, token);
                _searchboss = true;
                var t12 = Task.Run(() => SEARCHBOSS(token));
                await Task.WhenAny(new[] {t12});
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        private async Task SEARCHBOSS(CancellationToken token)
        {
            try
            {
              
                if (_searchboss == true && _floorFight == false && _stopp == false && !token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    if (_floorint2 == 1)
                    {
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 2: search enemy..."));
                    }

                    if (_floorint2 == 2)
                    {
                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 3: search enemy..."));
                    }

                    if (_searchSequence == 1)
                    {
                        await Task.Delay(humanizer.Next(10, 240) + 1500, token);
                        VirtualMouse.MoveTo(Recalc(960), Recalc(529, false), 10);
                        KeyboardWrapper.PressKey(currentMouseButton);

                        _searchSequence = 2;
                    }

                    if (chBoxGunlancer.Checked == true && _gunlancer == false && _searchboss == true)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(1, token);

                        KeyboardWrapper.AlternateHoldKey(UltimateKey(txBoxUltimateKey.Text), 1000);
                        _gunlancer = true;

                        lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Deactivate: Gunlancer Ultimate"));
                    }

                    for (int i = 0; i < int.Parse(txtDungeon2search.Text) && !token.IsCancellationRequested; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(humanizer.Next(10, 240) + 100, token);
                        float threshold = 0.705f;

                        var enemyTemplate = Image_enemy;
                        var enemyMask = Image_mask;
                        var BossTemplate = Image_boss1;
                        var BossMask = Image_bossmask1;
                        var mobTemplate = Image_mob1;
                        var mobMask =  Image_mobmask1;
                        var portalTemplate = Image_portalenter1;
                        var portalMask = Image_portalentermask1;

                        Point myPosition = new Point(Recalc(148), Recalc(127, false));
                        Point screenResolution = new Point(screenWidth, screenHeight);
                        var enemyDetector = new EnemyDetector(enemyTemplate, enemyMask, threshold);
                        var BossDetector = new EnemyDetector(BossTemplate, BossMask, threshold);
                        var mobDetector = new EnemyDetector(mobTemplate, mobMask, threshold);
                        var portalDetector = new EnemyDetector(portalTemplate, portalMask, threshold);
                        var screenPrinter = new PrintScreen();

                        var rawScreen = screenPrinter.CaptureScreen();
                        Bitmap bitmapImage = new Bitmap(rawScreen);
                        using (var screenCapture = bitmapImage.ToImage<Bgr, byte>())
                        {

                            var enemy = enemyDetector.GetClosestEnemy(screenCapture, false);
                            var Boss = BossDetector.GetClosestEnemy(screenCapture, false);
                            var mob = mobDetector.GetClosestEnemy(screenCapture, false);
                            var portal = portalDetector.GetClosestEnemy(screenCapture, false);

                            if (Boss.HasValue && _searchboss == true)
                            {
                                token.ThrowIfCancellationRequested();
                                CvInvoke.Rectangle(screenCapture,
                                    new Rectangle(new Point(Boss.Value.X, Boss.Value.Y), BossTemplate.Size),
                                    new MCvScalar(255));
                                double distance_x = (screenWidth - Recalc(296)) / 2;
                                double distance_y = (screenHeight - Recalc(255, false)) / 2;

                                var boss_position = ((Boss.Value.X + distance_x), (Boss.Value.Y + distance_y));
                                double multiplier = 1;
                                var boss_position_on_minimap = ((Boss.Value.X), (Boss.Value.Y));
                                var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                var dist = Math.Sqrt(
                                    Math.Pow((my_position_on_minimap.Item1 - boss_position_on_minimap.Item1), 2) +
                                    Math.Pow((my_position_on_minimap.Item2 - boss_position_on_minimap.Item2), 2));

                                if (dist < 180 && _searchboss == true)
                                {
                                    multiplier = 1.2;
                                }

                                double posx;
                                double posy;
                                if (boss_position.Item1 < (screenWidth / 2) && _searchboss == true)
                                {
                                    posx = boss_position.Item1 * (2 - multiplier);
                                }
                                else
                                {
                                    posx = boss_position.Item1 * multiplier;
                                }

                                if (boss_position.Item2 < (screenHeight / 2) && _searchboss == true)
                                {
                                    posy = boss_position.Item2 * (2 - multiplier);
                                }
                                else
                                {
                                    posy = boss_position.Item2 * multiplier;
                                }

                                var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);

                                if (_floorint2 == 1 && _searchboss == true)
                                {
                                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 2: Big-Boss found!"));
                                }

                                if (_floorint2 == 2 && _searchboss == true)
                                {
                                    lbStatus.Invoke((MethodInvoker) (() => lbStatus.Text = "Floor 3: Big-Boss found!"));
                                }

                                VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                                KeyboardWrapper.AlternateHoldKey(currentMouseButton, 1000);
                            }
                            else
                            {
                                if (enemy.HasValue && _searchboss == true)
                                {
                                    token.ThrowIfCancellationRequested();
                                    CvInvoke.Rectangle(screenCapture,
                                        new Rectangle(new Point(enemy.Value.X, enemy.Value.Y), enemyTemplate.Size),
                                        new MCvScalar(255));
                                    double distance_x = (screenWidth - Recalc(296)) / 2;
                                    double distance_y = (screenHeight - Recalc(255, false)) / 2;

                                    var enemy_position = ((enemy.Value.X + distance_x), (enemy.Value.Y + distance_y));
                                    double multiplier = 1;
                                    var enemy_position_on_minimap = ((enemy.Value.X), (enemy.Value.Y));
                                    var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                    var dist = Math.Sqrt(
                                        Math.Pow((my_position_on_minimap.Item1 - enemy_position_on_minimap.Item1), 2) +
                                        Math.Pow((my_position_on_minimap.Item2 - enemy_position_on_minimap.Item2), 2));

                                    if (dist < 180 && _searchboss == true)
                                    {
                                        multiplier = 1.2;
                                    }

                                    double posx;
                                    double posy;
                                    if (enemy_position.Item1 < (screenWidth / 2) && _searchboss == true)
                                    {
                                        posx = enemy_position.Item1 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posx = enemy_position.Item1 * multiplier;
                                    }

                                    if (enemy_position.Item2 < (screenHeight / 2) && _searchboss == true)
                                    {
                                        posy = enemy_position.Item2 * (2 - multiplier);
                                    }
                                    else
                                    {
                                        posy = enemy_position.Item2 * multiplier;
                                    }

                                    var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                    if (_floorint2 == 1 && _searchboss == true)
                                    {
                                        lbStatus.Invoke((MethodInvoker) (() =>
                                            lbStatus.Text = "Floor 2: Mid-Boss found!"));
                                    }

                                    if (_floorint2 == 2 && _searchboss == true)
                                    {
                                        lbStatus.Invoke((MethodInvoker) (() =>
                                            lbStatus.Text = "Floor 3: Mid-Boss found!"));
                                    }

                                    VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                                    KeyboardWrapper.AlternateHoldKey(currentMouseButton, 1000);
                                }
                                else
                                {
                                    if (mob.HasValue && _searchboss == true)
                                    {
                                        token.ThrowIfCancellationRequested();
                                        CvInvoke.Rectangle(screenCapture,
                                            new Rectangle(new Point(mob.Value.X, mob.Value.Y), mobTemplate.Size),
                                            new MCvScalar(255));
                                        double distance_x = (screenWidth - Recalc(296)) / 2;
                                        double distance_y = (screenHeight - Recalc(255, false)) / 2;

                                        var mob_position = ((mob.Value.X + distance_x), (mob.Value.Y + distance_y));
                                        double multiplier = 1;
                                        var mob_position_on_minimap = ((mob.Value.X), (mob.Value.Y));
                                        var my_position_on_minimap = ((Recalc(296) / 2), (Recalc(255, false) / 2));
                                        var dist = Math.Sqrt(
                                            Math.Pow((my_position_on_minimap.Item1 - mob_position_on_minimap.Item1),
                                                2) +
                                            Math.Pow((my_position_on_minimap.Item2 - mob_position_on_minimap.Item2),
                                                2));

                                        if (dist < 180 && _searchboss == true)
                                        {
                                            multiplier = 1.2;
                                        }

                                        double posx;
                                        double posy;
                                        if (mob_position.Item1 < (screenWidth / 2) && _searchboss == true)
                                        {
                                            posx = mob_position.Item1 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posx = mob_position.Item1 * multiplier;
                                        }

                                        if (mob_position.Item2 < (screenHeight / 2) && _searchboss == true)
                                        {
                                            posy = mob_position.Item2 * (2 - multiplier);
                                        }
                                        else
                                        {
                                            posy = mob_position.Item2 * multiplier;
                                        }

                                        var absolutePositions = PixelToAbsolute(posx, posy, screenResolution);
                                        if (_floorint2 == 1 && _searchboss == true)
                                        {
                                            lbStatus.Invoke(
                                                (MethodInvoker) (() => lbStatus.Text = "Floor 2: Mob found!"));
                                        }

                                        if (_floorint2 == 2 && _searchboss == true)
                                        {
                                            lbStatus.Invoke(
                                                (MethodInvoker) (() => lbStatus.Text = "Floor 3: Mob found!"));
                                        }

                                        VirtualMouse.MoveTo(absolutePositions.Item1, absolutePositions.Item2);

                                        KeyboardWrapper.AlternateHoldKey(currentMouseButton, 1000);
                                    }
                                }
                            }
                        }

                        Random random = new Random();
                        var sleepTime = random.Next(100, 150);
                        Thread.Sleep(sleepTime);
                    }
                    token.ThrowIfCancellationRequested();

                    if (_floorint2 == 1)
                    {
                        _floor1 = false;
                        _floor2 = true;
                    }

                    if (_floorint3 == 2)
                    {
                        _floor1 = false;
                        _floor2 = false;
                        
                    }

                    _searchboss = false;
                    _gunlancer = true;
                    _shadowhunter = true;
                    _berserker = true;
                    _paladin = true;
                    _sharpshooter = true;
                    _bard = true;
                    _sorcerer = true;
                    _soulfist = true;
                    _ultimate = true;
                    _floorFight = true;
                    token.ThrowIfCancellationRequested();
                    var t14 = Task.Run(() => UltimateAttack(token));
                    var t12 = Task.Run(() => Floortime(token));
                    await Task.WhenAny(new[] {t12,t14});
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
                ExceptionHandler.SendException(ex);
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}