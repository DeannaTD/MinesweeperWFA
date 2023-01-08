using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperWFA
{
    internal class Cell : Button
    {
        public static string Flag = "\u2691";
        public CellState State { get; set; }
        public int Value { get; set; }
        public int X, Y;

        public Cell(int X, int Y) : base()
        {
            State = CellState.None;
            Width = 40;
            Height = 40;
            this.X = X;
            this.Y = Y;
        }

        public Color GetTextColor()
        {
            switch (Value)
            {
                case 1: return Color.Green;
                case 2: return Color.Blue;
                case 3: return Color.Red;
                case 4: return Color.Magenta;
                case 5: return Color.Yellow;
                case 6: return Color.Orange;
                case 7: return Color.Orchid;
                case 8: return Color.Pink;
            }
            return Color.Gray;
        } 
    }

    public enum CellState
    {
        None,
        Flag,
        Open,
        Question
    }
}
