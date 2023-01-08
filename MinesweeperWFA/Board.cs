using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperWFA
{
    internal class Board
    {
        static bool started = false;
        int _width, _height;
        public Cell[,] Field { get; set; }
        public int MinesCount { get; set; }
        public Size GetSize => new Size(_width, _height);

        public Board(int width, int height, int mines)
        {
            _width = width;
            _height = height;
            Field = new Cell[_width, _height];
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                {
                    Field[i, j] = new Cell(i, j);
                    Field[i, j].MouseDown += Board_MouseDown;
                    Field[i, j].DoubleClick += Board_DoubleClick;
                }
            MinesCount = mines;
        }

        private void Board_DoubleClick(object sender, EventArgs e)
        {
        }

        private void Board_MouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show(e.Clicks.ToString());
            Cell cell = (Cell)sender;

            if (e.Button == MouseButtons.Right)
            {
                MarkFlag(cell.X, cell.Y);
            }
            else
            {
                if (!started)
                {
                    Initialize(new Point(cell.X, cell.Y));
                    started = true;
                }
                OpenCellWhileEmpty(cell.X, cell.Y);
            }
        }

        private void MarkFlag(int i, int j)
        {
            switch(Field[i,j].State)
            {
                case CellState.Flag:
                    Field[i,j].Text = "";
                    Field[i, j].State = CellState.None;
                    break;
                case CellState.None:
                    Field[i, j].ForeColor = Color.Red;
                    Field[i, j].Text = Cell.Flag;
                    Field[i, j].State = CellState.Flag;
                    break;
                default: return;
            }
        }

        private void OpenCellWhileEmpty(int i, int j)
        {
            if (Field[i, j].State == CellState.Flag) return;
            Field[i, j].BackColor = Color.Bisque;
            if (Field[i,j].Value != 0)
                Field[i, j].Text = Field[i, j].Value.ToString();
            Point coordinates = new Point(i, j);
            Field[i, j].State = CellState.Open;
            Field[i, j].Enabled = false;
            if (Field[i,j].Value == -1)
            {
                MessageBox.Show("LOSE");
                return;
            }
            if (Field[i, j].Value != 0) return;
            else
            {
                Field[i,j].ForeColor = Color.Black;
                if (coordinates.Y > 0)
                    if(Field[coordinates.X, coordinates.Y - 1].State != CellState.Open) OpenCellWhileEmpty(coordinates.X, coordinates.Y - 1);
                if (coordinates.X > 0)
                    if (Field[coordinates.X - 1, coordinates.Y].State != CellState.Open) OpenCellWhileEmpty(coordinates.X - 1, coordinates.Y);
                if (coordinates.X < _width - 1)
                    if (Field[coordinates.X + 1, coordinates.Y].State != CellState.Open) OpenCellWhileEmpty(coordinates.X + 1, coordinates.Y);
                if (coordinates.Y < _height - 1)
                    if (Field[coordinates.X, coordinates.Y + 1].State != CellState.Open) OpenCellWhileEmpty(coordinates.X, coordinates.Y + 1);

                if (coordinates.X > 0 && coordinates.Y > 0)
                    if (Field[coordinates.X - 1, coordinates.Y - 1].State != CellState.Open) OpenCellWhileEmpty(coordinates.X - 1, coordinates.Y - 1);

                if (coordinates.X < _width - 1 && coordinates.Y > 0)
                    if (Field[coordinates.X + 1, coordinates.Y - 1].State != CellState.Open) OpenCellWhileEmpty(coordinates.X + 1, coordinates.Y - 1);

                if (coordinates.X > 0 && coordinates.Y < _height - 1)
                    if (Field[coordinates.X - 1, coordinates.Y + 1].State != CellState.Open) OpenCellWhileEmpty(coordinates.X - 1, coordinates.Y + 1);
                
                if (coordinates.X < _width - 1 && coordinates.Y < _height - 1)
                    if (Field[coordinates.X + 1, coordinates.Y + 1].State != CellState.Open) OpenCellWhileEmpty(coordinates.X + 1, coordinates.Y + 1);
            }
        }

        private void EndGame()
        {
            MessageBox.Show("Gameover");
        }

        private void Initialize(Point EmptyPoint)
        {
            Random rand = new Random();
            int minesLeft = MinesCount;
            while(minesLeft > 0)
            {
                Point nextPoint = new Point(rand.Next(_width), rand.Next(_height));
                if (nextPoint == EmptyPoint || GetValue(nextPoint) == -1) continue;
                SetMineToPoint(nextPoint);
                minesLeft--;
            }
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    if (Field[i, j].Value != -1)
                    {
                        Field[i, j].Value = GetMinesAround(new Point(i, j));
                        //Field[i, j].Text = Field[i, j].Value.ToString();
                    }
        }

        public int GetMinesAround(Point coordinates)
        {
            int mines = 0;
            if(coordinates.X > 0 && coordinates.Y > 0)
                if (Field[coordinates.X - 1, coordinates.Y - 1].Value == -1) mines++;
            if(coordinates.Y > 0)
                if (Field[coordinates.X, coordinates.Y - 1].Value == -1) mines++;
            if(coordinates.X < _width - 1 && coordinates.Y > 0)
                if (Field[coordinates.X + 1, coordinates.Y - 1].Value == -1) mines++;
            if(coordinates.X > 0)
                if (Field[coordinates.X - 1, coordinates.Y].Value == -1) mines++;
            if(coordinates.X < _width - 1)
                if (Field[coordinates.X + 1, coordinates.Y].Value == -1) mines++;
            if(coordinates.X > 0 && coordinates.Y < _height - 1)
                if (Field[coordinates.X - 1, coordinates.Y + 1].Value == -1) mines++;
            if(coordinates.Y < _height - 1)
                if (Field[coordinates.X, coordinates.Y + 1].Value == -1) mines++;
            if(coordinates.X < _width - 1 && coordinates.Y < _height - 1)
                if (Field[coordinates.X + 1, coordinates.Y + 1].Value == -1) mines++;
            return mines;
        }
        public int GetValue(Point coordinates) => Field[coordinates.X, coordinates.Y].Value;
        private void SetMineToPoint(Point coordinates)
        {
            Field[coordinates.X, coordinates.Y].Value = -1;
        }

        public void DrawField(Form1 form)
        {
            for(int i = 0; i<_width; i++)
            {
                for(int j = 0; j<_height; j++)
                {
                    Field[i, j].Location = new Point(i * 40, j * 40);
                    form.Controls.Add(Field[i, j]);
                }
            }
        }
    }
}
