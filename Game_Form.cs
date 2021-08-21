using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using tttGame.Models;

namespace tttGame
{
    public partial class Game_Form : Form
    {
        public const int SIZE = 5;
        public const string turnPath = "api/GameBoards/test"; // the path for the servers turn

        private static HttpClient client = new HttpClient();
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\batel\Documents\vsprojects\tttGame\MYDB.mdf;Integrated Security=True";

        

        private bool isMyTurn = true; // my turn flag
        private int turns = 0; // how many turns were played (max 25 no winner game over)
        private GameBoard game = new GameBoard(); // The game board
        private Players player;
        private int option;
        private GameHistory history = new GameHistory();
        private string gameData = ""; // A string representation of the plays that were made. 
        //For example: Xa1/Oa2 = Player x selected a1 square, player O selected a2 square


        private bool Waiting = false; // a boolean variable that indicates if the user decided to wait 3 seconds
        // between plays

        /** A method to initialize the game matrix*/
        private void Init_Game_Matrix()
        {
            game.gameMatrix = new Square[SIZE, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    game.gameMatrix[i, j] = new Square();
                    string btnName = "";

                    switch (i)
                    {
                        case 0:
                            btnName = "a" + j;
                            break;
                        case 1:
                            btnName = "b" + j;
                            break;
                        case 2:
                            btnName = "c" + j;
                            break;
                        case 3:
                            btnName = "d" + j;
                            break;
                        case 4:
                            btnName = "e" + j;
                            break;
                    }
                    game.gameMatrix[i, j].Button = btnName;
                    game.gameMatrix[i, j].Shape = ' ';
                }
            }

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Console.Write(game.gameMatrix[i, j].Button + " ");
                }
                Console.WriteLine("");
            }
        }
        public Game_Form(Players players, int options, GameHistory history, bool waiting)
        {
            Console.WriteLine("Game_Form");
            InitializeComponent();
            Init_Game_Matrix();
            this.player = players;
            this.option = options;
            this.history = history;
            this.Waiting = waiting;

            Console.WriteLine("Got history in game form = "+history.moves);
        }

        private void Game_Form_Shown(object sender, EventArgs e) {
            Console.WriteLine("Form shown");
            if (option == 1)
            {
                ReplayGame();
            }
        }

        private void Game_Form_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Game_form_load");
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("https://localhost:5001/");
            }


        }

        private void ReplayGame()
        {
            Console.WriteLine("Replay game: "+history.moves);
            string omittedMoves = history.moves.Remove(history.moves.Length - 1, 1);
            string[] moves = omittedMoves.Split('/'); // split the string into moves with '/' delimiter
            ArrayList movesArray = new ArrayList();
            foreach (var move in moves)
            {
                char shape = move[0];
                string btn = move[1].ToString() + move[2].ToString() + "";
                Square temp = new Square();
                temp.Shape = shape;
                temp.Button = btn;
                movesArray.Add(temp);
            }

            _ = Play_auto_gameAsync(movesArray);
            Disable_Buttons();
        }

        /** A method to play an automatic game with a given moves set*/
        private async Task Play_auto_gameAsync(ArrayList movesArray)
        {
            Disable_Buttons();
            Console.WriteLine("Playing auto game");
            foreach(Square move in movesArray){
                await MakeMove(move);
            }
        }

        private async Task MakeMove(Square move)
        {
            Console.WriteLine("Playing " + move.Shape + " to " + move.Button);
            Button btn = (Button)this.Controls[move.Button];
            btn.Text = move.Shape.ToString();
            int milliseconds = 200;
            if (move.Shape == 'X')
            {
                btn.BackColor = Color.Blue;
            }
            else {
                btn.BackColor = Color.Red;
            }
            if (Waiting)
            {
                Console.WriteLine("User selected waiting mode");
                milliseconds = 3000;
            }

            //sleep between
            Thread.Sleep(milliseconds);
            Application.DoEvents();
        }


        /** A method to update the selected cell with the proper shape (X or O)*/
        private int[] Get_cell_index(string btnName)
        {
            int[] res = { 0, 0 };


            switch (btnName[0])
            {
                case 'a':
                    res[0] = 0;
                    break;
                case 'b':
                    res[0] = 1;
                    break;
                case 'c':
                    res[0] = 2;
                    break;
                case 'd':
                    res[0] = 3;
                    break;
                case 'e':
                    res[0] = 4;
                    break;
            }

            res[1] = Convert.ToInt32(btnName[1].ToString()) - 1;

            return res;
        }

        /** A method to check if a win combination has been made
            - Check row condition (4 same shapes in a row)
            - Check column condition (4 same shapes in a column)
            - Check diagonal condition (4 same shapes diagonally)
         */
        private bool Check_win(int[] cell_index)
        {
            Console.WriteLine("Checking win for: " + cell_index[0] + "," + cell_index[1]);

            if (turns == 25)
            {
                Console.WriteLine("Game over!");
                //TODO: send the game to the server 
            }

            bool row_win = Check_row_win(cell_index);
            bool col_win = Check_col_win(cell_index);
            bool dia_win = Check_dia_win(cell_index);

            if (row_win || col_win || dia_win)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /** A method that will iterate each row and will check if there is a row win*/
        private bool Check_dia_win(int[] cell_index)
        {
            bool win = false;
            char current_shape = game.gameMatrix[cell_index[0], cell_index[1]].Shape;
            int counter1 = 0; // a counter to see how many similar shapes are there (first axis)
            int counter2 = 0; // a counter to see how many similar shapes are there (second axis)


            int i = cell_index[0];
            int j = cell_index[1];

            bool gap = false; // a boolean variable that will become true if there is a gap in the line

            // First axis

            while (i != -1 && j != SIZE)
            { // up right
                Console.WriteLine("up right");
                if (counter1 == 2 && game.gameMatrix[i, j].Shape != current_shape)
                {
                    return false;
                }
                if (game.gameMatrix[i, j].Shape == current_shape)
                {
                    counter1++;
                }
                i--;
                j++;
            }

            i = cell_index[0];
            j = cell_index[1];
            counter1--; // duplicate cell count

            while (i != SIZE && j != -1)
            { // down left
                Console.WriteLine("down left");
                if (counter1 == 2 && game.gameMatrix[i, j].Shape != current_shape)
                {
                    return false;
                }
                if (game.gameMatrix[i, j].Shape == current_shape)
                {
                    counter1++;
                }
                i++;
                j--;
            }


            i = cell_index[0];
            j = cell_index[1];

            // Second axis
            while (i != SIZE && j != SIZE)
            { // down right
                Console.WriteLine("down right");
                if (counter1 == 2 && game.gameMatrix[i, j].Shape != current_shape)
                {
                    return false;
                }
                if (game.gameMatrix[i, j].Shape == current_shape)
                {
                    counter2++;
                }
                i++;
                j++;
            }

            i = cell_index[0];
            j = cell_index[1];
            counter2--; // duplicate cell count

            while (i != -1 && j != -1)
            { // down left
                Console.WriteLine("down left");
                if (counter1 == 2 && game.gameMatrix[i, j].Shape != current_shape)
                {
                    return false;
                }
                if (game.gameMatrix[i, j].Shape == current_shape)
                {
                    counter2++;
                }
                i--;
                j--;
            }


            if (counter1 >= 4 || counter2 >= 4)
            {
                win = true;
            }
            return win;
        }

        /** A method that will iterate each column and check if there is a column win*/
        private bool Check_col_win(int[] cell_index)
        {
            bool win = false;
            char current_shape = game.gameMatrix[cell_index[0], cell_index[1]].Shape;
            int counter = 0; // a counter to see how many similar shapes are there

            for (int i = cell_index[1]; i < SIZE; i++) // check until the end
            {
                if (counter == 2 && game.gameMatrix[i, cell_index[1]].Shape != current_shape)
                {
                    return false;
                }
                if (game.gameMatrix[i, cell_index[1]].Shape == current_shape)
                {
                    counter++;
                }
            }

            counter--; // duplicate count of current cell

            for (int i = cell_index[1]; i >= 0; i--) // check until the start
            {
                if (counter == 2 && game.gameMatrix[i, cell_index[1]].Shape != current_shape)
                {
                    return false;
                }
                if (game.gameMatrix[i, cell_index[1]].Shape == current_shape)
                {
                    counter++;
                }
            }

            if (counter >= 4)
            { // if we counted more than 4
                win = true;
                Console.WriteLine("We have a win at col: " + cell_index[1]);
            }

            return win;
        }

        /** A method that will check if there is a row win*/
        private bool Check_row_win(int[] cell_index)
        {
            bool win = false;
            char current_shape = game.gameMatrix[cell_index[0], cell_index[1]].Shape;
            int counter = 0; // a counter to see how many similar shapes are there

            for (int i = cell_index[1]; i < SIZE; i++) // check until the end
            {
                if (counter == 2 && game.gameMatrix[cell_index[0], i].Shape != current_shape)
                {
                    return false;
                }
                if (game.gameMatrix[cell_index[0], i].Shape == current_shape)
                {
                    counter++;
                }
            }

            counter--; // duplicate count of current cell

            for (int i = cell_index[1]; i >= 0; i--) // check until the start
            {
                if (counter == 2 && game.gameMatrix[cell_index[0], i].Shape != current_shape)
                {
                    return false;
                }
                if (game.gameMatrix[cell_index[0], i].Shape == current_shape)
                {
                    counter++;
                }
            }

            if (counter >= 4)
            { // if we counted more than 4
                win = true;
                Console.WriteLine("We have a win at row: " + cell_index[0]);
            }

            return win;
        }

        /** A click listener*/
        private void Button_click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            Console.WriteLine("Pressed button: " + btn.Name);

            int[] cell_index = Get_cell_index(btn.Name);

            Console.WriteLine("x = " + cell_index[0] + " y = " + cell_index[1]);

            if (isMyTurn)
            {
                btn.Text = "X";
                btn.BackColor = Color.Blue;
                game.gameMatrix[cell_index[0], cell_index[1]].Shape = 'X';
                gameData = gameData + "X" + btn.Name + "/";
            }
            else
            {

                btn.Text = "O";
                game.gameMatrix[cell_index[0], cell_index[1]].Shape = 'O';
            }

            isMyTurn = !isMyTurn;
            btn.Enabled = false;
            turns += 1;

            if (Check_win(cell_index))
            { // I won
                Win_Scenario('X');
            }
            else
            {
                //Disable_Buttons();
                Server_move();
            }
        }

        /** A method that will send the board to the server and the server will make a play, and return the board*/
        private async void Server_move()
        {
            // a json representation of the matrix
            string jsonMatrix = Convert_matrix_to_json();

            // send the board to the server
            string path = $"api/GameBoards/test";

            var response = client.PostAsJsonAsync(turnPath, jsonMatrix).Result;

            //HttpResponseMessage response = await client.PostAsJsonAsync(path, jsonMatrix);
            if (response.IsSuccessStatusCode)
            {

                // receive the board with the play
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Success: " + responseString);

                Square[,] temp = Convert_json_to_matrix(responseString);

                game.gameMatrix = temp;

            }
            else
            {
                Console.WriteLine("Error: " + response.ReasonPhrase);
            }
        }



        /** A method to convert the current board to a json object*/
        private string Convert_matrix_to_json()
        {
            char[,] temp = new char[SIZE, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    temp[i, j] = game.gameMatrix[i, j].Shape;
                }
            }

            var demoJsonMatrix = JsonConvert.SerializeObject(temp);
            //convert json to object
            Console.WriteLine("Json matrix = " + demoJsonMatrix);
            
            return demoJsonMatrix;
        }


        /** A method to convert the http response to a game board and update the board*/
        private Square[,] Convert_json_to_matrix(string content)
        {
            char[,] plain = JsonConvert.DeserializeObject<char[,]>(content);
            Square[,] temp = game.gameMatrix;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (!game.gameMatrix[i, j].Shape.Equals(plain[i, j]))
                    {
                        Console.WriteLine($"the original = {game.gameMatrix[i, j].Shape} , the temp =  {plain[i, j]}");
                        temp[i, j].Shape = plain[i, j]; // copy the shapes to the game board
                        if (plain[i, j] != ' ')
                        {

                            string btnName = temp[i, j].Button;
                            int c = Convert.ToInt32(btnName[1].ToString()) + 1;
                            string finalName = btnName[0] + c.ToString();
                            Console.WriteLine(finalName);
                            Button btn = (Button)this.Controls[finalName];
                            btn.Text = plain[i, j].ToString();
                            btn.Enabled = false;
                            isMyTurn = true;
                            btn.BackColor = Color.Red;
                            gameData = gameData + "O" + finalName + "/";
                            int[] cell_index = Get_cell_index(finalName);
                            if (Check_win(cell_index))
                            {
                                Win_Scenario('O');

                            }

                            //sleep
                            int milliseconds = 500;
                            Thread.Sleep(milliseconds);
                        }
                    }
                }
            }

            return temp;
        }

        private void Win_Scenario(char winner)
        {
            Console.WriteLine(winner + " is the winner!");
            Console.WriteLine(gameData);
            Disable_Buttons();

            //TODO : send the game to the server
            //game id 
            //user id 
            //num of turn
            //Date time
            //User win yes , no 
            string w = "";
            if (winner.Equals("X"))
            { w = "No"; }
            else { w = "Yes"; }
            Game game = new Game(player.id, turns, DateTime.Now, w);
            sendGameToServer(game);
            Add_game_to_user();
            Show_win_dialog(winner);
        }
        
        /** A method that will update the number of games for the current user*/
        private void Add_game_to_user()
        {
            Console.WriteLine("Adding game to: "+player.id);
            string tempPath = "api/TblPlayers/addgame/"+player.id;

            var response = client.GetAsync(tempPath).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("SUCCESS: "+response.ReasonPhrase);
            }
            else
            {
                Console.WriteLine("ERROR: " + response.ReasonPhrase);
            }
        }

        private void Save_game_to_local_db(int id)
        {
            Console.WriteLine("Saving game to local db");

            string query = "INSERT INTO TblGames (Id, Turns) " +
                "VALUES('" + id + "','" + gameData + "')";
            putData(query);
        }

        private async void sendGameToServer(Game game)
        {
            string gamePATH = "api/TblGames";
            var response = client.PostAsJsonAsync(gamePATH, game).Result;

            if (response.IsSuccessStatusCode)
            {
                // receive the board with the play
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Success: " + responseString);

                dynamic stuff = JObject.Parse(responseString);

                int id = stuff["id"];

                Console.WriteLine("Got game id from server " + id);

                Save_game_to_local_db(id);
                /*                MessageBox.Show("Post on server: " + response.ReasonPhrase);
                */
            }
            else
            {
                /*                MessageBox.Show("Error: " + response.ReasonPhrase);
                */
                Console.WriteLine("Error: " + response.ReasonPhrase);
            }
        }


        /** A method to disable the buttons*/
        private void Disable_Buttons()
        {
            foreach (Control c in Controls) // disable all the other buttons
            {
                if (c is Button b)
                {
                    b.Enabled = false;
                }
            }
        }

        /** A method to show the user a finish dialog with the option to quit and play again*/
        private void Show_win_dialog(char winner)
        {
            Console.WriteLine("Showing win dialog with winner: " + winner);
            Win_Dialog dialog = new Win_Dialog(winner.ToString(), turns);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("result ok");
                Reset_Game();
            }
            else {
                Console.WriteLine("Result not ok");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /** A method to reset the game*/
        private void Reset_Game()
        {
            //enable buttons
            //reset matrix
            //reset turns
            isMyTurn = true;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    game.gameMatrix[i, j] = new Square();
                }
            }
            turns = 0;
            foreach (Control c in Controls) // disable all the other buttons
            {
                if (c is Button b)
                {
                    b.Enabled = true;
                    b.Text = "";
                    b.BackColor = SystemColors.ButtonFace;
                }
            }
        }

        private void getData(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {

                    }
                    else
                    {

                    }
                }
                connection.Close();
            }
        }

        private void putData(string queryString)
        {
            Console.WriteLine("Putting data: " + queryString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    queryString, connection);

                try
                {
                    connection.Open();
                    Console.WriteLine("Connection is open");
                    command.ExecuteNonQuery();
                    connection.Close();
                    //MessageBox.Show("Post on Local DB");
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Error writing to local db: " + e.Message);
                    //MessageBox.Show("Error Post on Local DB: ");
                }
                connection.Close();
            }

        }
    }
}

