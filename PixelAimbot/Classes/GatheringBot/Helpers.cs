using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PixelAimbot
{
    partial class GatheringBot
    {
        private static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public Stream ToStream(Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }


        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        public static int Recalc(int value, bool horizontal = true)
        {
            decimal oldResolution;
            decimal newResolution;
            if (horizontal)
            {
                oldResolution = 1920;
                newResolution = _screenWidth;
            }
            else
            {
                oldResolution = 1080;
                newResolution = _screenHeight;
            }


            decimal normalized = (decimal) value * newResolution;
            decimal rescaledPosition = (decimal) normalized / oldResolution;

            int returnValue = Decimal.ToInt32(rescaledPosition);
            return returnValue;
        }
        

        private void CheckIsDigit(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        public void UpdateArea(int x, int y, int width, int height)
        {
            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
            btnStart.Enabled = true;
            btnPause.Enabled = true;
        }

    }
}