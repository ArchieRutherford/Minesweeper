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
        Button[,] btn = new Button[5, 5];       // Create 2D array of buttons
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
                    btn[x, y].SetBounds(55 * x, 30 + 55 * y, 45, 45);
                    btn[x, y].BackColor = Color.PowderBlue;
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);
                }
            }
        }
        void btnEvent_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)  //REQUIRED
        {
        }
    }
}
