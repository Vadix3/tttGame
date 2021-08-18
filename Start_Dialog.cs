using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tttGame
{
    public partial class Start_Dialog : Form
    {
        public Start_Dialog()
        {
            InitializeComponent();
        }

        private void startGame_Click(object sender, EventArgs e)
        {
            Game_Form dialog = new Game_Form();
            dialog.ShowDialog();
        }

        private void replayGame_Click(object sender, EventArgs e)
        {

        }
    }
}
