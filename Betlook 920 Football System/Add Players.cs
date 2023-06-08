using BetLook_920_Football;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Betlook_920_Football_System
{
    public partial class Add_Players : Form
    {
        string User = Login_Form.Username;
        public Add_Players()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True";
            if (FirstNameTextBox.Text == "" || LastNameTextBox.Text == "" || PositionTextBox.Text == "" || PlayerOvrRatingTextBox.Text == "" || ClubTextBox.Text == "" || ClubIdTextbox.Text == "" || HealthTextBox.Text == "" || TeamNumberTextBox.Text == "")
            {
                MessageBox.Show("Missing information", "Important message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    con.Open();
                    if (MessageBox.Show("Are you sure you want to submit this data? Please confirm before submitting.", "Important Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "INSERT INTO PlayerProfile (FirstName, LastName, Position, PlayerOvrRating, Club, Clubid, Health, TeamNumber) VALUES (@FirstName, @LastName, @Position, @PlayerOvrRating, @Club, @Clubid, @Health, @TeamNumber)";
                        SqlCommand command = new SqlCommand(query, con);
                        command.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text);
                        command.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                        command.Parameters.AddWithValue("@Position", PositionTextBox.Text);
                        command.Parameters.AddWithValue("@PlayerOvrRating", PlayerOvrRatingTextBox.Text);
                        command.Parameters.AddWithValue("@Club", ClubTextBox.Text);
                        command.Parameters.AddWithValue("@Clubid", ClubIdTextbox.Text);
                        command.Parameters.AddWithValue("@Health", HealthTextBox.Text);
                        command.Parameters.AddWithValue("@TeamNumber", TeamNumberTextBox.Text);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Data successfully recorded", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        con.Close();
                        FirstNameTextBox.Clear();
                        LastNameTextBox.Clear();
                        PositionTextBox.Clear();
                        PlayerOvrRatingTextBox.Clear();
                        ClubTextBox.Clear();
                        ClubIdTextbox.Clear();
                        HealthTextBox.Clear();
                        TeamNumberTextBox.Clear();
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
            try
            {
                if (MessageBox.Show("Do you want to update the data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string str = "UPDATE PlayerProfile SET FirstName = @FirstName, LastName = @LastName, Position = @Position, PlayerOvrRating = @PlayerOvrRating, Club = @Club, Clubid = @Clubid, Health = @Health, TeamNumber = @TeamNumber WHERE PlayerID = @PlayerID";
                    SqlCommand command = new SqlCommand(str, con);
                    command.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text);
                    command.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                    command.Parameters.AddWithValue("@Position", PositionTextBox.Text);
                    command.Parameters.AddWithValue("@PlayerOvrRating", PlayerOvrRatingTextBox.Text);
                    command.Parameters.AddWithValue("@Club", ClubTextBox.Text);
                    command.Parameters.AddWithValue("@Clubid", ClubIdTextbox.Text);
                    command.Parameters.AddWithValue("@Health", HealthTextBox.Text);
                    command.Parameters.AddWithValue("@TeamNumber", TeamNumberTextBox.Text);
                    command.Parameters.AddWithValue("@PlayerID", PlayerIdTextBox.Text);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Data updated successfully", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();
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
            try
            {
                if (MessageBox.Show("Do you want to delete the data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string str = "DELETE FROM PlayerProfile WHERE PlayerID = @PlayerID";
                    SqlCommand cmd = new SqlCommand(str, con);
                    cmd.Parameters.AddWithValue("@PlayerID", PlayerIdTextBox.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data deleted successfully", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        FirstNameTextBox.Text = dr["FirstName"].ToString();
                        LastNameTextBox.Text = dr["LastName"].ToString();
                        PositionTextBox.Text = dr["Position"].ToString();
                        PlayerOvrRatingTextBox.Text = dr["PlayerOvrRating"].ToString();
                        ClubTextBox.Text = dr["Club"].ToString();
                        ClubIdTextbox.Text = dr["Clubid"].ToString();
                        HealthTextBox.Text = dr["Health"].ToString();
                        TeamNumberTextBox.Text = dr["TeamNumber"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Sorry, the player " + SearchTextBox.Text + " is not available.", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        SearchTextBox.Text = "";
                        PlayerIdTextBox.Clear();
                        FirstNameTextBox.Clear();
                        LastNameTextBox.Clear();
                        PositionTextBox.Clear();
                        PlayerOvrRatingTextBox.Clear();
                        ClubTextBox.Clear();
                        ClubIdTextbox.Clear();
                        HealthTextBox.Clear();
                        TeamNumberTextBox.Clear();
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

        private void PlayersDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow >= 0)
            {
                DataGridViewRow row = PlayersDataGridView.Rows[indexRow];
                PlayerIdTextBox.Text = row.Cells[0].Value.ToString();
                FirstNameTextBox.Text = row.Cells[1].Value.ToString();
                LastNameTextBox.Text = row.Cells[2].Value.ToString();
                PositionTextBox.Text = row.Cells[3].Value.ToString();
                PlayerOvrRatingTextBox.Text = row.Cells[4].Value.ToString();
                ClubTextBox.Text = row.Cells[5].Value.ToString();
                ClubIdTextbox.Text = row.Cells[6].Value.ToString();
                HealthTextBox.Text = row.Cells[7].Value.ToString();
                TeamNumberTextBox.Text = row.Cells[8].Value.ToString();
            }
        }

        private void RefreshDatabase()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    string str = "SELECT * FROM PlayerProfile";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    PlayersDataGridView.DataSource = new BindingSource(dt, null);
                }
                PlayersDataGridView.ForeColor = Color.Black;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Add_Players_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
                {
                    string str = "SELECT * FROM PlayerProfile";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    PlayersDataGridView.DataSource = new BindingSource(dt, null);
                }
                PlayersDataGridView.ForeColor = Color.Black;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
