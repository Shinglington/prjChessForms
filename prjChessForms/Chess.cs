using System;
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
            Play();
        }
        public async void Play()
        {
            _currentPlayer = _players[0];
            _currentTurn = 1;
            while (!GameOver())
            {
                ChessMove move = await _currentPlayer.GetMove(_board);
                if (_currentPlayer == _players[1])
                {
                    _currentTurn += 1;
                    _currentPlayer = _players[0];
                }
                else
                {
                    _currentPlayer = _players[1];
                }
            }
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

        private bool GameOver()
        {
            return false;
        }
    }
}
