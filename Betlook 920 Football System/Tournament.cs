using BetLook_920_Football;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace Betlook_920_Football_System
{
    public partial class Tournament : Form
    {
        public static string AccountNum = SIgnUp.AccNumber;
        private const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True";
        private readonly List<string> teamNames = new List<string> { "Arsenal", "Manchester City", "Liverpool", "Manchester United", "Chelsea", "Tottenham", "Everton", "Leicester City" };
        private List<string> ListTeamNames;

        private SqlConnection connection;
        private SqlCommand command;

        string Username = Login_Form.Username;
        public Tournament()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            connection = new SqlConnection(connectionString);
            command = connection.CreateCommand();
        }

        private void Tournament_Load(object sender, EventArgs e)
        {
            UserLabel.Text = Username;
            ListTeamNames = new List<string>
            {
                "Team A",
                "Team B",
                "Team C",
                "Team D",
                "Team E",
                "Team F",
                "Team G",
                "Team H"
            };

            // Shuffle the team names randomly
            ShuffleTeams(ListTeamNames);

            // Display the first two teams playing
            DisplayTeams();


            Random random = new Random();

            string GenerateTicket()
            {
                StringBuilder ticket = new StringBuilder();
                ticket.Append((char)random.Next('A', 'Z' + 1));
                ticket.Append((char)random.Next('A', 'Z' + 1));
                ticket.Append(random.Next(1000, 10000).ToString("D4"));
                return ticket.ToString();
            }

            TicketTxtBox.Text = GenerateTicket();
            TicketTxtBoxTwo.Text = GenerateTicket();
            TicketTxtBoxThree.Text = GenerateTicket();
            TicketTxtBoxFour.Text = GenerateTicket();

            connection.Open();
            string query = "SELECT Amount FROM Wallet w JOIN SignUp s ON w.AccountNumber = s.AccountNumber WHERE s.Username = @Username";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);

            // Execute the command and retrieve the account balance

            decimal? accountBalance = command.ExecuteScalar() as decimal?;
            connection.Close();

            if (accountBalance.HasValue)
            {
                UserAccountBalanceTextBox.Text = accountBalance.Value.ToString();
            }
            else
            {
                UserAccountBalanceTextBox.Text = "0";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            if (LblTeam1.Checked == false && LblTeam2.Checked == false ||
                LblTeam3.Checked == false && LblTeam4.Checked == false ||
                LblTeam5.Checked == false && LblTeam6.Checked == false ||
                LblTeam7.Checked == false && LblTeam8.Checked == false)
            {
                MessageBox.Show("Please pick the team which will win");
                isValid = false;
            }
            else if (LblTeam1.Checked == true && LblTeam2.Checked == true ||
                     LblTeam3.Checked == true && LblTeam4.Checked == true ||
                     LblTeam5.Checked == true && LblTeam6.Checked == true ||
                     LblTeam7.Checked == true && LblTeam8.Checked == true)
            {
                MessageBox.Show("You can only select one team");
                isValid = false;
            }

            if (TeamWinAmountOne.Text == "" || TeamWinAmountTwo.Text == "" ||
                TeamWinAmountThree.Text == "" || TeamWinAmountFour.Text == "")
            {
                MessageBox.Show("Please enter an amount to bet on which team will win");
                isValid = false;
            }

            if (WinCheckBox.Checked == false && DrawCheckBox.Checked == false && LossCheckBox.Checked == false ||
                WinCheckBox1.Checked == false && DrawCheckBox1.Checked == false && LossCheckBox1.Checked == false ||
                WinCheckBox2.Checked == false && DrawCheckBox2.Checked == false && LossCheckBox2.Checked == false ||
                WinCheckBox3.Checked == false && DrawCheckBox3.Checked == false && LossCheckBox3.Checked == false)
            {
                MessageBox.Show("Please tick an option");
                isValid = false;
            }
            else if (WinCheckBox.Checked == true && DrawCheckBox.Checked == true && LossCheckBox.Checked == true ||
                     WinCheckBox1.Checked == true && DrawCheckBox1.Checked == true && LossCheckBox1.Checked == true ||
                     WinCheckBox2.Checked == true && DrawCheckBox2.Checked == true && LossCheckBox2.Checked == true ||
                     WinCheckBox3.Checked == true && DrawCheckBox3.Checked == true && LossCheckBox3.Checked == true)
            {
                MessageBox.Show("Please tick only one option");
                isValid = false;
            }

            if (isValid)
            {
                timer1.Start();
            }

        }

        private void DisplayScoreInTextBox(TextBox textBox, double score)
        {
            if (score <= 7)
            {
                textBox.Text = score.ToString();
            }
            else
            {
                textBox.Text = GenerateRandomScore().ToString();
            }
        }

        private double GenerateRandomScore()
        {
            Random random = new Random();
            return random.NextDouble() * 7;
        }

        private double GenerateNewRandomScore()
        {
            Random random = new Random();
            return random.NextDouble() * 8;
        }

        private void SetWinningTeamLabels(string winningTeam)
        {
            TeamToWin1.Text = $"{winningTeam} wins!";
            TeamToWin2.Text = $"{winningTeam} wins!";
            TeamToWin3.Text = $"{winningTeam} wins!";
            TeamToWin4.Text = $"{winningTeam} wins!";
        }

        private string GetMatchResult(string homeTeam, string awayTeam, out double scoreA, out double scoreB)
        {
            scoreA = 0;
            scoreB = 0;

            if (string.IsNullOrEmpty(homeTeam) || string.IsNullOrEmpty(awayTeam))
            {
                MessageBox.Show("Please enter both home and away team names.");
                return string.Empty;
            }

            Team home = RetrieveTeamStats(homeTeam);
            Team away = RetrieveTeamStats(awayTeam);

            if (home != null && away != null)
            {
                List<Player> homePlayers = RetrievePlayersByClubId(home.ClubId);
                List<Player> awayPlayers = RetrievePlayersByClubId(away.ClubId);

                double homeScore = CalculateTeamScore(home, homePlayers);
                double awayScore = CalculateTeamScore(away, awayPlayers);

                scoreA = homeScore;
                scoreB = awayScore;


                // Check if there is a draw
                if (homeScore == awayScore)
                {
                    textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = homeScore.ToString();
                    textBox7.Text = textBox8.Text = awayScore.ToString();
                    TeamToWin1.Text = "It's a draw!";
                    TeamToWin2.Text = "It's a draw!";
                    TeamToWin3.Text = "It's a draw!";
                    TeamToWin4.Text = "It's a draw!";
                    return TeamToWin1.Text;
                }

                double[] homeScores = { homeScore, 0, 0, 0 };
                double[] awayScores = { awayScore, 0, 0, 0 };

                for (int i = 1; i < 4; i++)
                {
                    homeScores[i] = GetDifferentScore(homeScores[i - 1], awayScores[i - 1]);
                    awayScores[i] = GetDifferentScore(awayScores[i - 1], homeScores[i]);
                }

                DisplayScoreInTextBox(textBox1, homeScores[0]);
                DisplayScoreInTextBox(textBox2, awayScores[0]);
                DisplayScoreInTextBox(textBox3, homeScores[1]);
                DisplayScoreInTextBox(textBox4, awayScores[1]);
                DisplayScoreInTextBox(textBox5, homeScores[2]);
                DisplayScoreInTextBox(textBox6, awayScores[2]);
                DisplayScoreInTextBox(textBox7, homeScores[3]);
                DisplayScoreInTextBox(textBox8, awayScores[3]);

                // Determine the winner
                string result;
                double WinAmount = 0.0;

                if (WinCheckBox.Checked)
                {
                    result = $"{homeTeam} wins!";
                    WinAmount = double.Parse(TeamWinAmountOne.Text);
                    SetWinningTeamLabels(homeTeam);
                }
                else if (DrawCheckBox.Checked)
                {
                    result = "It's a draw!";
                    SetWinningTeamLabels("");
                }
                else if (LossCheckBox.Checked)
                {
                    result = $"{awayTeam} wins!";
                    WinAmount = double.Parse(TeamWinAmountOne.Text);
                    SetWinningTeamLabels(awayTeam);
                }

                else
                {
                    MessageBox.Show("Please select an outcome (win, draw, or loss).");
                    return string.Empty;
                }

                TeamToWin4.Text = result;


                if (scoreA > 7 || scoreB > 8)
                {
                    scoreA = GenerateRandomScore();
                    scoreB = GenerateNewRandomScore();
                }


                if (scoreA > 7)
                {
                    scoreA = GenerateRandomScore();
                    DisplayScoreInTextBox(textBox1, scoreA);
                }
                if (scoreB > 8)
                {
                    scoreB = GenerateNewRandomScore();
                    DisplayScoreInTextBox(textBox2, scoreB);
                }
            }
            return string.Empty;
        }


        private Team RetrieveTeamStats(string teamName)
        {
            Team team = null;

            string query = "SELECT Clubid, Matchplayed, Wins, Loss, Goaldifference, Points FROM Teams WHERE Club = @Club";
            command.CommandText = query;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Club", teamName);

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while retrieving team stats: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return team;
        }

        private List<Player> RetrievePlayersByClubId(int clubId)
        {
            List<Player> players = new List<Player>();

            string query = @"SELECT pp.FirstName, pp.LastName, pps.GoalsScored, pps.Assists, pps.Passes, pps.Shooting, pps.YellowCards, pps.RedCards, pp.Position
        FROM PlayerPerformanceStats pps
        INNER JOIN PlayerProfile pp ON pps.PlayerID = pp.PlayerID
        WHERE pp.Clubid = @ClubId";

            command.CommandText = query;
            command.Parameters.Clear();
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
            finally
            {
                connection.Close();
            }

            return players;
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

        private double CalculateTeamScore(Team team, List<Player> players)
        {
            const double MaxScore = 7.0; // Maximum score allowed
            const double MinScore = 0.0; // Minimum score allowed

            double baseScore = team.Points * 0.5 + team.Wins * 0.3 + team.GoalDifference * 0.2;

            Random random = new Random();
            double randomFactor = (random.NextDouble() * 0.4) - 0.2; // Random value between -0.2 and 0.2

            double score = baseScore + randomFactor;

            foreach (Player player in players)
            {
                double playerScore = player.Goals * 0.4 + player.Assists * 0.2;
                playerScore = Math.Max(Math.Min(playerScore, MaxScore - score), MinScore);

                score += playerScore;
            }

            Random rand = new Random();
            double variation = rand.NextDouble() * 0.5 - 0.25;
            score += variation;

            return score;
        }

        private double GetDifferentScore(double existingScore, double differentFrom)
        {
            const double MaxScore = 7.0; // Maximum score allowed
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
        private void DisplayTeams()
        {
            if (teamNames.Count >= 2)
            {
                LblTeam1.Text = teamNames[0];
                LblTeam2.Text = teamNames[1];
                LblTeam3.Text = teamNames[2];
                LblTeam4.Text = teamNames[3];
                LblTeam5.Text = teamNames[4];
                LblTeam6.Text = teamNames[5];
                LblTeam7.Text = teamNames[6];
                LblTeam8.Text = teamNames[7];
            }
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

        public class Bet
        {
            public string TeamOne { get; set; }
            public string TeamTwo { get; set; }
            public double Amount { get; set; }
            public string Outcome { get; set; }
        }

        public class Player
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Goals { get; set; }
            public int Assists { get; set; }
            public string Position { get; set; }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            if (progressBar1.Value == 100)
            {
                timer1.Stop();


                string teamA = LblTeam1.Text;
                string teamB = LblTeam2.Text;
                string teamC = LblTeam3.Text;
                string teamD = LblTeam4.Text;
                string teamE = LblTeam5.Text;
                string teamF = LblTeam6.Text;
                string teamG = LblTeam7.Text;
                string teamH = LblTeam8.Text;


                double scoreA, scoreB;

                string result = GetMatchResult(teamA, teamB, out scoreA, out scoreB);
          
                TeamToWin1.Text = result;

                result = GetMatchResult(teamC, teamD, out scoreA, out scoreB);
                TeamToWin2.Text = result;

                result = GetMatchResult(teamE, teamF, out scoreA, out scoreB);
                TeamToWin3.Text = result;

                result = GetMatchResult(teamG, teamH, out scoreA, out scoreB);
               

                DisplayScoreInTextBox(textBox1, scoreA);
                DisplayScoreInTextBox(textBox2, scoreB);


                double textBox1Value, textBox2Value, textBox3Value, textBox4Value, textBox5Value, textBox6Value, textBox7Value, textBox8Value;

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

                if (!double.TryParse(textBox3.Text, out textBox3Value))
                {
                    MessageBox.Show("Invalid value in TextBox 3.");
                    return;
                }

                if (!double.TryParse(textBox4.Text, out textBox4Value))
                {
                    MessageBox.Show("Invalid value in TextBox 4.");
                    return;
                }

                if (!double.TryParse(textBox5.Text, out textBox5Value))
                {
                    MessageBox.Show("Invalid value in TextBox 5.");
                    return;
                }

                if (!double.TryParse(textBox6.Text, out textBox6Value))
                {
                    MessageBox.Show("Invalid value in TextBox 6.");
                    return;
                }

                if (!double.TryParse(textBox7.Text, out textBox7Value))
                {
                    MessageBox.Show("Invalid value in TextBox 7.");
                    return;
                }

                if (!double.TryParse(textBox8.Text, out textBox8Value))
                {
                    MessageBox.Show("Invalid value in TextBox 8.");
                    return;
                }

                int textBox1IntValue = Convert.ToInt32(textBox1Value);
                int textBox2IntValue = Convert.ToInt32(textBox2Value);
                int textBox3IntValue = Convert.ToInt32(textBox3Value);
                int textBox4IntValue = Convert.ToInt32(textBox4Value);
                int textBox5IntValue = Convert.ToInt32(textBox5Value);
                int textBox6IntValue = Convert.ToInt32(textBox6Value);
                int textBox7IntValue = Convert.ToInt32(textBox7Value);
                int textBox8IntValue = Convert.ToInt32(textBox8Value);

                textBox1.Text = textBox1IntValue.ToString(); 
                textBox2.Text = textBox2IntValue.ToString();
                textBox3.Text = textBox3IntValue.ToString();
                textBox4.Text = textBox4IntValue.ToString();
                textBox5.Text = textBox5IntValue.ToString();
                textBox6.Text = textBox6IntValue.ToString();
                textBox7.Text = textBox7IntValue.ToString();
                textBox8.Text = textBox8IntValue.ToString();

                double textAmount1Value, textAmount2Value, textAmount3Value, textAmount4Value;
                if (!double.TryParse(TeamWinAmountOne.Text, out textAmount1Value))
                {
                    MessageBox.Show("Invalid value in amount textbox.");
                    return;
                }
                if (!double.TryParse(TeamWinAmountTwo.Text, out textAmount2Value))
                {
                    MessageBox.Show("Invalid value in amount textbox.");
                    return;
                }
                if (!double.TryParse(TeamWinAmountThree.Text, out textAmount3Value))
                {
                    MessageBox.Show("Invalid value in amount textbox.");
                    return;
                }
                if (!double.TryParse(TeamWinAmountFour.Text, out textAmount4Value))
                {
                    MessageBox.Show("Invalid value in amount textbox.");
                    return;
                }

                double totalBetEarnings = 0;
                double totalBetLosses = 0;

                if (textBox1IntValue > textBox2IntValue )
                {
                    TeamToWin1.Text = LblTeam1.Text + " wins!";
                    totalBetEarnings += textAmount1Value;
                }
                else if (textBox1IntValue < textBox2IntValue )
                {
                    TeamToWin1.Text = LblTeam2.Text + " wins!";
                    totalBetLosses += textAmount1Value * 1.5;
                }
                else
                {
                    TeamToWin1.Text = "It's a draw!";
                    double drawBet = textAmount1Value / 2; // Calculate 1/4th of the bet
                    totalBetEarnings += (textAmount1Value - drawBet);
                    totalBetLosses += drawBet;
                }

                if (textBox3IntValue > textBox4IntValue ) 
                {
                    TeamToWin2.Text = LblTeam3.Text + " wins!";
                    totalBetEarnings += textAmount2Value;
                }
                else if (textBox3IntValue < textBox4IntValue)
                {
                    TeamToWin2.Text = LblTeam4.Text + " wins!";
                    totalBetLosses += textAmount2Value * 1.5;
                }
                else
                {
                    TeamToWin2.Text = "It's a draw!";
                    double drawBet = textAmount2Value / 2; // Calculate 1/4th of the bet
                    totalBetEarnings += (textAmount2Value - drawBet);
                    totalBetLosses += drawBet;
                }


                if (textBox5IntValue > textBox6IntValue)
                {
                    TeamToWin3.Text = LblTeam5.Text + " wins!";
                    totalBetEarnings += textAmount3Value;
                }
                else if (textBox5IntValue < textBox6IntValue)
                {
                    TeamToWin3.Text = LblTeam6.Text + " wins!";
                    totalBetLosses += textAmount3Value * 1.5;
                }
                else
                {
                    TeamToWin3.Text = "It's a draw!";
                    double drawBet = textAmount3Value / 2; // Calculate 1/4th of the bet
                    totalBetEarnings += (textAmount3Value - drawBet);
                    totalBetLosses += drawBet;
                }
  

                if (textBox7IntValue > textBox8IntValue)
                {
                    TeamToWin4.Text = LblTeam7.Text + " wins!";
                    totalBetEarnings += textAmount4Value;
                }
                else if (textBox7IntValue < textBox8IntValue)
                {
                    TeamToWin4.Text = LblTeam8.Text + " wins!";
                    totalBetLosses += textAmount4Value * 1.5;
                }
                else
                {
                    TeamToWin4.Text = "It's a draw!";
                    double drawBet = textAmount4Value / 2; // Calculate 1/4th of the bet
                    totalBetEarnings += (textAmount4Value - drawBet);
                    totalBetLosses += drawBet;
                }

                decimal accountBalance = Convert.ToDecimal(UserAccountBalanceTextBox.Text);
                double newAccountBalance =  Convert.ToDouble(accountBalance) - totalBetLosses + totalBetEarnings;
                UserAccountBalanceTextBox.Text = newAccountBalance.ToString();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE Wallet SET Amount = @NewAmount WHERE AccountNumber IN (SELECT AccountNumber FROM SignUp WHERE Username = @Username)";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@NewAmount", newAccountBalance);
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
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the account balance: " + ex.Message);
                }
                BetEarningTextBox.Text = totalBetEarnings.ToString();
                BetLossTextBox.Text = totalBetLosses.ToString();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            List<string> shuffledTeams = ShuffleTeams(teamNames); // Shuffle the team names randomly

            LblTeam1.Text = shuffledTeams[0];
            LblTeam2.Text = shuffledTeams[1];
            LblTeam3.Text = shuffledTeams[2];
            LblTeam4.Text = shuffledTeams[3];
            LblTeam5.Text = shuffledTeams[4];
            LblTeam6.Text = shuffledTeams[5];
            LblTeam7.Text = shuffledTeams[6];
            LblTeam8.Text = shuffledTeams[7];

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }

        private void Button3_Click(object sender, EventArgs e)
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

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Hide();
            Profile profile = new Profile();
            profile.Show();
        }
    }
}
