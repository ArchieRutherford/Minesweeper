/**
 * Minesweeper
 * 
 * By  Archie Rutherford - 170010226
 * And Benjamin Morrison - 170013039
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{ 
    public partial class frmGame : Form
    {
        int sizeX = 9, sizeY = 9, bombCount = 10;   // Global variables storing the size of grid and number of bombs
        Button[,] btn = new Button[30, 16];         // Button objects being created in the hardest difficulty
        int[,] board = new int[30, 16];             // Board varable stores the state of each square on the board
        bool firstPress, gameEnded = false;         // firstPress stores whether the first press has been taken
                                                    // gameEnded stores if the game has ended yet or not
        // Constructor for the game form
        public frmGame()
        {

            InitializeComponent();

            // This will change the size of the window based on the size of the grid.
            Size = new System.Drawing.Size(18 + (35 * sizeX), 70 + (35 * sizeY));
            
            for (int x = 0; x < 30; x++)        // Loop to initialise all of the buttons        
            {
                for (int y = 0; y < 16; y++)   
                {
                    btn[x, y] = new Button();
                }
            }
            
            startGame();    // Call the function to start the game
        }

        // Event handler which runs when a button is pressed with the mouse
        void btnEvent_Click(object sender, MouseEventArgs e)
        {
            int x = ((((Button)sender).Location.X) / 35);   // Work out the x and y coordinates of
            int y = ((((Button)sender).Location.Y) / 35);   // the buttons based on their location
            
            if (e.Button == MouseButtons.Left)      // If the button pressed is the left click
            {
                if (!firstPress)                    // If this is the first button pressed
                {
                    while (bombCheck(x, y) > 0)     // While the selected box is surrounded          
                        startGame();                // by a bomb, reload the board
                }                   
                
                firstPress = true;                  // Tell the game the first press has been made 
                openBox(x, y);                      // Open the selected box
            }
        }

        // Event handler which runs when the mouse wheel is used while hovering on a box.
        void btnEvent_Wheel(object sender, MouseEventArgs e)
        {
            int x = ((((Button)sender).Location.X) / 35);   // Work out the x and y coordinates of
            int y = ((((Button)sender).Location.Y) / 35);   // the buttons based on their location

            flag(x, y);     // Call the function to place a flag on the box
        }

        // Initialise the game ready to start
        void startGame()
        {
            firstPress = false;     // When the game starts the first press has not been made yet
            gameEnded = false;      // The game has now started

            for (int x = 0; x < sizeX; x++)     // Loop x and y for each box on screen
            {
                for (int y = 0; y < sizeY; y++)     
                {
                    board[x, y] = 0;                                                        // Initialise the state of all boxes
                    btn[x, y].Enabled = true;                                               // Activate the buttons
                    btn[x, y].SetBounds(35 * x, 30 + (35 * y), 35, 35);                     // Set the location and size
                    btn[x, y].BackColor = Color.BlueViolet;                                 // Set the colours
                    btn[x, y].Text = "";                                                    // Make all boxes empty
                    btn[x, y].MouseClick += new MouseEventHandler(this.btnEvent_Click);     // Set the event handlers for
                    btn[x, y].MouseWheel += new MouseEventHandler(this.btnEvent_Wheel);     // Click and Mouse Wheel
                    Controls.Add(btn[x, y]);                                                // Add the buttons to the form
                }
            }
            placeBombs(bombCount);  // Call the function to place the bombs on screen
        }

        // Restart the game
        void newGame()
        {
            for (int x = 0; x < sizeX; x++)         // Loop x and y for each button
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Controls.Remove(btn[x, y]);     // Remove the selected button
                }
            }

            startGame();    // Call the function to start the game
        }

        // Open the box at positon x, y
        void openBox(int x, int y)
        {
            int noOfBombs = 0;  // Counter for number of bombs surrounding the box

            if (board[x, y]%4 < 2 && board[x, y]%8 < 4)     // If the box isn't opened or flagged
            {
                board[x, y] += 2;                           // Set the box to an opened state
                btn[x, y].BackColor = Color.LightBlue;      // Change the colour to look opened

                if (board[x, y] % 2 == 1)   // If the opened box contained a bomb
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer("Bomb+4.wav");
                    player.Play();      // Play a bomb sound effect

                    endGame(1);             // End the game with a loss
                }
                else
                {
                    noOfBombs = bombCheck(x, y);        // Check how many bombs surround it
                    
                    if (noOfBombs == 0)                 // If there are no bombs surrounding it
                    {
                        openSurroundingBoxes(x, y);     // Open all the boxes surrounding it
                    }
                    else
                    {
                        btn[x, y].Text = Convert.ToString(noOfBombs);   // Display how many bombs surround it
                    }
                }
            }

            checkWin();     // Call the function to check whether the game has been won
        }

        // Open all the boxes surrounding the position x, y
        void openSurroundingBoxes(int x, int y)
        {
            for (int i = -1; i < 2; i++)        // Loop i and j from -1 to 1
            {                                 
                for (int j = -1; j < 2; j++)
                {
                    if (x + i >= 0 && x + i < sizeX && y + j >= 0 && y + j < sizeY) // If the selected box is within the grid
                    {
                        if (board[x + i, y + j]%4 < 2)  // If the selected box is unopened
                        {
                            openBox(x + i, y + j);      // Call the function to open the selected box
                        }
                    }
                }
            }
        }

        // Find the number of bombs surrounding a certain tile
        int bombCheck(int x, int y)
        { 
            int bombClose = 0;  // Integer to count the bombs found

            for (int i = -1; i < 2; i++)    // Loop i and j from -1 to 1
            {
                for (int j = -1; j < 2; j++)
                {
                    if (x + i >= 0 && x + i < sizeX && y + j >= 0 && y + j < sizeY) // If the selected box is within the grid 
                    { 
                        if (board[x + i, y + j] % 2 == 1)   // If the selected box contains a bomb
                        {
                            bombClose++;    // Incriment the bomb counter
                        }
                    }
                }
            }

            return bombClose;   // Return the number of bombs found
        }

        // Place n bombs randomly across the board
        void placeBombs(int number)
        {
            Random rnd = new Random();  // Instantiate a random number generator
            int x = 0, y = 0;           // Integers for the x and y coordinates

            for (int i = 0; i < number; i++)    // Loop for the number of bombs wanted
            {
                do {                            
                    x = rnd.Next(0, sizeX - 1); // Generate a random number for 
                    y = rnd.Next(0, sizeY - 1); // the x and y coordinates

                } while (board[x,y] % 2 == 1);  // While the selected box is a bomb

                board[x, y]++;  // Place a bomb at the selected coordinates
            }
        }

        // Mark a box as a flag
        void flag(int x, int y)
        {
            if (board[x, y]%8 < 4 && board[x, y]%4 < 2)     // If the box isn't opened or a flag
            {
                board[x, y] += 4;                           // Flag this box
                btn[x, y].BackColor = Color.Magenta;        // Change the colour of the box
            }
            else if (board[x, y] >= 4)                      // If the box is flagged
            {
                board[x, y] -= 4;                           // Remove the flag from the box
                btn[x, y].BackColor = Color.BlueViolet;     // Change colour back to unopened
            }

        }

        // Check if the game has been won
        void checkWin()
        {
            int unopenedBoxes = 0;      // Counter for the number of unopened boxes on screen 

            for (int x = 0; x < sizeX; x++)                         // Loop x and y for each box on screen
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (board[x, y]%4 < 2 && gameEnded == false)    // If the box is unopened and the game has't ended
                        unopenedBoxes++;                            // Increment the counter
                }
            }
            
            if (unopenedBoxes == bombCount)     // If the number of unopened boxes is the same as 
                endGame(0);                     // the number of bombs then end the game with a win
        }

        // End the game
        void endGame(int state)
        {
            for (int x = 0; x < sizeX; x++)                 // Loop x and y for each box
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (board[x, y] % 2 == 1)               // If this box contains a bomb
                    {
                        btn[x, y].BackColor = Color.Red;    // Turn the bomb red
                    }

                    btn[x, y].Enabled = false;              // Disable the button
                }
            }

            gameEnded = true;   // Set gameEnded to true and the game has now ended

            if (state == 0)                                                     // If the state is 0 (Win)
                MessageBox.Show("You have won the game!", "Congratulations");   // Tell the user they have won
            else if (state == 1)                                                // If the state is 1 (Lose)
                MessageBox.Show("You have hit a bomb!", "Unlucky");             // Tell the user they have lost
        }

        // Event handler for the new game button
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGame();  // Call function to start a new game
        }

        // Event handler for the easy difficulty button
        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeX = 9;          // Set the size of the board to 9 x 9
            sizeY = 9;          
            bombCount = 10;     // Set the number of bombs to 10

            Size = new System.Drawing.Size(18 + (35 * sizeX), 70 + (35 * sizeY));   // Resize the window

            newGame();      // Call the function to start a new game
        }

        // Event handler for the medium difficulty button
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeX = 16;         // Set the size of the board to 16 x 16 
            sizeY = 16;
            bombCount = 40;     // Set the number of bombs to 40

            Size = new System.Drawing.Size(18 + (35 * sizeX), 70 + (35 * sizeY));   // Resize the window

            newGame();      // Call the function to start a new game
        }

        // Display information about the game
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        // Display the instructions for how to play the game
        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Minesweeper", "Instructions");
        }

        // Event handler for the medium difficulty button
        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeX = 30;         // Set the size of the board to 30 x 16
            sizeY = 16;
            bombCount = 99;     // Set the number of bombs to 99

            Size = new System.Drawing.Size(18 + (35 * sizeX), 70 + (35 * sizeY));   // Resize the window

            newGame();      // Call the function to start a new game
        }

        // Event handler for the exit button
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);    // Exit the game
        }

        // Event handler for the loading of the form
        private void Form1_Load(object sender, EventArgs e)
        {  
            // This is required for the program to run
        }
    }
}