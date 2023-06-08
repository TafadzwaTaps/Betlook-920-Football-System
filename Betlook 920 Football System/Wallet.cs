using Betlook_920_Football_System;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class Wallet : Form
    {
        string Username = Login_Form.Username;
        string TeamName = Choose_Teams.ChosenTeam;
        public Wallet()
        {
            InitializeComponent();
        }

        private void IconButton9_Click(object sender, EventArgs e)
        {
            if (IbanAccountTextBox.Text == "" || string.IsNullOrEmpty(AmountTextBox.Text))
            {
                MessageBox.Show("Please enter account number and amount.", "Deposit");
                return;
            }

            decimal amount;
            if (!decimal.TryParse(AmountTextBox.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Invalid deposit amount.", "Deposit");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("INSERT INTO Wallet (AccountNumber, TransactionDate, TransactionType, PaymentMethod, Amount, Status, Currency, Clubid) VALUES (@AccountNumber, @TransactionDate, @TransactionType, @PaymentMethod, @Amount, @Status, @Currency, @Clubid)", connection);
                    command.Parameters.AddWithValue("@AccountNumber", IbanAccountTextBox.Text);
                    command.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                    command.Parameters.AddWithValue("@TransactionType", "Deposit");
                    command.Parameters.AddWithValue("@PaymentMethod", "Credit Card");
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@Status", "Pending");
                    command.Parameters.AddWithValue("@Currency", "USD");
                    command.Parameters.AddWithValue("@Clubid", ClubIdLbl.Text);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Successfully deposited {amount:C}.", "Deposit");
                        IbanAccountTextBox.Clear();
                        AmountTextBox.Clear();

                        ReceiveLbl.Text = $"Money Received To Account Holder ({Username})";
                        AmtLbl.Text = amount.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Deposit failed.", "Deposit");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Deposit");
            }
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(IbanAccountTextBox.Text) || string.IsNullOrEmpty(AmountTextBox.Text))
            {
                MessageBox.Show("Please enter account number and amount.", "Withdraw");
                return;
            }

            decimal amount;
            if (!decimal.TryParse(AmountTextBox.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Invalid withdrawal amount.", "Withdraw");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("INSERT INTO Wallet (AccountNumber, TransactionDate, TransactionType, PaymentMethod, Amount, Status, Currency, Clubid) VALUES (@AccountNumber, @TransactionDate, @TransactionType, @PaymentMethod, @Amount, @Status, @Currency, @Clubid)", connection);
                    command.Parameters.AddWithValue("@AccountNumber", IbanAccountTextBox.Text);
                    command.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                    command.Parameters.AddWithValue("@TransactionType", "Withdrawal");
                    command.Parameters.AddWithValue("@PaymentMethod", "Bank Transfer");
                    command.Parameters.AddWithValue("@Amount", -amount);
                    command.Parameters.AddWithValue("@Status", "Completed");
                    command.Parameters.AddWithValue("@Currency", "USD");
                    command.Parameters.AddWithValue("@Clubid", ClubIdLbl.Text);


                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Successfully withdrew {amount:C}.", "Withdraw");
                        IbanAccountTextBox.Clear();;
                        AmountTextBox.Clear();

                        PaymentLbl.Text = $"Money Received To Account Holder ({Username})";
                        AmtLbl2.Text = amount.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Withdrawal failed.", "Withdraw");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Withdraw");
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            Hide();
            Transfer_Form transfer = new Transfer_Form();
            transfer.Show();
        }

        private void PictureBox6_Click(object sender, EventArgs e)
        {
            Hide();
            Bank_Statement bank_Statement = new Bank_Statement();
            bank_Statement.Show();
        }

        private string RetrieveAccountNumber(string username)
        {
            string accountNumber = null;

            using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
            {
                string query = "SELECT AccountNumber FROM SignUp WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            accountNumber = reader.GetString(0);
                        }
                    }
                }
            }
            return accountNumber;
        }

        private void Wallet_Load(object sender, EventArgs e)
        {
            ClubIdLbl.Text = Choose_Teams.SelectedClubID;
            UserLabel.Text = Username;
            RetrieveAccNum();
            TeamNameLbl.Text = TeamName;
            AmountTextBox.Focus();
            IbanAccountTextBox.Focus();

            string query = "SELECT ISNULL(SUM(Amount), 0) FROM Wallet WHERE AccountNumber = @AccountNumber";

            using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True")) // Replace with your actual connection string
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AccountNumber", IbanAccountTextBox.Text);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    decimal balance = Convert.ToDecimal(result);

                    BalanceTextBox.Text = balance.ToString("0.00");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while retrieving the wallet balance: " + ex.Message);
                }
            }
        }

        private void IconButton1_Click(object sender, EventArgs e)
        {
            Hide();
            HomeSelection homeSelection = new HomeSelection();
            homeSelection.Show();
        }

        private void IconButton2_Click(object sender, EventArgs e)
        {
            Hide();
            PlayersStats playersStats = new PlayersStats();
            playersStats.Show();
        }

        private void IconButton3_Click(object sender, EventArgs e)
        {
            Hide();
            Team_Management team_Management = new Team_Management();
            team_Management.Show();
        }

        private void IconButton7_Click(object sender, EventArgs e)
        {
            Hide();
            PlayersStats playersStats = new PlayersStats();
            playersStats.Show();
        }

        private void IconButton4_Click(object sender, EventArgs e)
        {
            Hide();
            Play play = new Play();
            play.Show();
        }

        private void RetrieveAccNum()
        {
            string username = Login_Form.Username;

            if (!string.IsNullOrEmpty(username))
            {
                string accountNumber = RetrieveAccountNumber(username);

                if (accountNumber != null)
                {
                    IbanAccountTextBox.Text = accountNumber;
                    MasterCardAccNum.Text = IbanAccountTextBox.Text.Trim();
                }
                else
                {
                    MessageBox.Show("Account number not found.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a username.");
            }
        }
    }
}
