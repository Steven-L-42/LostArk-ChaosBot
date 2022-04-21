using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using WindowsInput.Native;
using Point = System.Windows.Point;

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
        public const int MOUSEEVENTF_WHEEL = 2048;
        public const byte VK_LBUTTON = 0x01;
        public const byte VK_RBUTTON = 0x02;
        public const byte VK_SPACE = 0x20;
        public const byte VK_ESCAPE = 0x1B;
        public const byte VK_RETURN = 0x0D;
        public const byte VK_C = 0x43;
        public const byte VK_B = 0x42;

        public const byte VK_G = 0x47;
        public const byte VK_L = 0x4C;
        public const byte VK_Q = 0x51;
        public const byte VK_W = 0x57;
        public const byte VK_E = 0x45;
        public const byte VK_R = 0x52;
        public const byte VK_A = 0x41;
        public const byte VK_S = 0x53;
        public const byte VK_D = 0x44;
        public const byte VK_F = 0x46;
        public const byte VK_V = 0x56;
        public const byte VK_Y = 0x59;
        public const byte VK_Z = 0x5A;

        public const byte VK_F9 = 0x78;
        public const byte VK_F1 = 0x70;
        public const byte VK_1 = 0x31;
        public const byte VK_2 = 0x32;
        public const byte VK_3 = 0x33;
        public const byte VK_4 = 0x34;
        public const byte VK_5 = 0x35;
        public const byte VK_6 = 0x36;
        public const byte VK_7 = 0x37;
        public const byte VK_8 = 0x38;
        public const byte VK_9 = 0x39;

        public const byte VK_I = 0x49;
        public string LAYOUTS { get; set; }

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
        /// 
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


    public static class VirtualMouse
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        // import the necessary API function so .NET can
        // marshall parameters appropriately
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        // constants for the mouse_input() API function
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        // simulates movement of the mouse.  parameters specify changes
        // in relative position.  positive values indicate movement
        // right or down
        public static void Move(int xDelta, int yDelta)
        {
            mouse_event(MOUSEEVENTF_MOVE, xDelta, yDelta, 0, 0);
        }

        // simulates movement of the mouse.  parameters specify an
        // absolute location, with the top left corner being the
        // origin
        public static void MoveTo(int x, int y)
        {
            double absX = 65535.0 * (x + 1) / Screen.PrimaryScreen.Bounds.Width;
            double absY = 65535.0 * (y + 1) / Screen.PrimaryScreen.Bounds.Height;
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, (int) absX, (int) absY, 0, 0);
        }

        public static void Smoothing(int x, int y, int nSpeed = 0)
        {
            Point ptCur;
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            int xCur, yCur;
            int delta;
            const int nMinSpeed = 32;

            x = ((65535 * x) / (rect.Right - 1)) + 1;
            y = ((65535 * y) / (rect.Bottom - 1)) + 1;
            
            if (nSpeed == 0)
            {
                mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, x, y, 0, 0);
                Task.Delay(10).Wait(); 
                return;
            }
            
            if (nSpeed < 0 || nSpeed > 100)
                nSpeed = 10; // Default is speed 10
            
            ptCur = GetCursorPosition();
            xCur = (((int) ptCur.X * 65535) / (rect.Right - 1)) + 1;
            yCur = (((int) ptCur.Y * 65535) / (rect.Bottom - 1)) + 1;

            // Mouse Calculation magic fickt meinen kopf ... im out now
            while (xCur != x || yCur != y)
            {
                if (xCur < x)
                {
                    delta = (x - xCur) / nSpeed;
                    if (delta == 0 || delta < nMinSpeed)
                        delta = nMinSpeed;
                    if ((xCur + delta) > x)
                        xCur = x;
                    else
                        xCur += delta;
                }
                else if (xCur > x)
                {
                    delta = (xCur - x) / nSpeed;
                    if (delta == 0 || delta < nMinSpeed)
                        delta = nMinSpeed;
                    if ((xCur - delta) < x)
                        xCur = x;
                    else
                        xCur -= delta;
                }

                if (yCur < y)
                {
                    delta = (y - yCur) / nSpeed;
                    if (delta == 0 || delta < nMinSpeed)
                        delta = nMinSpeed;
                    if ((yCur + delta) > y)
                        yCur = y;
                    else
                        yCur += delta;
                }
                else if (yCur > y)
                {
                    delta = (yCur - y) / nSpeed;
                    if (delta == 0 || delta < nMinSpeed)
                        delta = nMinSpeed;
                    if ((yCur - delta) < y)
                        yCur = y;
                    else
                        yCur -= delta;
                }

                mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, xCur, yCur, 0, 0);

                Task.Delay(10).Wait();
            }
        }

        // simulates a click-and-release action of the left mouse
        // button at its current position
        public static void LeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }

        public static void RightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }
    }

    public static class AI
    {

        public static int
            PixelSearch(int Left, int Top, int Right, int Bottom, int color, int Shade_Variation = 0) 
        {
            //soon alla
        

            return 0;
        }

    }
}