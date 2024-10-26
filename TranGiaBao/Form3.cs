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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private void btCong_Click(object sender, EventArgs e)
        {
            double soX = double.Parse(tbSoX.Text);
            double soY = double.Parse(tbSoY.Text);
            tbKetQua.Text = (soX + soY).ToString();
        }

        private void btNhan_Click(object sender, EventArgs e)
        {
            double soX = double.Parse(tbSoX.Text);
            double soY = double.Parse(tbSoY.Text);
            tbKetQua.Text = (soX * soY).ToString();
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
