using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranGiaBao
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }
        private void Form9_Load(object sender, EventArgs e)
        {

        }

        private void ckDiscount_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDiscount.Checked == true)
                tbDiscount.Enabled = true;
            else
                tbDiscount.Enabled = false;
        }

        private void btRun_Click(object sender, EventArgs e)
        {
            string msg = null;
            int disc = 0;
            if (rbMale.Checked == true)
                msg += "Ông ";
            if (rbFemale.Checked == true)
                msg += "Bà ";
            if (ckDiscount.Checked == true)
                disc = 5;
            tbDiscount.Text = msg + tbName.Text + "được giảm "+ disc. ToString() + " % "+ "\r\n";
        }

        private void tbDiscount_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
