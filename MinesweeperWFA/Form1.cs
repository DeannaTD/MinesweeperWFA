using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperWFA
{
    public partial class Form1 : Form
    {
        bool isOnGame = false;
        public Form1()
        {
            InitializeComponent();
            Board board = new Board(10, 10, 9);
            board.DrawField(this);
        }
    }
}
