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

        public List<KeyValuePair<VirtualKeyCode, int>> skillset { get; set; } = new Dictionary<VirtualKeyCode, int>()
        {            
            {VirtualKeyCode.VK_A, 1},
            {VirtualKeyCode.VK_S, 2},
            {VirtualKeyCode.VK_D, 3},
            {VirtualKeyCode.VK_F, 4},
            {VirtualKeyCode.VK_Q, 5},
            {VirtualKeyCode.VK_W, 6},
            {VirtualKeyCode.VK_E, 7},
            {VirtualKeyCode.VK_R, 8},
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
