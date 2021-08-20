using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Newtonsoft.Json;
using tttGame.Models;
using System.Net.Http;

namespace tttGame
{
    public partial class Login_Form : Form
    {

        private string PATH = "api/TblPlayers/login";
        private static HttpClient client = new HttpClient();
        public Login_Form()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
           
            string UserName = userBox.Text;
            string Password = passwordBox.Text;
         
            Credentials c = new Credentials(UserName, Password);

             //CheckLoginFromServer(c);

            goToGame(new Players(1,"nathan","amiel","nathan770","password","password","3"));
            
        }

        private async void CheckLoginFromServer(Credentials c)
        {
            var JsonCredential = JsonConvert.SerializeObject(c);
            //convert json to object
            Console.WriteLine("Json matrix = " + JsonCredential);

            var response = client.PostAsJsonAsync(PATH, JsonCredential).Result;

            if (response.IsSuccessStatusCode)
            {

                // receive the board with the play
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Success: " + responseString);

                Players temp = Convert_json_to_player(responseString);

                goToGame(temp);
               
            }
            else
            {
                MessageBox.Show("Error: " + response.ReasonPhrase + " the status code "+response.StatusCode);
                Console.WriteLine("Error: " + response.ReasonPhrase);
            }

        }

        private void goToGame(Players temp)
        {
            Start_Dialog dialog = new Start_Dialog(client,temp);
            dialog.ShowDialog();
            this.Close();
        }

        private Players Convert_json_to_player(string responseString)
        {
            Players players = JsonConvert.DeserializeObject<Players>(responseString);

            return players;
        }

        private bool checkFromServer(string userName, string password)
        {
            return true;
        }

        private void Login_Form_Load(object sender, EventArgs e)
        {
            client.BaseAddress = new Uri("https://localhost:44362/");
        }
    }
}
