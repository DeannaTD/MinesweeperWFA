using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWFA
{
    internal class Board
    {
        int _width, _height;
        public Cell[,] Field { get; set; }
        public int MinesCount { get; set; }
        public Size GetSize => new Size(_width, _height);

        public Board(int width, int height, int mines)
        {
            _width = width;
            _height = height;
            Field = new Cell[_width, _height];
            for(int i = 0; i < _width; i++)
                for(int j = 0; j < _height; j++)
                    Field[i, j] = new Cell();
            MinesCount = mines;
            Initialize(new Point(0,0));
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
                    if(Field[i,j].Value != -1)
                        Field[i, j].Value = GetMinesAround(new Point(i, j));
        }

        public int GetMinesAround(Point asd)
        {
            int mines = 0;
            if(asd.X > 0 && asd.Y > 0)
                if (Field[asd.X - 1, asd.Y - 1].Value == -1) mines++;
            if(asd.Y > 0)
                if (Field[asd.X, asd.Y - 1].Value == -1) mines++;
            if(asd.X < _width - 1 && asd.Y > 0)
                if (Field[asd.X + 1, asd.Y - 1].Value == -1) mines++;
            if(asd.X > 0)
                if (Field[asd.X - 1, asd.Y].Value == -1) mines++;
            if(asd.X < _width - 1)
                if (Field[asd.X + 1, asd.Y].Value == -1) mines++;
            if(asd.X > 0 && asd.Y < _height - 1)
                if (Field[asd.X - 1, asd.Y + 1].Value == -1) mines++;
            if(asd.Y < _height - 1)
                if (Field[asd.X, asd.Y + 1].Value == -1) mines++;
            if(asd.X < _width - 1 && asd.Y < _height - 1)
                if (Field[asd.X + 1, asd.Y + 1].Value == -1) mines++;
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
                    switch(Field[i,j].Value)
                    {
                        case 0:
                            Field[i, j].BackColor = Color.Gray; break;
                        case -1:
                            Field[i, j].Text = "*"; break;
                        default:
                            Field[i, j].ForeColor = Field[i, j].GetTextColor();
                            Field[i, j].Text = Field[i, j].Value.ToString();
                            break;
                    }
                    form.Controls.Add(Field[i, j]);
                }
            }
        }
    }
}
