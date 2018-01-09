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
            new Test1().Run();
            new Test1().Run();
            new Test2().Run();
            new Test3().Run();
            new Test4().Run();
        }
    }
}
