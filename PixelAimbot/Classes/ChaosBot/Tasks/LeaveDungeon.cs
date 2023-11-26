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
            {   CtsBoss.Cancel();
            
                _stopp = true;
                starten = false;
                gefunden = false;
                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _potions = false;
                _floor1 = false;
                _floor2 = false;
                _floor3 = false;


                _gunlancer = false;
                _shadowhunter = false;
                _berserker = false;
                _paladin = false;
                _deathblade = false;
                _destroyer = false;
                _sharpshooter = false;
                _bard = false;
                _sorcerer = false;
                _soulfist = false;
         

                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay(_humanizer.Next(10, 240) + 500, token);
                // KLICKT AUF LEAVE BUTTON
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                VirtualMouse.MoveTo(Recalc(158), Recalc(285, false), 5);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);

                var t6 = Task.Run(() => Leaveaccept(token));
                await Task.WhenAny(new[] {t6});
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

        private async Task Leaveaccept(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                ChaosAllRounds++;

                _stopp = true;
                starten = false;
                gefunden = false;
                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _potions = false;
                _floor1 = false;
                _floor2 = false;
                _floor3 = false;

                _gunlancer = false;
                _shadowhunter = false;
                _berserker = false;
                _paladin = false;
                _deathblade = false;
                _sharpshooter = false;
                _bard = false;
                _sorcerer = false;
                _soulfist = false;

                // KLICKT ENTER
                await Task.Delay(_humanizer.Next(10, 240) + 1000);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                await Task.Delay(_humanizer.Next(10, 240) + 2000, token);

                CheckIfLoadScreen();

                if (_repairTimer <= DateTime.Now && chBoxAutoRepair.Checked)
                {
                    _repairTimer = DateTime.Now.AddMinutes(Convert.ToDouble(txtRepair.Text));
                   // await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    var t7 = Task.Run(() => Repair(token));
                    await Task.WhenAny(t7);
                }
                else if (_repairTimer <= DateTime.Now && chBoxNPCRepair.Checked)
                {
                    _repairTimer = DateTime.Now.AddMinutes(Convert.ToDouble(txtRepair.Text));
                  //  await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    var t7 = Task.Run(() => NPCRepair(token));
                    await Task.WhenAny(t7);
                }
                else if (_Logout <= DateTime.Now && chBoxLOGOUT.Checked)
                {
                  //  await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    var t11 = Task.Run(() => Logout(token));
                    await Task.WhenAny(t11);
                }
                else
                {
                    _swap++;

                  //  await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    var t9 = Task.Run(() => Restart(token));
                    await Task.WhenAny(t9);
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