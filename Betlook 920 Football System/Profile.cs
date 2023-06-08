using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class Profile : Form
    {
        public Profile()
        {
            InitializeComponent();
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            string Username = Login_Form.Username;
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT * FROM SignUp Where Username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", Username);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        UsernameTextBox.Text = reader["Username"].ToString();
                        FullNameTextBox.Text = reader["Fullname"].ToString();
                        PasswordTextBox.Text = reader["Password"].ToString();
                        PhoneNumberTextBox.Text = reader["PhoneNumber"].ToString();
                        radioButton1.Text = reader["Gender"].ToString();
                        radioButton1.Checked = true;
                        UserRoleTextBox.Text = reader["UserRole"].ToString();
                        
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Handle the exception (e.g., log the error message, display a user-friendly message)
            }
        }
    }
}
