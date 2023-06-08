using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

namespace BetLook_920_Football
{
    public partial class SIgnUp : Form
    {

        public static string AccNumber;
        public SIgnUp()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Login_Form login = new Login_Form();
            login.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (UsernameTextBox.Text == "" || PasswordTextBox.Text == "" || FullnameTextBox.Text == "" || GenderComboBox.Text == "" || UserRoleComboBox.Text == "" || PhoneNumberTextbox.Text == "")
            {
                MessageBox.Show("Missing information", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (PhoneNumberTextbox.Text.Length > 10 && PhoneNumberTextbox.Text.Length < 10)
            {
                MessageBox.Show("Please enter a valid phone number", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else
            {
                try
                {
                    SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True");
                    connection.Open();
                    if (MessageBox.Show("Are you sure you want to submit this data, please confirm before submitting", "Important Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "INSERT INTO SignUp VALUES ('" + AccountNumTextBox.Text + "','" + UsernameTextBox.Text + "','" + FullnameTextBox.Text + "','" + GenderComboBox.SelectedItem.ToString() + "','" + PasswordTextBox.Text + "','" + PhoneNumberTextbox.Text + "','" + UserRoleComboBox.SelectedItem.ToString() + "')";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Data successfully recorded", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        connection.Close();
                        UsernameTextBox.Clear();
                        FullnameTextBox.Clear();
                        GenderComboBox.Text = "";
                        PasswordTextBox.Clear();
                        PhoneNumberTextbox.Clear();
                        UserRoleComboBox.Text = "";
                        Hide();
                        Login_Form login = new Login_Form();
                        login.Show();
                    }
                }

                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void SIgnUp_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            int length = 16;
            var rString = "";
            for (var i = 0; i < length; i++)
            {
                rString += (random.Next(1, 10)).ToString();
            }
            AccountNumTextBox.Text = rString;
            AccNumber = AccountNumTextBox.Text;
        }
    }
}
