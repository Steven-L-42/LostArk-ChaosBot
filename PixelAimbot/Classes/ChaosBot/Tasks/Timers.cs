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


        private async Task Floor1Detectiontimer()
        {
            try
            {
                token.ThrowIfCancellationRequested();
                _Floor1Detectiontimer++;
                await Task.Delay(humanizer.Next(10, 240) + 180000, token);


                if (_portalIsNotDetected && _Floor1Detectiontimer == 1)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    _portalIsNotDetected = false;

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

                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, token);
                    cts = new CancellationTokenSource();
                    token = cts.Token;
                    var leave = Task.Run(() => Leavedungeon(token), token);
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

        public async void Leavetimerfloor1()
        {


            try
            {
                token.ThrowIfCancellationRequested();
                _Leavetimerfloor1++;
                await Task.Delay(humanizer.Next(10, 240) + 25000, tokenBossUndTimer);
                if (_portalIsDetected == true && _Leavetimerfloor1 == 1)
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
                    ChaosAllStucks++;
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Failed to Enter Portal..."));
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1, tokenBossUndTimer);
                    cts = new CancellationTokenSource();
                    token = cts.Token;
                    var t12 = Task.Run(() => Leavedungeon(token), token);
                  
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

        public async void Leavetimerfloor2()
        {

            try
            {
                if (chBoxLeavetimer.Checked && _stopp == false)
                {
                    tokenBossUndTimer.ThrowIfCancellationRequested();
                    await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txLeaveTimerFloor2.Text) * 1000), tokenBossUndTimer);

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
                    tokenBossUndTimer.ThrowIfCancellationRequested();
                    cts = new CancellationTokenSource();
                    token = cts.Token;
                    var t12 = Task.Run(() => Leavedungeon(token), token);
                }
                else
                {
                    tokenBossUndTimer.ThrowIfCancellationRequested();
                    await Task.Delay(humanizer.Next(10, 240) + 240 * 1000, tokenBossUndTimer);

                    if (_stopp == false)
                    {
   
                        _stopp = true;
                        _portalIsDetected = false;
                        starten = false;
                        gefunden = false;
                        _portalIsNotDetected = false;
                        _floorFight = false;
                        _searchboss = false;
                        _revive = false;
                        _ultimate = false;
                        _portaldetect = false;
                        _potions = false;
                        _floor1 = false;
                        _floor2 = false;
                        ChaosAllStucks++;

                        lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Failed to Detect Boss Kill..."));
                        tokenBossUndTimer.ThrowIfCancellationRequested();
                        await Task.Delay(1, tokenBossUndTimer);
                        cts = new CancellationTokenSource();
                        token = cts.Token;

                        var t12 = Task.Run(() => Leavedungeon(token), token);
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