using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        public async void LOGOUTTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay((int.Parse(txtLOGOUT.Text) * 1000) * 60, token);
                _logout = true;
            }
            catch(Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

        public async void BUFFTIMER(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1, token);
                await Task.Delay(15 * 1000 * 60, token); // 15 Minutes
                _buff = true;
            }
            catch(Exception ex)
            {
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
            }
        }

    }
}