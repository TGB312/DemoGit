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
    public partial class Form24wp : Form
    {
        PictureBox pb = new PictureBox();
        int y = 0;
        int x = 0;
        public Form24wp()
        {
            InitializeComponent();
        }

        private void btLeft_Click_1(object sender, EventArgs e)
        {
            x -= 10;
            pb.Location = new Point(x, y);
        }

        private void btRight_Click_1(object sender, EventArgs e)
        {
            x += 10;
            pb.Location = new Point(x, y);
        }

        private void btFile_Click_1(object sender, EventArgs e)
        {
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Size = new Size(100, 100);
            pb.Location = new Point(x, y);
            this.Controls.Add(pb);
            pb.ImageLocation = @"d:\bong.png";
        }
    }
}
