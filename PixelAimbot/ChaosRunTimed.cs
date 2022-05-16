using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using PixelAimbot;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;


namespace PixelAimbot
{
    public partial class ChaosRunTimed : Form
    {
        //ChaosBot ChaosStart = new ChaosBot();
        //ChaosBot ChaosStop = new ChaosBot();


        public ChaosRunTimed()
        {
            InitializeComponent();
        }
        
        private void ChaosRunTimed_Load(object sender, EventArgs e)
        {
            
            long Full = ChaosBot.ChaosTime.Ticks+10000000;

            DateTime ChaosTimePlus1 = new DateTime(Full);
           
            this.lbChaosRunStart.Text = ChaosBot.ChaosStart.ToString("HH:mm:ss");
            this.lbChaosRunStop.Text = ChaosBot.ChaosStop.ToString("HH:mm:ss");
            this.lbChaosRunTime.Text = ChaosTimePlus1.ToString("HH:mm:ss");
            this.lbChaosRounds.Text = "54";


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

      

        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
