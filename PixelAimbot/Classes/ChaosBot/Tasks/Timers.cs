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




        public async void Leavetimerfloor1(CancellationToken tokenBossUndTimer)
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

                    cts.Cancel();
                    cts.Dispose();
                    cts = new CancellationTokenSource();
                    token = cts.Token;
                    ctsBossUndTimer.Cancel();
                    ctsBossUndTimer.Dispose();
                    ctsBossUndTimer = new CancellationTokenSource();
                    tokenBossUndTimer = ctsBossUndTimer.Token;

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

        public async void Leavetimerfloor2(CancellationToken tokenBossUndTimer)
        {

            try
            {
                tokenBossUndTimer.ThrowIfCancellationRequested();

                if (chBoxLeavetimer.Checked)
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
                    await Task.Delay(1, tokenBossUndTimer);

                    cts.Cancel();
                    cts.Dispose();
                    cts = new CancellationTokenSource();
                    token = cts.Token;
                    ctsBossUndTimer.Cancel();
                    ctsBossUndTimer.Dispose();
                    ctsBossUndTimer = new CancellationTokenSource();
                    tokenBossUndTimer = ctsBossUndTimer.Token;

                    var t12 = Task.Run(() => Leavedungeon(token), token);

                }
                else
                if (!chBoxLeavetimer.Checked)
                {
                    tokenBossUndTimer.ThrowIfCancellationRequested();



                    await Task.Delay(humanizer.Next(10, 240) + 240 * 1000, tokenBossUndTimer);
                    tokenBossUndTimer.ThrowIfCancellationRequested();
                    await Task.Delay(1, tokenBossUndTimer);
                    if (_stopp == false)
                    {
                        tokenBossUndTimer.ThrowIfCancellationRequested();
                        await Task.Delay(1, tokenBossUndTimer);
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

                        cts.Cancel();
                        cts.Dispose();
                        cts = new CancellationTokenSource();
                        token = cts.Token;
                        ctsBossUndTimer.Cancel();
                        ctsBossUndTimer.Dispose();
                        ctsBossUndTimer = new CancellationTokenSource();
                        tokenBossUndTimer = ctsBossUndTimer.Token;
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