using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjChessForms
{
    public partial class Chess : Form
    {
        private Board _board;
        private Player[] _players;
        private int _currentTurn;
        public Chess()
        {
            InitializeComponent();
            CreatePlayers();
            CreateBoard();

            _board.MakeMove((0, 0), (5, 5));
        }


        private void CreatePlayers()
        {
            _players = new Player[2];
            _players[0] = new Player();
            _players[1] = new Player();
        }

        private void CreateBoard()
        {
            TableLayoutPanel boardTable = new TableLayoutPanel()
            {
                Parent = this,
                Dock = DockStyle.Fill
            };

            _board = new Board(boardTable, _players);
        }
    }
}
