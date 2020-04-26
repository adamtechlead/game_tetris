using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTetris
{
    public partial class Form1 : Form
    {
        TetrisGame Game = new TetrisGame();

        public Form1()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Game.GameLoop();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (Game.Matrix == null) return;
            int c = 20;
            int w = Game.Matrix.GetUpperBound(0);
            int h = Game.Matrix.GetUpperBound(1);
            int offsetX = 20;
            int offsetY = 50-80;
            e.Graphics.DrawRectangle(Pens.Black, offsetX + 0, offsetY + 80, c * (w + 1), c * (h + 1 - 4));
            for (int x = 0; x <= w; x++)
            {
                for (int y = 4; y <= h; y++)
                {
                    if (Game.Matrix[x, y] > 0)
                    {
                        e.Graphics.FillRectangle(Brushes.Green, offsetX + x * c, offsetY + y * c, c, c);
                        e.Graphics.DrawRectangle(Pens.Black, offsetX + x * c, offsetY + y * c, c, c);
                    }
                    e.Graphics.DrawRectangle(Pens.Black, offsetX + x * c, offsetY + y * c, c, c);
                }
            }
            for (int x = 0; x <= Game.CurrentPiece?.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= Game.CurrentPiece?.GetUpperBound(1); y++)
                {
                    if (Game.CurrentPiece[x, y] == 0 || (y + Game.CurrentY) < 4) continue;
                    e.Graphics.FillRectangle(Brushes.Green, offsetX + (x + Game.CurrentX) * c, offsetY + (y + Game.CurrentY) * c, c, c);
                    e.Graphics.DrawRectangle(Pens.Black, offsetX + (x + Game.CurrentX) * c, offsetY + (y + Game.CurrentY) * c, c, c);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Game.StartNewGame();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
                Game.MoveLeft();
            else if (keyData == Keys.Right)
                Game.MoveRight();
            else if (keyData == Keys.Up)
                Game.TurnPiece();
            else if (keyData == Keys.Down)
                Game.MoveDown();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
