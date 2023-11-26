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
            TopMost = true;
            this.Size = new Size(ChaosBot.Recalc(Size.Width), ChaosBot.Recalc(Size.Height, false));

            splitContainer2.Size = new Size(ChaosBot.Recalc(splitContainer2.Size.Width), ChaosBot.Recalc(splitContainer2.Size.Height, false));
            splitContainer2.SplitterDistance = splitContainer2.Size.Width / 2;

            picChaosStart.MaximumSize = picChaosStart.MinimumSize = picChaosStart.Size = new Size(ChaosBot.Recalc(544), ChaosBot.Recalc(640, false));

            picChaosEnd.MaximumSize = picChaosEnd.MinimumSize = picChaosEnd.Size = new Size(ChaosBot.Recalc(544), ChaosBot.Recalc(640, false));

            picChaosStart.Image = ChaosBot.StartInventar;
            picChaosEnd.Image = ChaosBot.EndInventar;

            long Full = ChaosBot.ChaosTime.Ticks + 10000000;

            DateTime ChaosTimePlus1 = new DateTime(Full);
         //   ChaosBot.ChaosRedStages = ChaosBot.ChaosAllRounds - ChaosBot.ChaosAllStucks;
            this.lbChaosRunStart.Text = ChaosBot.ChaosStart.ToString("HH:mm:ss");
            this.lbChaosRunStop.Text = ChaosBot.ChaosStop.ToString("HH:mm:ss");
            this.lbChaosRunTime.Text = ChaosTimePlus1.ToString("HH:mm:ss");
            this.lbChaosAllRounds.Text = ChaosBot.ChaosAllRounds.ToString();
            this.lbChaosAllStucks.Text = ChaosBot.ChaosAllStucks.ToString();
            this.lbChaosRedStages.Text = ChaosBot.ChaosRedStages.ToString();
            this.lbChaosGameCrashed.Text = ChaosBot.ChaosGameCrashed.ToString();

        }

        public static Rectangle bounds;
        private void button1_Click_1(object sender, EventArgs e)
        {
            TopMost = false;

            bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width - 20, bounds.Height - 50))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left + 11, bounds.Top + 40), Point.Empty, bounds.Size);
                }
                bitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "/SymbioticInv_" + DateTime.Now.ToString("HH.mm-[dd.MM.yyyy]") + ".jpg", ImageFormat.Jpeg);

                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory);

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Text = DateTime.Now.ToString("HH:mm:ss");
        }


    }
}
