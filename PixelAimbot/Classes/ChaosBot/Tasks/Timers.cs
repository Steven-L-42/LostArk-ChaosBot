using System;
using System.Diagnostics;
using System.Drawing;
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


        private async Task Floor1Detectiontimer(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(_humanizer.Next(10, 240) + 180000, token);


                if (_portalIsNotDetected)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);


                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "ChaosDungeon Floor 1 Abort!"));

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

        public async void Leavetimerfloor1(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(_humanizer.Next(10, 240) + 25000, token);
                if (_portalIsDetected == true)
                {

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
                    ChaosAllStucks++;
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Failed to Enter Portal!"));
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    var t12 = Task.Run(() => Leavedungeon(token));
                    await Task.WhenAny(new[] { t12 });


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
        
        public async void Leavetimerfloor2(CancellationToken token)
        {
            try
            {
              
                if (_stopp == false)
                {
                    _bossKill.ThrowIfCancellationRequested();
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(_humanizer.Next(10, 240) + 240 * 1000, token);

                   
                    _bossKill.ThrowIfCancellationRequested();
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);

                    if(_floor3 == false)
                    {
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
                        ChaosAllStucks++;

                        _bossKill.ThrowIfCancellationRequested();
                        token.ThrowIfCancellationRequested();
                        var t12 = Task.Run(() => Leavedungeon(token));
                        await Task.WhenAny(new[] { t12 });
                    }
                    else
                    {
                        _bossKill.ThrowIfCancellationRequested();
                        token.ThrowIfCancellationRequested();

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
                        ChaosAllStucks++;

                        await Task.Delay(_humanizer.Next(10, 240) + 180 * 1000, token);
                        _bossKill.ThrowIfCancellationRequested();
                        token.ThrowIfCancellationRequested();
                        var t12 = Task.Run(() => Leavedungeon(token));
                        await Task.WhenAny(new[] { t12 });
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


        public async void DestroyerTimer(CancellationToken token)
        {
            try
            {
                if (cmbDestroyer.SelectedIndex == 1)
                {
                    var Destroyer = Task.Run(() => DestroyerAutoAttack(token), token);
                }
                token.ThrowIfCancellationRequested();
                await Task.Delay(_humanizer.Next(10, 240) + 30000, token);
               
                _destroyer = true;
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


        public async void Floor3BossTimer(CancellationToken BossKill)
        {
            try
            {

                BossKill.ThrowIfCancellationRequested();
                await Task.Delay(_humanizer.Next(10, 240) + 5000, BossKill);
                if (Floor3BossGesichtet == false)
                {
                    BossKill.ThrowIfCancellationRequested();
                    gefunden = true;
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

        public async void DestroyerAutoAttack(CancellationToken token)
        {
            try
            {
                await Task.Delay(_humanizer.Next(10, 240) + 1, token);
              
                lbStatus.Invoke(
                        (MethodInvoker)(() => lbStatus.Text = "Bot is autoattacking..."));
                    KeyboardWrapper.AlternateHoldKey(KeyboardWrapper.VK_C,10000);
                await Task.Delay(_humanizer.Next(10, 240) + 1, token);
                _doUltimateAttack = false;

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