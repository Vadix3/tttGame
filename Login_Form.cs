using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace tttGame
{
    public partial class Login_Form : Form
    {

        //       public static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\batel\Documents\vsprojects\tttGame\MYDB.mdf;Integrated Security=True";
       
       
       //       private  SqlConnection connection = new SqlConnection(connectionString);
       
        
        //            string query = "SELECT * FROM TblStudents";
      
        
        //                   using (SqlConnection connection = new SqlConnection(connectionString))
        //           {
        //               SqlCommand command = new SqlCommand(query, connection);
        //   connection.Open();
        //               using (SqlDataReader reader = command.ExecuteReader())
        //               {
        //                   Console.write(reader);
        //               }
        //           }


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

        private void Login_Form_Load(object sender, EventArgs e)
        {

        }
    }
}
