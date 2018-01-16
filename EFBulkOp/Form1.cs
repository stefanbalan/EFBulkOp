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

        private void button1_Click(object sender, EventArgs e)
        {
            new Test1().Run(1000);
            new Test2().Run(1000);
            new Test3().Run(1000);
            new Test4().Run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Test5().Run(10_000);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Test6().Run(10_000);
        }
    }
}
