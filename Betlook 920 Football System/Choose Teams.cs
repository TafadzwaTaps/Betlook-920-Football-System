using BetLook_920_Football;
using FontAwesome.Sharp;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Betlook_920_Football_System
{
    public partial class Choose_Teams : Form
    {
        string User = Login_Form.Username;
        public Choose_Teams()
        {
            InitializeComponent();
        }

        public static string ChosenTeam;
        public static Image ChosenImage;
        public static string SelectedClubID;

        private void Choose_Teams_Load(object sender, EventArgs e)
        {
            DataTable teamsTable = GetTeamsFromDatabase();

            // Bind the teams to the ComboBox
            ComboBoxTeams.DisplayMember = "Club";
            ComboBoxTeams.ValueMember = "Clubid";
            ComboBoxTeams.DataSource = teamsTable;
        }

        private DataTable GetTeamsFromDatabase()
        {
            DataTable teamsTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Betlook;Integrated Security=True"))
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

        private void ShowForm()
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

        private void SelectButton_Click(object sender, EventArgs e)
        {
            if (ComboBoxTeams.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)ComboBoxTeams.SelectedItem;
                string selectedTeam = selectedRow["Club"].ToString();

                // Store the selected ClubID in the SelectedClubID variable
                SelectedClubID = selectedRow["Clubid"].ToString();

                switch (selectedTeam)
                {
                    case "Manchester City":
                        ChosenTeam = "Manchester City";
                        ChosenImage = Properties.Resources.Manchester_City_FC_badge;
                        ShowForm();
                        break;
                    case "Manchester United":
                        ChosenTeam = "Manchester United";
                        ChosenImage = Properties.Resources.manchester_united_f_c_premier_league_logo_football_premier_league_emblem_text_thumbnail;
                        ShowForm();
                        break;
                    case "Arsenal":
                        ChosenTeam = "Arsenal";
                        ChosenImage = Properties.Resources.Arsenal_FC_logo;
                        ShowForm();
                        break;
                    case "Liverpool":
                        ChosenTeam = "Liverpool";
                        ChosenImage = Properties.Resources.Liverpool_FC;
                        ShowForm();
                        break;
                    case "Chelsea":
                        ChosenTeam = "Chelsea";
                        ChosenImage = Properties.Resources.Chelsea_FC_svg;
                        ShowForm();
                        break;
                    case "Everton":
                        ChosenTeam = "Everton";
                        ChosenImage = Properties.Resources.Everton_Logo;
                        ShowForm();
                        break;
                    case "Leicster City":
                        ChosenTeam = "Leicster City";
                        ChosenImage = Properties.Resources.leicester_city_fc_logo_FD9C3CA26E_seeklogo_com;
                        ShowForm();
                        break;
                    case "Tottenham":
                        ChosenTeam = "Tottenham";
                        ChosenImage = Properties.Resources.Tottenham_Logo;
                        ShowForm();
                        break;
                    default:
                        MessageBox.Show("Please Select A Team", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
            }
            else
            {
                MessageBox.Show("No selected team", "Important Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
