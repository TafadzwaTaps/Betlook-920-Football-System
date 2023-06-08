using BetLook_920_Football;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Betlook_920_Football_System
{
    public partial class AdminHomeSelection : Form
    {
        private readonly List<string> teamNames = new List<string> { "Arsenal", "Manchester City", "Liverpool", "Manchester United", "Chelsea", "Tottenham", "Everton", "Leicester City" };

        public AdminHomeSelection()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            Hide();
            AdminHomeSelection adminHomeSelection = new AdminHomeSelection();
            adminHomeSelection.Show();
        }

        private void StatsButton_Click(object sender, EventArgs e)
        {
            Hide();
            PlayersStats playersStats = new PlayersStats();
            playersStats.Show();
        }

        private void AddPlayerButton_Click(object sender, EventArgs e)
        {
            Hide();
            Add_Players add_Players = new Add_Players();
            add_Players.Show();
        }

        private void TeamsButton_Click(object sender, EventArgs e)
        {
            Hide();
            Select_Teams select_Teams = new Select_Teams();
            select_Teams.Show();
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

        private void IconButton2_Click(object sender, EventArgs e)
        {
            Hide();
            User_Management user_Management = new User_Management();
            user_Management.Show();
        }

        private void IconButton1_Click(object sender, EventArgs e)
        {
            Hide();
            Team_Management team_Management = new Team_Management();
            team_Management.Show();
        }

        private void AdminHomeSelection_Load(object sender, EventArgs e)
        {
            UserTeamLbl.Text = Choose_Teams.ChosenTeam;

            List<string> shuffledTeams = ShuffleTeams(teamNames); // Shuffle the team names randomly

            string teamA = shuffledTeams[0];

            OppTeamLbl.Text = teamA;

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555"))
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

            pictureBox3.Image = Choose_Teams.ChosenImage;
            TeamsDataGridView.Columns[2].Width = 175;

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555"))
                {
                    string query = "SELECT Clublogo FROM Teams WHERE club = @club";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@club", teamA);
                    con.Open();

                    byte[] clubLogoBytes = (byte[])command.ExecuteScalar();

                    if (clubLogoBytes != null)
                    {
                        using (MemoryStream ms = new MemoryStream(clubLogoBytes))
                        {
                            Image clubLogo = Image.FromStream(ms);
                            pictureBox4.Image = clubLogo;
                        }
                    }
                    else
                    {
                        // Handle the case when the team or club logo is not found
                        MessageBox.Show("Club logo not found for team: " + teamA);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private List<string> ShuffleTeams(List<string> teamList)
        {
            Random random = new Random();
            List<string> shuffledList = new List<string>(teamList);

            int n = shuffledList.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }

            return shuffledList;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Hide();
            Profile profile = new Profile();
            profile.Show();
        }
    }
}
