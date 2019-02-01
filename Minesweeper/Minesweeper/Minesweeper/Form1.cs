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
{   //   EXTEND PREVIOUS TO CREATE A 2-D ARRAY OF BUTTONS (each with number on)
    public partial class game : Form
    {
        static int sizeX = 6, sizeY = 6;
        Button[,] btn = new Button[sizeX, sizeY];       // Create 2D array of buttons
        int[,] board = new int[sizeX, sizeY];

        public game()
        {
            InitializeComponent();
            for (int x = 0; x < sizeX; x++)         // Loop for x
            {
                for (int y = 0; y < sizeY; y++)     // Loop for y
                {
                    board[x,y] = 0;
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(55 * x, 30 + (55 * y), 45, 45);
                    btn[x, y].BackColor = Color.PowderBlue;
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);
                }
            }
            placeBombs(6);
        }
        void btnEvent_Click(object sender, EventArgs e)
        {
            int x = ((((Button)sender).Location.X) / 55);
            int y = ((((Button)sender).Location.Y) / 55);

            openBox(x, y, sender);
        }
       
        void openBox(int x, int y, object sender)
        {
            int noOfBombs = 0;

            if (board[x, y] < 2)
            {
                board[x, y] += 2;
                ((Button)sender).BackColor = Color.Aquamarine;

                if (board[x, y] % 2 == 1)
                {
                    //Trigger game over
                    ((Button)sender).BackColor = Color.Red;
                }
                else
                {
                    noOfBombs = bombCheck(x, y);
                    ((Button)sender).Text = Convert.ToString(noOfBombs);

                    if (noOfBombs == 0)
                    {
                        openSurroundingBoxes(x, y, sender);
                    }

                    // TODO open all open surrounding boxes

                }
            }
        }

        void openSurroundingBoxes(int x, int y, object sender)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (x + i >= 0 && x + i < sizeX && y + j >= 0 && y + j < sizeY)
                    {
                        if (board[x + i, y + j] < 2)
                        {
                            openBox(x + i, y + j, sender);
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


        private void Form1_Load(object sender, EventArgs e)
        {  //REQUIRED
        }
    }
}


