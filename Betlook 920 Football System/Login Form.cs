using Betlook_920_Football_System;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }


        public static string Username;
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int LoginAttempt = 0;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True";
                con.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("SELECT COUNT(*) FROM SignUp WHERE Username = '" + UserNameTextBox.Text + "' AND Password = '" + PasswordTextBox.Text + "'", con);
                DataTable dt = new DataTable();
                sqlData.Fill(dt);

                if (dt.Rows[0][0].ToString() == "1")
                {
                    Username = UserNameTextBox.Text;
                    MessageBox.Show("Login successful", "Login successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();

                    // Check user role and redirect accordingly
                    SqlCommand cmd = new SqlCommand("SELECT UserRole FROM SignUp WHERE Username = '" + UserNameTextBox.Text + "'", con);
                    string userRole = cmd.ExecuteScalar().ToString();

                    if (userRole == "User")
                    {
                        Choose_Teams choose_Teams = new Choose_Teams();
                        choose_Teams.Show();
                    }
                    else if (userRole == "Admin")
                    {
                        Choose_Teams choose_Teams = new Choose_Teams();
                        choose_Teams.Show();
                    }                 

                    con.Close();
                }
                else if (UserNameTextBox.Text == "" && PasswordTextBox.Text == "")
                {
                    MessageBox.Show("Please enter username and password", "Please enter username and password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LoginAttempt++;
                }
                else if (UserNameTextBox.Text == "")
                {
                    MessageBox.Show("Please enter username", "Please enter username", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LoginAttempt++;
                }
                else if (PasswordTextBox.Text == "")
                {
                    MessageBox.Show("Please enter password", "Please enter password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    LoginAttempt++;
                }
                else
                {
                    MessageBox.Show("Wrong username and password", "Wrong username and password, access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException excep) // handling errors that may occur
            {
                MessageBox.Show(excep.Message);
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide(); 
            SIgnUp sIgnUp = new SIgnUp();
            sIgnUp.Show();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                PasswordTextBox.PasswordChar = '\0';
            }
            else
            {
                PasswordTextBox.PasswordChar = '*';
            }
        }

        private void Label5_Click(object sender, EventArgs e)
        {
            Hide();
            Change_Password change_Password = new Change_Password();
            change_Password.Show();
        }
    }
}
