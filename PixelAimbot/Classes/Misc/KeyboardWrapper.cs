using System.Runtime.InteropServices;


namespace PixelAimbot.Classes.Misc
{
    public class KeyboardWrapper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        /// <summary>
        /// Key down flag
        /// </summary>
        private const int KEY_DOWN_EVENT = 0x0001;
        private const int KEY_UP_EVENT = 0x0002;

        private const int PauseBetweenStrokes = 50;

        public static byte VK_LBUTTON = 0x01;
        public static byte VK_RBUTTON = 0x02;
        public static byte VK_SPACE = 0x20;
        public static byte VK_ESCAPE = 0x1B;
        public static byte VK_RETURN = 0x0D;

        public static byte VK_G = 0x47;
        public static byte VK_Q = 0x51;
        public static byte VK_W = 0x57;
        public static byte VK_E = 0x45;
        public static byte VK_R = 0x52;
        public static byte VK_A = 0x41;
        public static byte VK_S = 0x53;
        public static byte VK_D = 0x44;
        public static byte VK_F = 0x46;
        public static byte VK_V = 0x56;
        public static byte VK_Y = 0x59;
        public static byte VK_Z = 0x5A;

        public static byte VK_F1 = 0x70;
        public static byte VK_1 = 0x31;
        public static byte VK_2 = 0x32;
        public static byte VK_3 = 0x33;
        public static byte VK_4 = 0x34;
        public static byte VK_5 = 0x35;
        public static byte VK_6 = 0x36;
        public static byte VK_7 = 0x37;
        public static byte VK_8 = 0x38;
        public static byte VK_9 = 0x39;

        /// <summary>
        /// Will hold a key down for a number of milliseconds
        /// </summary>
        /// <param name="key">byte value for key. can cast like this: (byte)System.Windows.Forms.Keys.F24</param>
        /// <param name="duration">ms to hold key down for</param>
        /// <example>
        /// <code>
        /// Keyboard.KeyUp((byte)Keys.F24,5000);
        /// </code>
        /// </example>
        public static void HoldKey(byte key, int duration)
        {
            int totalDuration = 0;
            while (totalDuration < duration)
            {
                keybd_event(key, 0, KEY_DOWN_EVENT, 0);
                keybd_event(key, 0, KEY_UP_EVENT, 0);
                System.Threading.Thread.Sleep(PauseBetweenStrokes);
                totalDuration += PauseBetweenStrokes;
            }
        }

        public static void AlternateHoldKey(byte key, int duration)
        {
            int totalDuration = 0;
            while (totalDuration < duration)
            {
                keybd_event(key, 0, KEY_DOWN_EVENT, 0);
                System.Threading.Thread.Sleep(PauseBetweenStrokes);
                totalDuration += PauseBetweenStrokes;
            }
            keybd_event(key, 0, KEY_UP_EVENT, 0);
        }
        /// <summary>
        /// Will press a key
        /// </summary>
        /// <param name="key">byte value for key. can cast like this: (byte)System.Windows.Forms.Keys.F24</param>
        /// <example>
        /// <code>
        /// Keyboard.PressKey((byte)Keys.F24);
        /// </code>
        /// </example>
        public static void PressKey(byte key)
        {
            keybd_event(key, 0, KEY_DOWN_EVENT, 0);
            keybd_event(key, 0, KEY_UP_EVENT, 0);
        }
        /// <summary>
        /// Will trigger the KeyUp event for a key. Easy way to keep the computer awake without sending any input.
        /// </summary>
        /// <param name="key">byte value for key. can cast like this: (byte)System.Windows.Forms.Keys.F24</param>
        /// <example>
        /// <code>
        /// Keyboard.KeyUp((byte)Keys.F24);
        /// </code>
        /// </example>
        public static void KeyUp(byte key)
        {
            keybd_event(key, 0, KEY_UP_EVENT, 0);
        }

        public static void KeyDown(byte key)
        {
            keybd_event(key, 0, KEY_DOWN_EVENT, 0);
        }
    }
}
