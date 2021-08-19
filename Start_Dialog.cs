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
        private Players p;
        

        public Start_Dialog(Players temp)
        {
            InitializeComponent();
            this.p = temp;
        }

        private void startGame_Click(object sender, EventArgs e)
        {
            Console.WriteLine(p.ToString());
            Game_Form dialog = new Game_Form(p);
            dialog.ShowDialog();
        }

        private void replayGame_Click(object sender, EventArgs e)
        {

        }

        private void Start_Dialog_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome " + p.name;
        }
    }
}
