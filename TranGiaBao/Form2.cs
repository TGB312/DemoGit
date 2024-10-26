using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TranGiaBao
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void tbYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tbYear_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int year = int.Parse(tbYear.Text);

                // Validate year - must be less than or equal to 2000
                if (year > 2000)
                {
                    e.Cancel = true;
                    MessageBox.Show("Year must be less than or equal to 2000.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                e.Cancel = true;
                MessageBox.Show("Please enter a valid year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
