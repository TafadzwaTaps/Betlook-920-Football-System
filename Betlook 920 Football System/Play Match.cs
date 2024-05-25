using BetLook_920_Football;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Betlook_920_Football_System
{

    public partial class Play_Match : Form
    {
        string Username = Login_Form.Username;
        private readonly RestClient restClient = new RestClient("https://api.openweathermap.org/data/2.5/weather?q=Rzeszow&appid=222105add318d24ebb985a15b2c4b239");
        public Play_Match()
        {
            InitializeComponent();
        }

        string homeTeam = Choose_Teams.ChosenTeam;
        string awayTeam;
        string stadium;
        Random Random = new Random();
        int userAccountBalance;
        int systemAccountBalance;

        private void DisplayScoreInTextBox(TextBox textBox, int score, Random random)
        {
            if (score <= 6)
            {
                textBox.Text = score.ToString();
            }
            else
            {
                textBox.Text = GenerateRandomScore(random).ToString();
            }
        }


        private int GenerateRandomScore(Random random)
        {
            return random.Next(7); // Generates a random integer from 0 to 6
        }

        private int GenerateNewRandomScore(Random random)
        {
            return random.Next(8); // Generates a random integer from 0 to 7
        }



        private void PlayMatchButton_Click(object sender, EventArgs e)
        {
            DataRowView selectedRow = (DataRowView)OppComboBox.SelectedItem;
            awayTeam = selectedRow["Club"].ToString();

            switch (awayTeam)
            {
                case "Manchester City":
                    OppTeamName.Text = "Manchester City";
                    OppPictureBox.Image = Properties.Resources.Manchester_City_FC_badge;
                    break;
                case "Manchester United":
                    OppTeamName.Text = "Manchester United";
                    OppPictureBox.Image = Properties.Resources.manchester_united_f_c_premier_league_logo_football_premier_league_emblem_text_thumbnail;
                    break;
                case "Arsenal":
                    OppTeamName.Text = "Arsenal";
                    OppPictureBox.Image = Properties.Resources.Arsenal_FC_logo;
                    break;
                case "Liverpool":
                    OppTeamName.Text = "Liverpool";
                    OppPictureBox.Image = Properties.Resources.Liverpool_FC;
                    break;
                case "Chelsea":
                    OppTeamName.Text = "Chelsea";
                    OppPictureBox.Image = Properties.Resources.Chelsea_FC_svg;
                    break;
                case "Everton":
                    OppTeamName.Text = "Everton";
                    OppPictureBox.Image = Properties.Resources.Everton_Logo;
                    break;
                case "Leicster City":
                    OppTeamName.Text = "Leicster City";
                    OppPictureBox.Image = Properties.Resources.leicester_city_fc_logo_FD9C3CA26E_seeklogo_com;
                    break;
                case "Tottenham":
                    OppTeamName.Text = "Tottenham";
                    OppPictureBox.Image = Properties.Resources.Tottenham_Logo;
                    break;
                default:
                    MessageBox.Show("Please Select A Team", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }

            stadium = StadiumComboBox.Text;

            switch (stadium)
            {
                case "Old Trafford (Manchester United)":
                    // Code for Old Trafford stadium
                    break;
                case "Anfield (Liverpool)":
                    // Code for Anfield stadium
                    break;
                case "Emirates Stadium (Arsenal)":
                    // Code for Emirates Stadium
                    break;
                case "Etihad Stadium (Manchester City)":
                    // Code for Etihad Stadium
                    break;
                case "Stamford Bridge (Chelsea)":
                    // Code for Stamford Bridge stadium
                    break;
                case "Tottenham Hotspur Stadium (Tottenham Hotspur)":
                    // Code for Tottenham Hotspur Stadium
                    break;
                case "Goodison Park (Everton)":
                    // Code for Goodison Park stadium
                    break;
                case "St James' Park (Newcastle United)":
                    // Code for St James' Park stadium
                    break;
                case "King Power Stadium":
                    // Code for King Power Stadium
                    break;
                case "Molineux Stadium":
                    // Code for Molineux Stadium
                    break;
                default:
                    MessageBox.Show("Invalid stadium selection", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }

            if (PitchComboBox.SelectedItem == null || PitchComboBox.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Please select pitch weather", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            if (TeamToWin.Text == "")
            {
                MessageBox.Show("Enter the name of the which will","Important Message",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            else if (BetAmountTextBox.Text == "")
            {
                MessageBox.Show("Please enter bet amount");
            }
            else
            {
                timer1.Start();
            }           
        }

        private Team RetrieveTeamStats(string teamName)
        {
            Team team = null;

            using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
            {
                string query = "SELECT Clubid, Matchplayed, Wins, Loss, Goaldifference, Points FROM Teams WHERE Club = @Club";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Club", teamName);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            team = new Team
                            {
                                ClubId = reader.GetInt32(0),
                                MatchPlayed = reader.GetInt32(1),
                                Wins = reader.GetInt32(2),
                                Loss = reader.GetInt32(3),
                                GoalDifference = reader.GetInt32(4),
                                Points = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }

            return team;
        }

        private DataTable GetTeamsFromDatabase()
        {
            DataTable teamsTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555"))
                {
                    connection.Open();

                    string query = "SELECT Clubid, Club FROM Teams";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(teamsTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving teams: " + ex.Message);
            }

            return teamsTable;
        }

        private List<Player> RetrievePlayersByClubId(int clubId)
        {
            List<Player> players = new List<Player>();

            using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555"))
            {
                string query = @"SELECT pp.FirstName, pp.LastName, pps.GoalsScored, pps.Assists, pps.Passes, pps.Shooting, pps.YellowCards, pps.RedCards, pp.Position
                        FROM PlayerPerformanceStats pps
                        INNER JOIN PlayerProfile pp ON pps.PlayerID = pp.PlayerID
                        WHERE pp.Clubid = @ClubId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClubId", clubId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player player = new Player
                                {
                                    FirstName = reader.GetString(0),
                                    LastName = reader.GetString(1),
                                    Goals = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                    Assists = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                    Position = reader.GetString(8) // Update the index to 8 for the Position column
                                };

                                players.Add(player);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while retrieving players: " + ex.Message);
                    }
                }
            }

            return players;
        }

        private int CalculateTeamScore(Team team, List<Player> players, string stadium, Random random)
        {
            int baseScore = (int)(team.Points * 0.5 + team.Wins * 0.3 + team.GoalDifference * 0.2);

            // Apply stadium effect
            double stadiumEffect = GetStadiumEffect(stadium);
            baseScore = (int)(baseScore * stadiumEffect);

            double randomFactor = (random.NextDouble() * 0.4) - 0.2; // Random value between -0.2 and 0.2

            int score = baseScore + (int)randomFactor;

            foreach (Player player in players)
            {
                int playerScore = (int)(player.Goals * 0.4 + player.Assists * 0.2);
                score += playerScore;
            }

            if (score > 5)
            {
                score = random.Next(5, 11);
            }
            return score;

        }

        private double GetStadiumEffect(string stadium)
        {
            // Define stadium effects based on the stadium name
            Dictionary<string, double> stadiumEffects = new Dictionary<string, double>()
            {
              { "StadiumA", 0.9 },
              { "StadiumB", 1.1 },
            };

            // Check if the stadium exists in the dictionary
            if (stadiumEffects.ContainsKey(stadium))
            {
                return stadiumEffects[stadium];
            }
            else
            {
                // If the stadium is not found, return a neutral effect (1.0)
                return 1.0;
            }
        }

        private double GetDifferentScore(double existingScore, double differentFrom)
        {
            const double MaxScore = 6.0; // Maximum score allowed
            const double MinScore = 0.0; // Minimum score allowed

            Random random = new Random();
            double score = existingScore;

            // Generate a different score within the allowed range
            while (score == existingScore || score == differentFrom)
            {
                score = random.NextDouble() * (MaxScore - MinScore) + MinScore;
            }

            return score;
        }


        public class Team
        {
            public int ClubId { get; set; }
            public int MatchPlayed { get; set; }
            public int Wins { get; set; }
            public int Loss { get; set; }
            public int GoalDifference { get; set; }
            public int Points { get; set; }
        }

        public class Player
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Goals { get; set; }
            public int Assists { get; set; }
            public string Position { get; set; }
        }

        private void Play_Match_Load(object sender, EventArgs e)
        {
            UserPictureBox.Image = Choose_Teams.ChosenImage;
            UserTeamLabel.Text = Choose_Teams.ChosenTeam;
            DataTable teamsTable = GetTeamsFromDatabase();

            // Bind the teams to the ComboBox
            OppComboBox.DisplayMember = "Club";
            OppComboBox.ValueMember = "Clubid";
            OppComboBox.DataSource = teamsTable;
            ApiWeatherData();

        }


        private void ApiWeatherData()
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q=Rzeszow&appid=ccef0127848996431ec751a199c5f956";

                var client = new RestClient(url);
                var request = new RestRequest();

                RestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var weatherData = JsonConvert.DeserializeObject<Coordinate>(response.Content);
                    var temperature = weatherData.Main.Temp;
                    var cityName = weatherData.Name;
                    foreach (var weather in weatherData.Weather)
                    {
                        var weatherType = weather.Main;
                        PitchComboBox.Items.Add(weatherType);
                    }
                }
                else
                {
                    PitchComboBox.Text = "Failed to retrieve weather data";
                }
            }
            catch (Exception ex)
            {
                PitchComboBox.Text = "An error occurred";
                MessageBox.Show(ex.Message);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            if(progressBar1.Value == 100)
            {
                timer1.Stop();
                if (!string.IsNullOrEmpty(homeTeam) && !string.IsNullOrEmpty(awayTeam))
                {
                    Team home = RetrieveTeamStats(homeTeam);
                    Team away = RetrieveTeamStats(awayTeam);

                    if (home != null && away != null)
                    {
                        List<Player> homePlayers = RetrievePlayersByClubId(home.ClubId);
                        List<Player> awayPlayers = RetrievePlayersByClubId(away.ClubId);

                        Random random = new Random();

                        int homeScore = CalculateTeamScore(home, homePlayers, stadium, random);
                        int awayScore = CalculateTeamScore(away, awayPlayers, stadium, random);


                        string result;

                        if (homeScore > awayScore)
                        {
                            result = homeTeam + " wins!";
                        }
                        else if (homeScore < awayScore)
                        {
                            result = awayTeam + " wins!";
                        }
                        else
                        {
                            result = "It's a draw!";
                        }


                        textBox1.Text = homeScore.ToString();
                        textBox2.Text = awayScore.ToString();                    
                        ResultLabel.Text = result;

                        double[] homeScores = { homeScore, 1, 0, 0 };
                        double[] awayScores = { awayScore, 0, 0, 0 };

                        for (int i = 1; i < 4; i++)
                        {
                            homeScores[i] = GetDifferentScore(homeScores[i - 1], awayScores[i - 1]);
                            awayScores[i] = GetDifferentScore(awayScores[i - 1], homeScores[i]);
                        }

                        DisplayScoreInTextBox(textBox1, homeScore, random);
                        DisplayScoreInTextBox(textBox2, awayScore, random);

                        double textBox1Value, textBox2Value;

                        if (!double.TryParse(textBox1.Text, out textBox1Value))
                        {
                            MessageBox.Show("Invalid value in TextBox 1.");
                            return;
                        }

                        if (!double.TryParse(textBox2.Text, out textBox2Value))
                        {
                            MessageBox.Show("Invalid value in TextBox 2.");
                            return;
                        }

                        if (textBox1Value > textBox2Value)
                        {
                            ResultLabel.Text = UserTeamLabel.Text + " wins";
                        }
                        else if (textBox1Value < textBox2Value)
                        {
                            ResultLabel.Text = OppTeamName.Text + " wins";
                        }
                        else
                        {
                            ResultLabel.Text = "It's a draw!";
                        }


                        string GameResult = ResultLabel.Text;
                        if (GameResult.Contains(TeamToWin.Text))
                        {
                            int systemBet = Random.Next(50, 50000);
                            int userBet = Convert.ToInt32(BetAmountTextBox.Text);
                            int totalBetEarning = userBet + systemBet;

                            userAccountBalance += totalBetEarning;
                            systemAccountBalance -= totalBetEarning;
                         
                            MessageBox.Show("Congratulations! You won the bet, and the money $ " + userAccountBalance.ToString() + " has been deposited to your account.");

                            SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True");
                            string query = "UPDATE Wallet SET Amount = @NewAmount WHERE AccountNumber IN (SELECT AccountNumber FROM SignUp WHERE Username = @Username)";
                            connection.Open();
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@NewAmount", userAccountBalance);
                            command.Parameters.AddWithValue("@Username", Username); // Assuming you have the username stored in a variable named 'username'

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                // Update successful
                                MessageBox.Show("Account balance updated successfully.");
                            }
                            else
                            {
                                // Update failed
                                MessageBox.Show("Failed to update account balance.");
                            }
                            MessageBox.Show(systemAccountBalance.ToString());
                            connection.Close();
                        }
                    
                        else
                        {

                            int userBet = Convert.ToInt32(BetAmountTextBox.Text);
                            userAccountBalance -= userBet;
                            systemAccountBalance += userBet;
                            MessageBox.Show("Sorry, you lost the bet, the money $ " + userAccountBalance.ToString() + " has been deducted from your account");
                            SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True");
                            connection.Open();
                            string query = "UPDATE Wallet SET Amount = @NewAmount WHERE AccountNumber IN (SELECT AccountNumber FROM SignUp WHERE Username = @Username)";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@NewAmount", userAccountBalance);
                            command.Parameters.AddWithValue("@Username", Username); // Assuming you have the username stored in a variable named 'username'

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                // Update successful
                                MessageBox.Show("Account balance updated successfully.");
                            }
                            else
                            {
                                // Update failed
                                MessageBox.Show("Failed to update account balance.");
                            }
                            MessageBox.Show(systemAccountBalance.ToString());
                            connection.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("One or both teams not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter both home and away team names.");
                }
            }    
        }

        private void IconButton1_Click(object sender, EventArgs e)
        {
            Hide();
            HomeSelection homeSelection = new HomeSelection();
            homeSelection.Show();
        }

        private void BetAmountTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
    }
}
