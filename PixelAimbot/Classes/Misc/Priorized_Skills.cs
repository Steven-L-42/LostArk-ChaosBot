using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;


namespace PixelAimbot.Classes.Misc
{
  
    internal class Priorized_Skills
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        public Key A { get; set; }
        public Key B { get; set; }
        public Key C { get; set; }
        public Key D { get; set; }
        public Key E { get; set; }
        public Key F { get; set; }
        public Key G { get; set; }
        public Key H { get; set; }
        
        public async void Priorized(Key Key, ChaosBot ChaosBot)
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
        public enum Key
        {
            A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7, H = 8
        }

        internal void Priorized(System.Windows.Input.Key a)
        {
            throw new NotImplementedException();
        }
    }

}
