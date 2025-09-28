namespace Evaluator.Windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Btn0_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "0";
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "1";
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "2";
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "3";
        }

        private void Btn4_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "4";
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "5";
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "6";
        }

        private void Btn7_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "7";
        }

        private void Btn8_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "8";
        }

        private void Btn9_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "9";
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "(";
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += ")";
        }

        private void BtnMul_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "*";
        }

        private void BntDiv_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "/";
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "+";
        }

        private void BtnMin_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "-";
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (TexDisplay.Text.Length > 0)
            {
                TexDisplay.Text = TexDisplay.Text.Substring(0, TexDisplay.Text.Length - 1);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            TexDisplay.Text = "";
        }

        private void BtnPow_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += "^";
        }

        private void BtnDot_Click(object sender, EventArgs e)
        {
            TexDisplay.Text += ".";
        }

        private void BtnEqual_Click(object sender, EventArgs e)
        {
            try
            {
                var result = Evaluator.Core.ExpressionEvaluator.Evaluate(TexDisplay.Text);
                TexDisplay.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
