using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace tttGame
{
    public partial class Start_Dialog : Form
    {
        private Players p;
        private bool waiting = false;
        private HttpClient client;

        public Start_Dialog(HttpClient client,Players temp)
        {
            InitializeComponent();
            this.p = temp;
            this.client = client;
            GetAllGameFromServer();
        }

        private void startGame_Click(object sender, EventArgs e)
        {
            Game_Form dialog = new Game_Form(p,0,"");
            dialog.ShowDialog();
        }

        private void replayGame_Click(object sender, EventArgs e)
        {
            //GetAllGameFromServer();
        }

        private async void GetAllGameFromServer()
        {
         
            // send the player to the server
            string path = "api/TblGames/player/" + p.id;

            var response = client.GetAsync(path).Result;
                
            if (response.IsSuccessStatusCode)
            {
                // receive the player's play list 
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Success: " + responseString);

                UpdateListBox(responseString);

            }
            }

        private void UpdateListBox(string responseString)
        {
            Console.WriteLine(responseString);
           Game[] g =  JsonConvert.DeserializeObject<Game[]>(responseString);
            for (int i = 0; i < g.Length; i++)
            {
                listBox1.Items.Add(g[i].date);
            }
            
        }

        private void Start_Dialog_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome " + p.name ;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(listBox1.SelectedItem.ToString());
            Game_Form dialog = new Game_Form(p, 1,listBox1.SelectedItem.ToString());
            dialog.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                waiting = true;
            }
            else if(!checkBox1.Checked) {
                waiting = false;
            }
        }
    }
}
