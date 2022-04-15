using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelAimbot
{
    public partial class frmMinimized : Form
    {
        public Stopwatch sw = new Stopwatch();
        public frmMinimized()
        {
            InitializeComponent();
            this.Text = frmLogin.RandomString(15);
        }

        private void timerRuntimer_Tick(object sender, EventArgs e)
        {
           
            labelRuntimer.Text = sw.Elapsed.Hours.ToString("D2") + ":" + sw.Elapsed.Minutes.ToString("D2") + ":" + sw.Elapsed.Seconds.ToString("D2") + " running";
            
        }

      
    }
}
