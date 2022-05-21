using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private async Task Leavedungeon(CancellationToken token)
        {
            try
            {
                starten = false;
                gefunden = false;
                _stopp = true;
                _portalIsDetected = false;
                _Leavetimerfloor1 = 0;
                _Leavetimerfloor2 = 0;
                _GlobalLeavetimerfloor2 = 0;
                _Floor1Detectiontimer = 0;
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
                _Glavier = false;
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
                await Task.Delay(humanizer.Next(10, 240) + 500, token);
                // KLICKT AUF LEAVE BUTTON
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                if (!token.IsCancellationRequested)
                {
                    var t6 = Task.Run(() => Leaveaccept(token));
                    await Task.WhenAny(new[] { t6 });
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

        // ITS A DIFFERENT CLICK LOCATION BECAUSE OF SMALLER LEAVE BUTTON FOR FLOOR 3
        //private async Task Leavedungeoncomplete(CancellationToken token)
        //{
        //    try
        //    {
        //        token.ThrowIfCancellationRequested();
        //        await Task.Delay(1, token);
        //        object walk = Pixel.PixelSearch(Recalc(141), Recalc(274, false), Recalc(245), Recalc(294, false),
        //            0x29343F, 10);

        //        if (walk.ToString() != "0")
        //        {
        //            object[] walkCoord = (object[]) walk;
        //            VirtualMouse.MoveTo((int) walkCoord[0], (int) walkCoord[1], 5);
        //            KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
        //        }

        //        var t6 = Task.Run(() => Leaveaccept(token));
        //        await Task.WhenAny(new[] {t6});
        //    }
        //    catch (AggregateException)
        //    {
        //        Debug.WriteLine("Expected");
        //    }
        //    catch (ObjectDisposedException)
        //    {
        //        Debug.WriteLine("Bug");
        //    }
        //    catch (Exception ex)
        //    {
        //        int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
        //        Debug.WriteLine("[" + line + "]" + ex.Message);
        //    }
        //}

        private async Task Leaveaccept(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);


                starten = false;
                gefunden = false;
                _stopp = true;
                _portalIsDetected = false;
                _Leavetimerfloor1 = 0;
                _Leavetimerfloor2 = 0;
                _GlobalLeavetimerfloor2 = 0;
                _Floor1Detectiontimer = 0;
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
                ChaosAllRounds++;
                // KLICKT ENTER
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                token.ThrowIfCancellationRequested();
                if (_repairTimer <= DateTime.Now && chBoxAutoRepair.Checked && !token.IsCancellationRequested || _repairTimer <= DateTime.Now && chBoxNPCRepair.Checked && !token.IsCancellationRequested)
                {
                    _repairTimer = DateTime.Now.AddMinutes(Convert.ToDouble(txtRepair.Text));
                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    if(chBoxAutoRepair.Checked)
                    {
                        token.ThrowIfCancellationRequested();
                        if (!token.IsCancellationRequested)
                        {
                            var t7 = Task.Run(() => Repair(token));
                            await Task.WhenAny(t7);
                        }
                    }
                    else
                    {
                        token.ThrowIfCancellationRequested();
                        if (!token.IsCancellationRequested)
                        {
                            var t7 = Task.Run(() => NPCRepair(token));
                            await Task.WhenAny(t7);
                        }
                    }
                 
                }
                else if (_Logout <= DateTime.Now && chBoxLOGOUT.Checked && !token.IsCancellationRequested)
                {
                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    token.ThrowIfCancellationRequested();
                    if (!token.IsCancellationRequested)
                    {
                        var t11 = Task.Run(() => Logout(token));
                        await Task.WhenAny(t11);
                    }
                }
                else if (_logout == false && _repairTimer <= DateTime.Now == false && !token.IsCancellationRequested)
                {
                    _swap++;

                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    token.ThrowIfCancellationRequested();
                    if (!token.IsCancellationRequested)
                    {
                        var t9 = Task.Run(() => Restart(token));
                        await Task.WhenAny(t9);
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
}