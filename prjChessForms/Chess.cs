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
        private Player _currentPlayer;
        private int _currentTurn; 
        public Chess()
        {
            InitializeComponent();
            CreatePlayers();
            CreateBoard();

            _board.MakeMove((0, 0), (5, 5));
        }

        public void Play()
        {
            _currentPlayer = _players[0];
            _currentTurn = 1;

        }


        private void CreatePlayers()
        {
            _players = new Player[2];
            _players[0] = new HumanPlayer(PieceColour.White);
            _players[1] = new HumanPlayer(PieceColour.Black);
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
