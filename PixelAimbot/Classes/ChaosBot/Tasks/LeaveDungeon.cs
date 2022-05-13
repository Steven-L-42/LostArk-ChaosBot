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
                _stopp = true;
                _portalIsDetected = false;
              

                _portalIsNotDetected = false;
                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _portaldetect2 = false;
                _potions = false;
                _floor1 = false;
                _floor2 = false;
                _floor3 = false;

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

        // ITS A DIFFERENT CLICK LOCATION BECAUSE OF SMALLER LEAVE BUTTON FOR FLOOR 3
        private async Task Leavedungeoncomplete(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                object walk = Pixel.PixelSearch(Recalc(141), Recalc(274, false), Recalc(245), Recalc(294, false),
                    0x29343F, 10);

                if (walk.ToString() != "0")
                {
                    object[] walkCoord = (object[]) walk;
                    VirtualMouse.MoveTo((int) walkCoord[0], (int) walkCoord[1], 5);
                    KeyboardWrapper.PressKey(KeyboardWrapper.VK_LBUTTON);
                }

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
              

                _stopp = true;

                _floorFight = false;
                _searchboss = false;
                _revive = false;
                _ultimate = false;
                _portaldetect = false;
                _portaldetect2 = false;
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
                await Task.Delay(humanizer.Next(10, 240) + 1000);
                KeyboardWrapper.PressKey(KeyboardWrapper.VK_RETURN);

                await Task.Delay(humanizer.Next(10, 240) + 2000, token);
               
                if (_repairTimer <= DateTime.Now && chBoxAutoRepair.Checked)
                {
                    _repairTimer = DateTime.Now.AddMinutes(Convert.ToDouble(txtRepair.Text));
                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    var t7 = Task.Run(() => Repair(token));
                    await Task.WhenAny(t7);
                }
                else if (_Logout <= DateTime.Now && chBoxLOGOUT.Checked)
                {
                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
                    var t11 = Task.Run(() => Logout(token));
                    await Task.WhenAny(t11);
                }
                else if (_repair == false && _logout == false)
                {
                    _swap++;

                    await Task.Delay(humanizer.Next(10, 240) + 7000, token);
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