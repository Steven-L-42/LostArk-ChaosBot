using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace PixelAimbot.Classes.Misc
{
    public class Pixel
    {
        public static Point PixelSearch(Rectangle rect, int ColorValue, int Shade_Variation)
        {   
            ArrayList points = new ArrayList();
            Bitmap RegionIn_Bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);

            Color Pixel_Color = Color.FromArgb(ColorValue);
            
            int xOffset = Screen.PrimaryScreen.Bounds.Left;
            int yOffset = Screen.PrimaryScreen.Bounds.Top;

            using (Graphics GFX = Graphics.FromImage(RegionIn_Bitmap))
            {
                GFX.CopyFromScreen(rect.X + xOffset, rect.Y + yOffset, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
            }

            BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(
                new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);
            int[] Formatted_Color = new int[3] {Pixel_Color.B, Pixel_Color.G, Pixel_Color.R}; //bgr

            unsafe
            {
                for (int y = 0; y < RegionIn_BitmapData.Height; y++)
                {
                    byte* row = (byte*) RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);
                    for (int x = 0; x < RegionIn_BitmapData.Width; x++)
                    {
                        if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) &
                            row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
                            if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) &
                                row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
                                if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) &
                                    row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
                                    return new Point(x + rect.X, y + rect.Y);
                    }
                }
            }

            RegionIn_Bitmap.Dispose();
            return Point.Empty;
        }
    }
}