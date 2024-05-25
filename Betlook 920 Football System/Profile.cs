using Betlook_920_Football_System;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BetLook_920_Football
{
    public partial class Profile : Form
    {
        string Username = Login_Form.Username;
        public Profile()
        {
            InitializeComponent();
        }

        private void Profile_Load(object sender, EventArgs e)
        {
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

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT UserRole FROM SignUp WHERE Username = '" + Username + "'", con);
            string userRole = cmd.ExecuteScalar().ToString();

            if (userRole == "User")
            {
                Hide();
                HomeSelection homeSelection = new HomeSelection();
                homeSelection.Show();
            }
            else if (userRole == "Admin")
            {
                Hide();
                AdminHomeSelection adminHomeSelection = new AdminHomeSelection();
                adminHomeSelection.Show();
            }
            con.Close();
        }
    }
}
