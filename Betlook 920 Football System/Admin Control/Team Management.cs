using Betlook_920_Football_System;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class Team_Management : Form
    {
        public Team_Management()
        {
            InitializeComponent();
        }


        private void AddButton_Click(object sender, EventArgs e)
        {
            byte[] imageLogo = ImageToByteArray(pictureBox2.Image);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True";
            if (ClubNameTextBox.Text == "" || WinsTextBox.Text == "" || MatchPlayedTextBox.Text == "" || LossesTextbox.Text == "" || GoalDifferenceTextBox.Text == "" || PointsTextbox.Text == "" || DrawsTextBox.Text == "")
            {
                MessageBox.Show("missing information", "Important text", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    con.Open();
                    if (MessageBox.Show("Are you sure you want to submit this data, please confirm before submitting", "Important Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "INSERT INTO Teams (Clublogo, Club, Matchplayed, Wins, Loss, Goaldifference, Points,Draws) VALUES (@ClubLogo, @Club, @MatchPlayed, @Wins, @Loss, @GoalDifference, @Points,@Draws)";
                        SqlCommand command = new SqlCommand(query, con);
                        command.Parameters.AddWithValue("@ClubLogo", imageLogo);
                        command.Parameters.AddWithValue("@Club", ClubNameTextBox.Text);
                        command.Parameters.AddWithValue("@MatchPlayed", MatchPlayedTextBox.Text);
                        command.Parameters.AddWithValue("@Wins", WinsTextBox.Text);
                        command.Parameters.AddWithValue("@Loss", LossesTextbox.Text);
                        command.Parameters.AddWithValue("@GoalDifference", GoalDifferenceTextBox.Text);
                        command.Parameters.AddWithValue("@Points", PointsTextbox.Text);
                        command.Parameters.AddWithValue("@Draws", DrawsTextBox.Text);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Data successfully recorded", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        con.Close();
                        ClubNameTextBox.Clear();
                        MatchPlayedTextBox.Clear();
                        WinsTextBox.Clear();
                        LossesTextbox.Clear();
                        GoalDifferenceTextBox.Clear();
                        PointsTextbox.Clear();
                        DrawsTextBox.Clear();
                        pictureBox2.Image = Betlook_920_Football_System.Properties.Resources.soccer_game_team_players_and_ball_league_recreational_sports_tournament_silhouette_style_icon_free_vector;
                        RefreshDatabase();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True");
            con.Open();
            if (pictureBox2.Image == Betlook_920_Football_System.Properties.Resources.soccer_game_team_players_and_ball_league_recreational_sports_tournament_silhouette_style_icon_free_vector)
            {
                MessageBox.Show("Please update image before proceeding or this will become you team logo");
            }
            try
            {
                if (MessageBox.Show("Do you want to update the data", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string str = "Update Teams set Matchplayed='" + MatchPlayedTextBox.Text + "',Wins ='" + WinsTextBox.Text + "',Loss ='" + LossesTextbox.Text + "',Goaldifference ='" + GoalDifferenceTextBox.Text + "',Clublogo ='" + pictureBox2.Image + "' Where Club='" + ClubNameTextBox.Text + "'";
                    SqlCommand command = new SqlCommand(str, con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("" + ClubNameTextBox.Text + "'s Details have been Updated Successfully.. ", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDatabase();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True");
            con.Open();
            if (ClubNameTextBox.Text == "")
                try
                {
                    if (MessageBox.Show("Do you want to delete the data", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string str = "DELETE FROM Teams WHERE Club = '" + ClubNameTextBox.Text + "'";
                        SqlCommand cmd = new SqlCommand(str, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Account Information Record Delete Successfully", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDatabase();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=\"Betlook\";Integrated Security=True");
            con.Open();
            if (SearchTextBox.Text == "")
            {
                MessageBox.Show("please enter data to perform search operation", "Important message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                try
                {
                    string SqlData = "Select * from Teams WHERE Club LIKE '%" + SearchTextBox.Text + "%'";
                    SqlCommand cmd = new SqlCommand(SqlData, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Club has been found", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClubIdTextBox.Text = dr.GetValue(0).ToString();
                        object imageDataObject = dr.GetValue(1);
                        if (imageDataObject != DBNull.Value)
                        {
                            byte[] imageData = (byte[])imageDataObject;
                            using (MemoryStream memoryStream = new MemoryStream(imageData))
                            {
                                pictureBox2.Image = Image.FromStream(memoryStream);
                            }
                        }
                        else
                        {
                            pictureBox2.Image = Betlook_920_Football_System.Properties.Resources.soccer_game_team_players_and_ball_league_recreational_sports_tournament_silhouette_style_icon_free_vector; // or set to a default image
                        }
                        ClubNameTextBox.Text = dr.GetValue(2).ToString();
                        MatchPlayedTextBox.Text = dr.GetValue(3).ToString();
                        WinsTextBox.Text = dr.GetValue(4).ToString();
                        LossesTextbox.Text = dr.GetValue(5).ToString();
                        GoalDifferenceTextBox.Text = dr.GetValue(6).ToString();
                        PointsTextbox.Text = dr.GetValue(7).ToString();
                        DrawsTextBox.Text = dr.GetValue(8).ToString();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, This user, " + SearchTextBox.Text + " is not available.", "Important Messgae", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        SearchTextBox.Text = "";
                        ClubNameTextBox.Clear();
                        MatchPlayedTextBox.Clear();
                        WinsTextBox.Clear();
                        LossesTextbox.Clear();
                        GoalDifferenceTextBox.Clear();
                        PointsTextbox.Clear();
                        DrawsTextBox.Clear();
                    }

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void TeamsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow >= 0)
            {
                DataGridViewRow row = TeamsDataGridView.Rows[indexRow];
                ClubIdTextBox.Text = row.Cells[0].Value.ToString();
                object imageDataObject = row.Cells[1].Value;
                if (imageDataObject != DBNull.Value)
                {
                    byte[] imageData = (byte[])imageDataObject;
                    using (MemoryStream memoryStream = new MemoryStream(imageData))
                    {
                        pictureBox2.Image = Image.FromStream(memoryStream);
                    }
                }
                else
                {
                    pictureBox2.Image = Betlook_920_Football_System.Properties.Resources.soccer_game_team_players_and_ball_league_recreational_sports_tournament_silhouette_style_icon_free_vector; // or set to a default image
                }
                ClubNameTextBox.Text = row.Cells[2].Value.ToString();
                MatchPlayedTextBox.Text = row.Cells[3].Value.ToString();
                WinsTextBox.Text = row.Cells[4].Value.ToString();
                LossesTextbox.Text = row.Cells[5].Value.ToString();
                GoalDifferenceTextBox.Text = row.Cells[6].Value.ToString();
                PointsTextbox.Text = row.Cells[7].Value.ToString();
                DrawsTextBox.Text = row.Cells[8].Value.ToString();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected image file path
                string imagePath = openFileDialog.FileName;

                // Load the image into the PictureBox
                pictureBox2.Image = new Bitmap(imagePath);

                byte[] imageData = ImageToByteArray(pictureBox2.Image);

                Image image = ByteArrayToImage(imageData);

                // Assign the Image to the PictureBox
                pictureBox2.Image = image;

                byte[] imageLogo = ImageToByteArray(pictureBox2.Image);


            }
        }

        private void Team_Management_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    string str = "SELECT * FROM Teams";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    TeamsDataGridView.DataSource = new BindingSource(dt, null);
                }
                TeamsDataGridView.ForeColor = Color.Black;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                Image image = Image.FromStream(memoryStream);
                return new Bitmap(image);
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        private void RefreshDatabase()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    string str = "SELECT * FROM Teams";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    TeamsDataGridView.DataSource = new BindingSource(dt, null);
                }
                TeamsDataGridView.ForeColor = Color.Black;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            Hide();
            HomeSelection homeSelection = new HomeSelection();
            homeSelection.Show();
        }

        private void StatsButton_Click(object sender, EventArgs e)
        {
            Hide();
            PlayersStats playersStats = new PlayersStats();
            playersStats.Show();
        }

        private void AddPlayersButton_Click(object sender, EventArgs e)
        {
            Hide();
            Add_Players add_Players = new Add_Players();
            add_Players.Show();
        }

        private void TeamsButton_Click(object sender, EventArgs e)
        {
            Hide();
            Select_Teams _Teams = new Select_Teams();
            _Teams.Show();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Hide();
            Play_Match play_Match = new Play_Match();
            play_Match.Show();
        }

        private void TournamentButton_Click(object sender, EventArgs e)
        {
            Hide();
            Tournament tournament = new Tournament();   
            tournament.Show();
        }

        private void WalletButton_Click(object sender, EventArgs e)
        {
            Hide();
            Wallet wallet = new Wallet();
            wallet.Show();
        }

        private void UserManagementButton1_Click(object sender, EventArgs e)
        {
            Hide();
            User_Management user_Management = new User_Management();
            user_Management.Show();
        }

        private void TeamManagementButton_Click(object sender, EventArgs e)
        {
            Hide();
            Team_Management team_Management = new Team_Management();
            team_Management.Show();
        }
    }
}
