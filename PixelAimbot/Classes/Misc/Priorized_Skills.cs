using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;


namespace PixelAimbot.Classes.Misc
{
  
    internal class Priorized_Skills
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

        public List<KeyValuePair<byte, int>> skillset { get; set; } = new Dictionary<byte, int>()
        {            
            {KeyboardWrapper.VK_A, 1},
            {KeyboardWrapper.VK_S, 2},
            {KeyboardWrapper.VK_D, 3},
            {KeyboardWrapper.VK_F, 4},
            {KeyboardWrapper.VK_Q, 5},
            {KeyboardWrapper.VK_W, 6},
            {KeyboardWrapper.VK_E, 7},
            {KeyboardWrapper.VK_R, 8},
        }.ToList();


        

        public async void Priorized(int Key, ChaosBot ChaosBot)
        {
            try
            {
                cts = new CancellationTokenSource();
                var token = cts.Token;
                var t1 = Task.Run(() => Convert.ToString(Key), token);
                await Task.WhenAny(new[] { t1 });
            }
            catch (OperationCanceledException)
            {
                // Handle canceled
            }
            catch (Exception)
            {
                // Handle other exceptions
            }
            
        }

        internal void Priorized(System.Windows.Input.Key a)
        {
            throw new NotImplementedException();
        }
    }

}
