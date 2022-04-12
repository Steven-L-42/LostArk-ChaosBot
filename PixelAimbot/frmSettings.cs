using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelAimbot
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmGuide Form = new frmGuide();
            Form.Show();
        }

        private void lbClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }
    }
}
