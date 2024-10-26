using System;
using System.Windows.Forms;

namespace TranGiaBao
{
    public partial class Form5 : Form
    {
        string opr = "";
        decimal workingMemory = 0;

        public Form5()
        {
            InitializeComponent();
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            tbDisplay.Text += btn.Text;
        }

        private void btEquals_Click(object sender, EventArgs e)
        {
            decimal secondValue;

            if (decimal.TryParse(tbDisplay.Text, out secondValue))
            {
                switch (opr)
                {
                    case "+":
                        tbDisplay.Text = (workingMemory + secondValue).ToString();
                        break;
                    case "*":
                        tbDisplay.Text = (workingMemory * secondValue).ToString();
                        break;
                }
                opr = ""; // Clear operation after calculation
                workingMemory = 0; // Reset working memory
            }
        }

        private void btPlus_Click(object sender, EventArgs e)
        {
            SetOperation("+");
        }

        private void btMul_Click(object sender, EventArgs e)
        {
            SetOperation("*");
        }

        private void SetOperation(string operation)
        {
            if (decimal.TryParse(tbDisplay.Text, out workingMemory))
            {
                opr = operation;
                tbDisplay.Clear();
            }
        }
    }
}
