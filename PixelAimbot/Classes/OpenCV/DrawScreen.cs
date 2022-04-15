using System.Runtime.InteropServices;
using System.Drawing;
using System;

public class DrawScreen
{
    [DllImport("User32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("User32.dll")]
    public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
    
    public void Draw(int x, int y, int width, int height)
    {
        IntPtr desktopPtr = GetDC(IntPtr.Zero);
        Graphics g = Graphics.FromHdc(desktopPtr);

        SolidBrush b = new SolidBrush(Color.Red);
        Pen p = new Pen(Color.Red, 2);
        //g.FillRectangle(b, new Rectangle(0, 0, 1920, 1080));
        g.DrawRectangle(p, new Rectangle(x,y, width, height));

        g.Dispose();
        ReleaseDC(IntPtr.Zero, desktopPtr);
    }
}