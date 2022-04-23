using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private async Task Start(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                Process[] processName = Process.GetProcessesByName("LostArk");
                if (processName.Length == 1)
                {
                    handle = processName[0].MainWindowHandle;
                    SetForegroundWindow(handle);
                }

                await Task.Delay(1500, token);
                _rodCounter = 0;

                var t12 = Task.Run(() => CheckGathering(token), token);
                await Task.Delay(1, token);
                var t14 = Task.Run(() => Repaircheck(token), token);
                var t1 = Task.Run(() => CheckEnergy(token), token);

                await Task.WhenAny(new[] {t1, t12, t14});
            }
            catch (Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }
    }
}