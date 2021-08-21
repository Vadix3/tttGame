using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using tttGame.Models;

namespace tttGame
{
    public partial class Start_Dialog : Form
    {
        private Players p;
        private bool waiting = false;
        private HttpClient client;
        private int SelectedEntry = -1;
        private string GamesStr = ""; // a string of the players games
        private string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\batel\Documents\vsprojects\tttGame\MYDB.mdf;Integrated Security=True";


        public Start_Dialog(HttpClient client, Players temp)
        {
            InitializeComponent();
            this.p = temp;
            this.client = client;
            GetAllGameFromServer();
        }

        private void startGame_Click(object sender, EventArgs e)
        {
            Game_Form dialog = new Game_Form(p, 0, new GameHistory(), false);
            dialog.ShowDialog();
        }

        private void ReplayGame_Click(object sender, EventArgs e)
        {
            GameServerResponse[] res = JsonConvert.DeserializeObject<GameServerResponse[]>(GamesStr);

            if (SelectedEntry != -1)
            {

                int selectedGame = res[SelectedEntry].id;

                Console.WriteLine("Selected game = " + selectedGame);

                Get_game_history(selectedGame);
            }
        }

        private async void GetAllGameFromServer()
        {

            // send the player to the server
            string path = "api/TblGames/player/" + p.id;

            Console.WriteLine("Getting games from server for: " + path);


            var response = client.GetAsync(path).Result;

            if (response.IsSuccessStatusCode)
            {
                // receive the player's play list 
                var responseString = await response.Content.ReadAsStringAsync();
                GamesStr = responseString;
                Console.WriteLine("Success: " + responseString);

                UpdateListBox(responseString);

            }
            else
            {
                Console.WriteLine("Error: " + response.ReasonPhrase);
            }
        }

        private void UpdateListBox(string responseString)
        {
            GamesStr = responseString;
            Console.WriteLine(responseString);
            Game[] g = JsonConvert.DeserializeObject<Game[]>(responseString);
            for (int i = 0; i < g.Length; i++)
            {
                listBox1.Items.Add(g[i].date);
            }

        }

        private void Start_Dialog_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome " + p.name + " " + p.last_name + "!";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedEntry = listBox1.SelectedIndex;
        }

        /** A method to extract the game history from database*/
        private void Get_game_history(int selectedGame)
        {
            Console.WriteLine("Getting history of: " + selectedGame);
            string query = "SELECT * FROM TblGames WHERE Id = '" + selectedGame + "'";
            Console.WriteLine("query = " + query);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    query, connection);
                connection.Open();


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Reading:");
                            GameHistory TempGame = new GameHistory(Convert.ToInt32(reader["Id"].ToString()), reader["Turns"].ToString());
                            Console.WriteLine("Game = " + TempGame.id + " Moves = " + TempGame.moves);

                            if (waiting)
                            {
                                // wait 3 seconds

                                Game_Form dialog = new Game_Form(p, 1, TempGame, true);
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    Console.WriteLine("listener result ok");
                                }
                                else
                                {
                                    Console.WriteLine("listener Result not ok");
                                }

                            }
                            else
                            {
                                Game_Form dialog = new Game_Form(p, 1, TempGame, false);
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    Console.WriteLine("listener result ok");
                                }
                                else
                                {
                                    Console.WriteLine("listener Result not ok");
                                }

                            }
                        }
                    }


                    else
                    {
                        Console.WriteLine("Error");
                    }
                }
                connection.Close();
            }
        }


        void MyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("Form closed");
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                waiting = true;
            }
            else if (!checkBox1.Checked)
            {
                waiting = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
