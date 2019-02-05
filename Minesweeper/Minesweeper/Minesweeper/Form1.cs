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
        int sizeX = 9, sizeY = 9, bombCount = 2;
        Button[,] btn = new Button[30, 16];
        int[,] board = new int[30, 16];
        bool firstPress, gameEnded = false;

        public frmGame()
        {

            InitializeComponent();
            Size = new System.Drawing.Size(18 + (35 * sizeX), 70 + (35 * sizeY));

            for (int x = 0; x < 30; x++)         // Loop for x
            {
                for (int y = 0; y < 16; y++)     // Loop for y
                {
                    btn[x, y] = new Button();
                }
            }
            startGame();
        }

        void btnEvent_Click(object sender, EventArgs e)
        {
            int x = ((((Button)sender).Location.X) / 35);
            int y = ((((Button)sender).Location.Y) / 35);

            if (bombCheck(x, y) > 0 && !firstPress)
                newGame();
            else
            {
                firstPress = true;
                openBox(x, y);
            }
        }

        void startGame()
        {
            firstPress = false;
            gameEnded = false;

            for (int x = 0; x < sizeX; x++)         
            {
                for (int y = 0; y < sizeY; y++)     
                {
                    board[x, y] = 0;
                    btn[x, y].Enabled = true;
                    btn[x, y].SetBounds(35 * x, 30 + (35 * y), 35, 35);
                    btn[x, y].BackColor = Color.BlueViolet;
                    btn[x, y].Text = "";
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);
                }
            }
            placeBombs(bombCount);
        }

        void newGame()
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    Controls.Remove(btn[x, y]);
                }
            }

            startGame();
        }

        void openBox(int x, int y)
        {
            int noOfBombs = 0;

            if (board[x, y] < 2)
            {
                board[x, y] += 2;
                btn[x, y].BackColor = Color.LightBlue;

                if (board[x, y] % 2 == 1)
                {
                    endGame(1);
                }
                else
                {
                    noOfBombs = bombCheck(x, y);
                    
                    if (noOfBombs == 0)
                    {
                        openSurroundingBoxes(x, y);
                    }
                    else
                    {

                        btn[x, y].Text = Convert.ToString(noOfBombs);
                    }
                }
            }
            checkWin();
        }

        void openSurroundingBoxes(int x, int y)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (x + i >= 0 && x + i < sizeX && y + j >= 0 && y + j < sizeY)
                    {
                        if (board[x + i, y + j] < 2)
                        {
                            openBox(x + i, y + j);
                        }
                    }
                }
            }
        }

        int bombCheck(int x, int y)
        { 
            int bombClose = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (x + i >= 0 && x + i < sizeX && y + j >= 0 && y + j < sizeY)
                    { 
                        if (board[x + i, y + j] % 2 == 1)
                        {
                            bombClose++;
                        }
                    }
                }
            }
            return bombClose;
        }

        int placeBombs(int number)
        {
            Random rnd = new Random();
            int x = 0, y = 0;

            for (int i = 0; i < number; i++)
            {
                do {
                    x = rnd.Next(0, sizeX - 1);
                    y = rnd.Next(0, sizeY - 1);

                } while (board[x,y] % 2 == 1);

                board[x, y]++;
            }

            return 0;
        }

        void checkWin()
        {
            int unopenedBoxes = 0;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (board[x, y] < 2 && gameEnded == false)
                    {
                        unopenedBoxes++;
                    }
                }
            }
            
            if (unopenedBoxes == bombCount)
                endGame(0);
        }

        void endGame(int state)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (board[x, y] % 2 == 1)
                    {
                        btn[x, y].BackColor = Color.Red;
                    }

                    btn[x, y].Enabled = false;
                }
            }

            gameEnded = true;

            if (state == 0)
                MessageBox.Show("You have won the game!", "Congratulations");
            else if (state == 1)
                MessageBox.Show("You have hit a bomb!", "Unlucky");
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeX = 9;
            sizeY = 9;
            bombCount = 10;

            Size = new System.Drawing.Size(18 + (35 * sizeX), 70 + (35 * sizeY));
            newGame();
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeX = 16;
            sizeY = 16;
            bombCount = 40;

            Size = new System.Drawing.Size(18 + (35 * sizeX), 70 + (35 * sizeY));
            newGame();
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeX = 30;
            sizeY = 16;
            bombCount = 99;

            Size = new System.Drawing.Size(18 + (35 * sizeX), 70 + (35 * sizeY));
            newGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {  //REQUIRED
        }
    }
}