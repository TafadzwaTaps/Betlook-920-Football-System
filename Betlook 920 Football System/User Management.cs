using BetLook_920_Football;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Betlook_920_Football_System
{
    public partial class User_Management : Form
    {
        public User_Management()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555";
            if (UsernameTextBox.Text == "" || PasswordTextBox.Text == "" || FullNameTextBox.Text == "" || GenderComboBox.Text == "" || UserRoleComboBox.Text == "" || PhoneNumberTextbox.Text == "")
            {
                MessageBox.Show("missing information", "Important text", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (PhoneNumberTextbox.Text.Length > 10 && PhoneNumberTextbox.Text.Length < 10)
            {
                MessageBox.Show("please enter a valid phone number");
            }
            else
            {
                try
                {
                    con.Open();
                    if (MessageBox.Show("Are you sure you want to submit this data, please confirm before submitting", "Important Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "INSERT INTO SignUp VALUES ('" + UsernameTextBox.Text + "','" + FullNameTextBox.Text + "','" + GenderComboBox.SelectedItem.ToString() + "','" + PasswordTextBox.Text + "','" + PhoneNumberTextbox.Text + "','" + UserRoleComboBox.SelectedItem.ToString() + "')";
                        SqlCommand command = new SqlCommand(query, con);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Data successfully recorded", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        con.Close();
                        UsernameTextBox.Clear();
                        FullNameTextBox.Clear();
                        GenderComboBox.Text = "";
                        PasswordTextBox.Clear();
                        PhoneNumberTextbox.Clear();
                        UserRoleComboBox.Text = "";
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
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555");
            con.Open();
            try
            {
                if (MessageBox.Show("Do you want to update the data", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string str = "Update SignUp set UserName='" + UsernameTextBox.Text + "',Fullname ='" + FullNameTextBox.Text + "',Password ='" + PasswordTextBox.Text + "',PhoneNumber ='" + PhoneNumberTextbox.Text + "',UserRole ='" + UserRoleComboBox.SelectedItem.ToString() + "' Where UserId='" + UserIDITextBox.Text + "'";
                    SqlCommand command = new SqlCommand(str, con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("" + UsernameTextBox.Text + "'s Details have been Updated Successfully.. ", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555");
            con.Open();
            if (UserIDITextBox.Text == "")
                try
                {
                    if (MessageBox.Show("Do you want to delete the data", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string str = "DELETE FROM UserTbl WHERE UserID = '" + UserIDITextBox.Text + "'";
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
                    string SqlData = "Select * from SignUp WHERE Username LIKE '%" + SearchTextBox.Text + "%'";
                    SqlCommand cmd = new SqlCommand(SqlData, con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("User has been found", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        UserIDITextBox.Text = dr.GetValue(0).ToString();
                        UsernameTextBox.Text = dr.GetValue(1).ToString();
                        FullNameTextBox.Text = dr.GetValue(2).ToString();
                        GenderComboBox.Text = dr.GetValue(3).ToString();
                        PasswordTextBox.Text = dr.GetValue(4).ToString();
                        PhoneNumberTextbox.Text = dr.GetValue(5).ToString();
                        UserRoleComboBox.Text = dr.GetValue(6).ToString();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, This user, " + SearchTextBox.Text + " is not available.", "Important Messgae", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        SearchTextBox.Text = "";
                    }

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UserDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow >= 0)
            {
                DataGridViewRow row = UserDataGridView.Rows[indexRow];
                UserIDITextBox.Text = row.Cells[0].Value.ToString();
                UsernameTextBox.Text = row.Cells[1].Value.ToString();
                FullNameTextBox.Text = row.Cells[2].Value.ToString();
                GenderComboBox.Text = row.Cells[3].Value.ToString();
                PasswordTextBox.Text = row.Cells[4].Value.ToString();
                PhoneNumberTextbox.Text = row.Cells[5].Value.ToString();
                UserRoleComboBox.Text = row.Cells[6].Value.ToString();
            }
        }

        private void User_Management_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555"))
                {
                    string str = "SELECT * FROM SignUp";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    UserDataGridView.DataSource = new BindingSource(dt, null);
                }
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
            Stats stats = new Stats();
            stats.Show();
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
            Teams teams = new Teams();
            teams.Show();
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

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Hide();
            Play_Match play_Match = new Play_Match();
            play_Match.Show();
        }

        private void RefreshDatabase()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=False;User Id=sa;Password=qqq555"))
                {
                    string str = "SELECT * FROM SignUp";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    UserDataGridView.DataSource = new BindingSource(dt, null);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
