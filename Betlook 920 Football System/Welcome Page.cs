using System;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class Welcome_Page : Form
    {
        public Welcome_Page()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Login_Form login = new Login_Form();
            login.Show();
        }
    }
}
