using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;


namespace PixelAimbot
{
    public partial class ChaosRunTimed : Form
    {

        public ChaosRunTimed()
        {
            InitializeComponent();
        }

        private void ChaosRunTimed_Load(object sender, EventArgs e)
        {

            picChaosStart.ImageLocation = Application.UserAppDataPath + "/ChaosStartInv.jpg";
            picChaosEnd.ImageLocation = Application.UserAppDataPath + "/ChaosEndInv.jpg";

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width-20, bounds.Height-50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left+11, bounds.Top+40), Point.Empty, bounds.Size);
                }
                bitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "/SymbioticInv_" + DateTime.Now.ToString("HH.mm-[dd.MM.yyyy]") + ".jpg", ImageFormat.Jpeg);
            }
        }
    }
}
