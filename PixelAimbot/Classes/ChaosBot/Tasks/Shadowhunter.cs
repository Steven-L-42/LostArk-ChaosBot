using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PixelAimbot.Classes.Misc;

namespace PixelAimbot
{
    partial class ChaosBot
    {
        public async void ShadowhunterSecond(CancellationToken token)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(30000, token);
                token.ThrowIfCancellationRequested();
                _q = false;
                _w = false;
                _e = false;
                _r = false;
                _a = false;
                _s = false;
                _d = false;
                _f = false;
                GetSkillQ();
                GetSkillW();
                GetSkillE();
                GetSkillR();
                GetSkillA();
                GetSkillS();
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
                int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                Debug.WriteLine("[" + line + "]" + ex.Message);
                ExceptionHandler.SendException(ex);
            }
        }




    }
}