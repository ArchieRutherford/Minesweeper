/*Title: Minesweeper
Author: Ben Morrison
Desc: Make and play a game of minesweeper.
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

namespace Minesweeper{   //   EXTEND PREVIOUS TO CREATE A 2-D ARRAY OF BUTTONS (each with number on)
    public partial class game : Form{
        static int sizeX = 6, sizeY = 6;
        Button[,] btn = new Button[5, 5];       // Create 2D array of buttons
        int[,] board = new int[sizeX, sizeY];

        public game(){
            InitializeComponent();
            for (int x = 0; x < sizeX; x++){         // Loop for x
                for (int y = 0; y < sizeY; y++){     // Loop for y
                    board[x,y] = 0;
                    btn[x, y] = new Button();
                    btn[x, y].SetBounds(55 * x, 30 + 55 * y, 45, 45);
                    btn[x, y].BackColor = Color.PowderBlue;
                    btn[x, y].Click += new EventHandler(this.btnEvent_Click);
                    Controls.Add(btn[x, y]);
                }
            }
        }
        void btnEvent_Click(object sender, EventArgs e){
			int x = ((((Button)sender).Location.X)/55);
			int y = (((((Button)sender).Location.Y)/55)- 30);
			if(bomb[x, y] == true){
				//Trigger game over
			}else{
				noofBombs = bombCheck(x, y);
			}
			while (noofBombs == 0 /*And Not Discovered*/){
				for (int i=-1; i<2; i++) {
					for (int j=-1; j<2; j++) {	
						noofBombs = bombCheck(x+i, y+j);
					}
				}
			}
        }
		
		int bombCheck(x, y){
			if(bomb[x+1, y] == true){
				bombClose+=1;
			}
			if(bomb[x+-1, y] == true){
				bombClose+=1;
			}
			if(bomb[x, y+1] == true){
				bombClose+=1;
			}
			if(bomb[x, y-1] == true){
				bombClose+=1;
			}
			if(bomb[x+1, y+1] == true){
				bombClose+=1;
			}
			if(bomb[x+1, y-1] == true){
				bombClose+=1;
			}
			if(bomb[x-1, y+1] == true){
				bombClose+=1;
			}
			if(bomb[x-1, y-1] == true){
				bombClose+=1;
			}
			return bombClose;
		}
        private void Form1_Load(object sender, EventArgs e){  //REQUIRED
        }
    }
}