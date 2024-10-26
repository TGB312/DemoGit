using System;
using System.Collections;
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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        //public ArrayList GetData()
        //{
        //    ArrayList lst = new ArrayList();

        //    Faculty f = new Faculty();
        //    f.Id = "K01";
        //    f.Name = "Công nghệ thông tin";
        //    f.Quantity = 1200;
        //    lst .Add(f);

        //    f = new Faculty();
        //    f.Id = "K02";
        //    f.Name = "Quản trị kinh doanh";
        //    f.Quantity = 4200;
        //    lst.Add(f);

        //    f = new Faculty();
        //    f.Id = "K02";
        //    f.Name = "Quản trị kinh doanh";
        //    f.Quantity = 5200;
        //    lst.Add(f);
        //    return lst;
        //}


        private void btOK_Click(object sender, EventArgs e)
        {
            string item = cb_Faculty.SelectedItem.ToString();
            tbDisplay.Text = "Bạn là sinh viên khoa :" + item ;
        }

        private void Form7_Load_1(object sender, EventArgs e)
        {   
            cb_Faculty.SelectedIndex = 2;
        }

        private void cb_Faculty_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int index = cb_Faculty.SelectedIndex;
            tbDisplay.Text = "Bạn đã chọn khoa thứ: " + index.ToString();
        }
    }
}
