using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tttGame
{
    public partial class Win_Dialog : Form
    {
        public Win_Dialog(string winnerName, int numOfTurns)
        {
            InitializeComponent();
            Win_Message.Text = "Player " + winnerName + " has won in " + numOfTurns + " turns!";
        }

        private void Win_Dialog_Load(object sender, EventArgs e)
        {

        }

        private void Win_Message_Click(object sender, EventArgs e)
        {

        }

        private void btnAgain_Click(object sender, EventArgs e)
        {
            Console.WriteLine("User decided to play another game");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Console.WriteLine("User decided to quit");
            this.Close();
        }
    }
}
