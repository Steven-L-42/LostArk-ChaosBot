using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace PixelAimbot.Classes.Misc
{
    public class Layout_Keyboard
    {
        public string LAYOUTS { get; set; }
        public VirtualKeyCode Q { get; set; }
        public VirtualKeyCode W { get; set; }
        public VirtualKeyCode E { get; set; }
        public VirtualKeyCode R { get; set; }
        public VirtualKeyCode A { get; set; }
        public VirtualKeyCode S { get; set; }
        public VirtualKeyCode D { get; set; }
        public VirtualKeyCode F { get; set; }
        public VirtualKeyCode Y { get; set; }
        public VirtualKeyCode Z { get; set; }

        public static void simulateHold(VirtualKeyCode key, int duration)
        {
            var sim = new InputSimulator();
            for (int t = 0; t < duration * 100; t++)
            {
                sim.Keyboard.KeyDown(key);
                Thread.Sleep(10);
            }

            sim.Keyboard.KeyUp(key);
        }
    }
}
