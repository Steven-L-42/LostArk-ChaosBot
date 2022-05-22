using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        private async Task Leavedungeon()
        {
            try
            {
                ctsSearchBoss.Cancel();
                ctsSearchBoss.Dispose();
                ctsSearchBoss = new CancellationTokenSource();
                tokenSearchBoss = ctsSearchBoss.Token;

                ctsSkills.Cancel();
                ctsSkills.Dispose();
                ctsSkills = new CancellationTokenSource();
                tokenSkills = ctsSkills.Token;

                ctsDetections.Cancel();
                ctsDetections.Dispose();
                ctsDetections = new CancellationTokenSource();
                tokenDetections = ctsDetections.Token;

                ctsBossUndTimer.Cancel();
                ctsBossUndTimer.Dispose();
                ctsBossUndTimer = new CancellationTokenSource();
                tokenBossUndTimer = ctsBossUndTimer.Token;

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
                await Task.Delay(humanizer.Next(10, 240) + 500, token);
                // KLICKT AUF LEAVE BUTTON
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
        
             

                var t6 = Task.Run(() => Leaveaccept());
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

        private async Task Leaveaccept()
        {
            try
            {
                token.ThrowIfCancellationRequested();

                ChaosAllRounds++;
                // KLICKT ENTER
  
                await Task.Delay(humanizer.Next(10, 240) + 1000, token);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
                token.ThrowIfCancellationRequested();
                if (_repairTimer <= DateTime.Now && chBoxAutoRepair.Checked || _repairTimer <= DateTime.Now && chBoxNPCRepair.Checked)
                {
                    _repairTimer = DateTime.Now.AddMinutes(Convert.ToDouble(txtRepair.Text));
                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    if(chBoxAutoRepair.Checked)
                    {
                     

                        var t7 = Task.Run(() => Repair(token));
                       
                        
                    }
                    else
                    {

                    

                        var t7 = Task.Run(() => NPCRepair(token));
                     
                    }
                 
                }
                else if (_Logout <= DateTime.Now && chBoxLOGOUT.Checked )
                {
                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                  
                  

                    var t11 = Task.Run(() => Logout(token),token);
                  
                }
                else
                {
                    _swap++;

                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                

                    var t9 = Task.Run(() => Restart(token));
                  
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