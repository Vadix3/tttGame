using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace tttGame
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            string UserName = userBox.Text;
            string Password = passwordBox.Text;
            // id 
            // 
            if(checkFromServer(UserName , Password))
            {
                Start_Dialog dialog = new Start_Dialog();
                dialog.ShowDialog();
                this.Close();
            }
            else { }
        }
       
        private bool checkFromServer(string userName, string password)
        {
            return true;
        }
    }
}
