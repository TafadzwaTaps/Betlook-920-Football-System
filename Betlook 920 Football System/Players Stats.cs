using Betlook_920_Football_System;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class PlayersStats : Form
    {
        public PlayersStats()
        {
            InitializeComponent();
        }
 

        private void Players_Load(object sender, EventArgs e)
        {
            UsernameLbl.Text = Login_Form.Username;
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555"))
                {
                    connection.Open();

                    string query = "SELECT * FROM PlayerProfile";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        PlayerIdTextBox.Text = reader["PlayerID"].ToString();
                        PlayersFullNameTextBox.Text = reader["FirstName"].ToString() + reader["LastName"].ToString();
                        positionTextBox.Text = reader["Position"].ToString();
                        playerOvrRatingTextBox.Text = reader["PlayerOvrRating"].ToString();
                        clubTextBox.Text = reader["Club"].ToString();
                        ClubIdTextBox.Text = reader["ClubId"].ToString();
                        healthTextBox.Text = reader["Health"].ToString();
                        teamNumberTextBox.Text = reader["TeamNumber"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Handle the exception (e.g., log the error message, display a user-friendly message)
            } 

            using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555"))
            {
                connection.Open();

                string query = "SELECT * FROM PlayerPerformanceStats";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                PlayerStatsDataGridView.DataSource = new BindingSource(dt, null);
            }
        }

        private void IconButton12_Click(object sender, EventArgs e)
        {
            Hide();
            HomeSelection homeSelection = new HomeSelection();
            homeSelection.Show();
        }

        private void IconButton8_Click(object sender, EventArgs e)
        {
            Hide();
            PlayersStats playersStats = new PlayersStats();
            playersStats.Show();
        }

        private void IconButton9_Click(object sender, EventArgs e)
        {
            Hide();
            Add_Players add_Players = new Add_Players();
            add_Players.Show();
        }

        private void IconButton7_Click(object sender, EventArgs e)
        {
            Hide();
            Select_Teams select_Teams = new Select_Teams();
            select_Teams.Show();
        }

        private void IconButton11_Click(object sender, EventArgs e)
        {
            Hide();
            Play play = new Play();
            play.Show();
        }

        private void IconButton10_Click(object sender, EventArgs e)
        {
            Hide();
            Tournament tournament = new Tournament();
            tournament.Show();
        }

        private void IconButton6_Click(object sender, EventArgs e)
        {
            Hide();
            Wallet wallet = new Wallet();
            wallet.Show();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True");
            con.Open();
            if (SearchTextBox.Text == "")
            {
                MessageBox.Show("Please enter data to perform a search operation", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    string query = "SELECT * FROM PlayerProfile WHERE FirstName LIKE '%' + @SearchText + '%' OR LastName LIKE '%' + @SearchText + '%'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SearchText", SearchTextBox.Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Player found", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PlayerIdTextBox.Text = dr["PlayerID"].ToString();
                        PlayersFullNameTextBox.Text = dr["FirstName"].ToString() + dr["LastName"].ToString();
                        positionTextBox.Text = dr["Position"].ToString();
                        playerOvrRatingTextBox.Text = dr["PlayerOvrRating"].ToString();
                        clubTextBox.Text = dr["Club"].ToString();
                        ClubIdTextBox.Text = dr["Clubid"].ToString();
                        healthTextBox.Text = dr["Health"].ToString();
                        teamNumberTextBox.Text = dr["TeamNumber"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, the player " + SearchTextBox.Text + " is not available.", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        SearchTextBox.Text = "";
                        PlayerIdTextBox.Clear();
                        PlayersFullNameTextBox.Clear();
                        positionTextBox.Clear();
                        playerOvrRatingTextBox.Clear();
                        clubTextBox.Clear();
                        ClubIdTextBox.Clear();
                        healthTextBox.Clear();
                        clubTextBox.Clear();
                    }
                    dr.Close();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
