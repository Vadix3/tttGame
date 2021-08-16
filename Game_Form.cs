using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tttGame.Models;

namespace tttGame
{
    public partial class Game_Form : Form
    {
        public  const int SIZE = 5;

        private static HttpClient client = new HttpClient();

        private bool isMyTurn = true; // my turn flag
        private int turns = 0; // how many turns were played (max 25 no winner game over)
        private GameBoard game = new GameBoard(); // The game board

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
                }
            }

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Console.WriteLine(game.gameMatrix[i, j].Button);
                }
            }
        }
        public Game_Form()
        {
            InitializeComponent();
            Init_Game_Matrix();
            Show_win_dialog('O');
        }

        private void Game_Form_Load(object sender, EventArgs e)
        {
            client.BaseAddress = new Uri("https://localhost:44362/");

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


            // First axis

            while (i != -1 && j != SIZE)
            { // up right
                Console.WriteLine("up right");
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
                if (game.gameMatrix[i, cell_index[1]].Shape == current_shape)
                {
                    counter++;
                }
            }

            counter--; // duplicate count of current cell

            for (int i = cell_index[1]; i >= 0; i--) // check until the start
            {
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

        /** A method that will check if there is a diagonal win*/
        private bool Check_row_win(int[] cell_index)
        {
            bool win = false;
            char current_shape = game.gameMatrix[cell_index[0], cell_index[1]].Shape;
            int counter = 0; // a counter to see how many similar shapes are there

            for (int i = cell_index[1]; i < SIZE; i++) // check until the end
            {
                if (game.gameMatrix[cell_index[0], i].Shape == current_shape)
                {
                    counter++;
                }
            }

            counter--; // duplicate count of current cell

            for (int i = cell_index[1]; i >= 0; i--) // check until the start
            {
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
                game.gameMatrix[cell_index[0], cell_index[1]].Shape = 'X';
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
                Disable_Buttons();
                Server_move();
            }
        }

        /** A method that will send the board to the server and the server will make a play, and return the board*/
        private void Server_move()
        {
            Console.WriteLine("Server move");


            
            // send the board to the server
            // receive the board with the play
        }

        private void Win_Scenario(char winner)
        {
            Console.WriteLine(winner + " is the winner!");

            Disable_Buttons();

            Show_win_dialog(winner);
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
                Reset_Game();
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
                }
            }
        }
    }
}
