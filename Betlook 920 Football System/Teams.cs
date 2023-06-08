using Betlook_920_Football_System;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class Select_Teams : Form
    {
        string User = Login_Form.Username;
        private List<Image> images = new List<Image>();
        private int currentIndex = 0;
        private int currentTeamId = 0;
        public Select_Teams()
        {
            InitializeComponent();
            LoadImagesFromDatabase();
            NextButton_Click(null, null);
        }

        private void LoadImagesFromDatabase()
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=betlook;Integrated Security=True";
            string query = "SELECT clublogo FROM Teams";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            byte[] imageData = (byte[])reader["Clublogo"];
                            Image image = Image.FromStream(new MemoryStream(imageData));
                            images.Add(image);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            DisplayCurrentImage();
        }
        private void MoveToTeamId(int statid)
        {
            string query = "SELECT Tattack,Tdefence,Tover FROM Teamstats inner join teams on teamstats.Clubid=teams.Clubid WHERE Statid=@Statid";

            using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=betlook;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@statid", statid);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            int tattack = Convert.ToInt32(reader["Tattack"]);
                            int tdefence = Convert.ToInt32(reader["Tdefence"]);
                            int tover = Convert.ToInt32(reader["Tover"]);

                            label5.Text = tattack.ToString();
                            label6.Text = tdefence.ToString();
                            label7.Text = tover.ToString();
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }

        }
        private void DisplayCurrentImage()
        {
            if (images.Count > 0)
            {
                pictureBox1.Image = images[currentIndex];
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (images.Count > 0)
            {
                currentIndex++;
                if (currentIndex >= images.Count)
                {
                    currentIndex = 0;
                    currentTeamId = 0;
                }

                DisplayCurrentImage();
                currentTeamId++;
                MoveToTeamId(currentTeamId);
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (images.Count > 0)
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = images.Count - 1;
                }
                DisplayCurrentImage();

                currentTeamId--;
                if (currentTeamId < 1)
                {
                    currentTeamId = images.Count;
                }
                MoveToTeamId(currentTeamId);
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to change to a new team", "Important Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Hide();
                Choose_Teams choose_Teams = new Choose_Teams();
                choose_Teams.Show();
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT UserRole FROM SignUp WHERE Username = '" + User + "'", con);
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
