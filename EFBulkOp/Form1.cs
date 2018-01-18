using System;
using System.Windows.Forms;

namespace EFBulkOp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void SetStatus(string s)
        {
            statusText.Text = s;
        }

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            new Test1().Run(10_000);
            new Test2().Run(10_000);
            new Test3().Run(10_000);
            new Test4().Run(10_000);
            new Test5().Run(10_000);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Test5().Run(10_000);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Test5().Run(100_000);
        }
    }
}
