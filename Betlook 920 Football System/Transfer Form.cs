using Betlook_920_Football_System;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class Transfer_Form : Form
    {
        public Transfer_Form()
        {
            InitializeComponent();
        }

        private void TransferButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SenderAccountTextBox.Text) || string.IsNullOrEmpty(ReceiverAccTextBox.Text) || string.IsNullOrEmpty(AmountTextBox.Text))
            {
                MessageBox.Show("Please enter sender account number, receiver account number, and amount.", "Transfer");
                return;
            }

            decimal amount;
            if (!decimal.TryParse(AmountTextBox.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Invalid transfer amount.", "Transfer");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    connection.Open();

                    // Deduct amount from sender's account
                    SqlCommand debitCommand = new SqlCommand("INSERT INTO Wallet (AccountNumber, TransactionDate, TransactionType, PaymentMethod, Amount, Status, Currency) VALUES (@AccountNumber, @TransactionDate, @TransactionType, @PaymentMethod, @Amount, @Status, @Currency)", connection);
                    debitCommand.Parameters.AddWithValue("@AccountNumber", SenderAccountTextBox.Text);
                    debitCommand.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                    debitCommand.Parameters.AddWithValue("@TransactionType", "Debit");
                    debitCommand.Parameters.AddWithValue("@PaymentMethod", "Account Transfer");
                    debitCommand.Parameters.AddWithValue("@Amount", -amount);
                    debitCommand.Parameters.AddWithValue("@Status", "Pending");
                    debitCommand.Parameters.AddWithValue("@Currency", "USD");
                    

                    // Add amount to receiver's account
                    SqlCommand creditCommand = new SqlCommand("INSERT INTO Wallet (AccountNumber, TransactionDate, TransactionType, PaymentMethod, Amount, Status, Currency) VALUES (@AccountNumber, @TransactionDate, @TransactionType, @PaymentMethod, @Amount, @Status, @Currency)", connection);
                    creditCommand.Parameters.AddWithValue("@AccountNumber", ReceiverAccTextBox.Text);
                    creditCommand.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                    creditCommand.Parameters.AddWithValue("@TransactionType", "Credit");
                    creditCommand.Parameters.AddWithValue("@PaymentMethod", "Account Transfer");
                    creditCommand.Parameters.AddWithValue("@Amount", amount);
                    creditCommand.Parameters.AddWithValue("@Status", "Pending");
                    creditCommand.Parameters.AddWithValue("@Currency", "USD");

                    // Execute both commands within a single transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        debitCommand.Transaction = transaction;
                        creditCommand.Transaction = transaction;

                        try
                        {
                            debitCommand.ExecuteNonQuery();
                            creditCommand.ExecuteNonQuery();

                            transaction.Commit();

                            MessageBox.Show($"Successfully transferred {amount:C} from {SenderAccountTextBox.Text} to {ReceiverAccTextBox.Text}.", "Transfer");
                            SenderAccountTextBox.Clear();
                            ReceiverAccTextBox.Clear();
                            AmountTextBox.Clear();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Transfer failed. Error: {ex.Message}", "Transfer");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Transfer");
            }
        }

        private void BalanceButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AccountNumberTextBox.Text))
            {
                MessageBox.Show("Please enter an account number.", "Check Balance",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT SUM(Amount) AS Balance FROM Wallet WHERE AccountNumber = @AccountNumber", connection);
                    command.Parameters.AddWithValue("@AccountNumber", AccountNumberTextBox.Text);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        decimal balance = Convert.ToDecimal(result);
                        MessageBox.Show($"Current balance for account {SenderAccountTextBox.Text}: {balance:C}", "Check Balance",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        SenderAccountTextBox.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Account not found or no transactions available.", "Check Balance");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Check Balance");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TransactionIdTextBox.Text))
            {
                MessageBox.Show("Please enter a transaction ID.", "Cancel Transaction");
                return;
            }

            int transactionID;
            if (!int.TryParse(TransactionIdTextBox.Text, out transactionID) || transactionID <= 0)
            {
                MessageBox.Show("Invalid transaction ID.", "Cancel Transaction");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    connection.Open();

                    SqlCommand checkCommand = new SqlCommand("SELECT Status FROM Wallet WHERE TransactionID = @TransactionID", connection);
                    checkCommand.Parameters.AddWithValue("@TransactionID", transactionID);

                    object result = checkCommand.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string status = result.ToString();
                        if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                        {
                            SqlCommand cancelCommand = new SqlCommand("UPDATE Wallet SET Status = 'Cancelled' WHERE TransactionID = @TransactionID", connection);
                            cancelCommand.Parameters.AddWithValue("@TransactionID", transactionID);

                            int rowsAffected = cancelCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Transaction {transactionID} has been cancelled.", "Cancel Transaction");
                                TransactionIdTextBox.Clear();
                            }
                            else
                            {
                                MessageBox.Show($"Unable to cancel transaction {transactionID}.", "Cancel Transaction");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Transaction {transactionID} cannot be cancelled. It is already processed or cancelled.", "Cancel Transaction");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Transaction {transactionID} not found.", "Cancel Transaction");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Cancel Transaction");
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
           PlayersStats players = new PlayersStats();
            players.Show();
        }

        private void AddPlayersButton_Click(object sender, EventArgs e)
        {
            Hide();
            Add_Players add_Players = new Add_Players();
            add_Players.Show();
        }

        private void GamePlayButton_Click(object sender, EventArgs e)
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
    }
}
