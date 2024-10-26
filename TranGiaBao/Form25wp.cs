using System;
using System.Drawing;
using System.Windows.Forms;

namespace TranGiaBao
{
    public partial class Form25wp : Form
    {
        PictureBox pb = new PictureBox();
        //Timer tmGame = new Timer();

        int xBall = 0, yBall = 0;
        int xDelta = 5, yDelta = 5;

        public Form25wp()
        {
            InitializeComponent();
        }

        private void Form25wp_Load(object sender, EventArgs e)
        {
            // Cài đặt Timer và bắt đầu
            tmGame.Interval = 10;
            tmGame.Tick += tmGame_Tick;
            tmGame.Start();

            // Cài đặt PictureBox (quả bóng)
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.Size = new Size(100, 100);
            pb.Location = new Point(xBall, yBall);
            pb.ImageLocation = @"d:\bong.png";  // Đường dẫn tới ảnh bóng

            // Thêm PictureBox vào Form
            this.Controls.Add(pb);
        }

        private void tmGame_Tick(object sender, EventArgs e)
        {
            // Cập nhật vị trí của quả bóng
            xBall += xDelta;
            yBall += yDelta;

            // Kiểm tra va chạm với biên của Form
            if (xBall > this.ClientSize.Width - pb.Width || xBall <= 0)
                xDelta = -xDelta;  // Đổi hướng theo trục X

            if (yBall > this.ClientSize.Height - pb.Height || yBall <= 0)
                yDelta = -yDelta;  // Đổi hướng theo trục Y

            // Cập nhật vị trí của PictureBox trên Form
            pb.Location = new Point(xBall, yBall);
        }
    }
}
