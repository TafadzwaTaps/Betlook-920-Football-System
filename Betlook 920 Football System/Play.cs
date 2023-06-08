using Betlook_920_Football_System;
using System;
using System.Windows.Forms;

namespace BetLook_920_Football
{
    public partial class Play : Form
    {
        public Play()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Hide();
            Play_Match play_Match = new Play_Match();
            play_Match.Show();  
        }

        private void IconButton9_Click(object sender, EventArgs e)
        {
            Hide();
            Wallet wallet = new Wallet();
            wallet.Show();
        }

        private void IconButton12_Click(object sender, EventArgs e)
        {
            Hide();
            Tournament tournament = new Tournament();
            tournament.Show();
        }

        private void IconButton13_Click(object sender, EventArgs e)
        {
            Hide();
            Play play = new Play();
            play.Show();
        }

        private void IconButton8_Click(object sender, EventArgs e)
        {
            Select_Teams select_Teams = new Select_Teams();
            select_Teams.Show();

        }

        private void IconButton11_Click(object sender, EventArgs e)
        {
            Hide();
            Add_Players add_Players = new Add_Players();
            add_Players.Show();
        }

        private void IconButton10_Click(object sender, EventArgs e)
        {
            Hide();
            PlayersStats playersStats = new PlayersStats();
            playersStats.Show();
        }

        private void IconButton14_Click(object sender, EventArgs e)
        {
            Hide();
            HomeSelection homeSelection = new HomeSelection();
            homeSelection.Show();
        }
    }
}
