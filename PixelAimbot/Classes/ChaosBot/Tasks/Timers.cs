using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using PixelAimbot.Classes.OpenCV;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        public async void Repairtimer()
        {
            try
            {
                await Task.Delay(humanizer.Next(10, 240) + (int.Parse(txtRepair.Text) * 1000) * 60);
                for(int i = 0; i < 1; i++)
                { _repair = true; }
                
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
                await Task.Delay(humanizer.Next(10, 240) + 25000, token);
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
                    lbStatus.Invoke((MethodInvoker)(() => lbStatus.Text = "Failed to Enter Portal!"));
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
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txLeaveTimerFloor2.Text) * 1000, token);

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

                var t12 = Task.Run(() => Leavedungeon(token));
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
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        public async void Leavetimerfloor3(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + int.Parse(txLeaveTimerFloor3.Text) * 1000, token);
                _stopp = true;
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
                var t12 = Task.Run(() => Leavedungeoncomplete(token));
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
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}