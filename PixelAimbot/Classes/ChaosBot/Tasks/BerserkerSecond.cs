using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        
        public async void BerserkerSecond(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(humanizer.Next(10, 240) + 5000, token);
                KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(humanizer.Next(10, 240) + 31000, token);

                    KeyboardWrapper.PressKey(UltimateKey(txBoxUltimateKey.Text));
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
                ExceptionHandler.SendException(ex);
            }
        }
    }
}