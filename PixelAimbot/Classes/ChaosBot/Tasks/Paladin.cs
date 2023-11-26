using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        
      

        public async void PaladinTimer(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                
                while(!_paladin)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(_humanizer.Next(10, 240) + 50000, token);
                    _paladin = true;
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