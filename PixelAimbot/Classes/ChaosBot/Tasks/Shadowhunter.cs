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
                await Task.Delay(28000, token);
                _Q = false;
                _W = false;
                _E = false;
                _R = false;
                _A = false;
                _S = false;
                _D = false;
                _F = false;
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