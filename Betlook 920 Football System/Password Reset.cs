using BetLook_920_Football;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Betlook_920_Football_System
{
    public partial class Change_Password : Form
    {
        public Change_Password()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555");
            if (ChangePasswordTxtBox.Text == "" || ConfirmPassWordTxtBox.Text == "")
            {
                MessageBox.Show("Enter and Confirm New Password", "Enter and Confirm New Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (ConfirmPassWordTxtBox.Text != ChangePasswordTxtBox.Text)
            {
                MessageBox.Show("Password1 and Password2 do not match", "Password1 and Password2 do not match", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "update SignUp set Password=" + ChangePasswordTxtBox.Text + " where Username= '" + UserTextBox.Text + "'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Password successfully updated", "Password successfully updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                    ChangePasswordTxtBox.Clear();
                    ConfirmPassWordTxtBox.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            Login_Form login = new Login_Form();
            login.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
