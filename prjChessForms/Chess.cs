using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjChessForms
{
    public partial class Chess : Form
    {
        private Board _board;
        private TableLayoutPanel _layoutPanel;
        private Label _currentPlayerLabel;

        private SemaphoreSlim _semaphoreClick = new SemaphoreSlim(0, 1);
        private Coords _clickedCoords;
        private Coords _fromCoords = new Coords();
        private Coords _toCoords = new Coords();

        private GameResult _result;
        private Player[] _players;
        private Player _currentPlayer;
        private int _currentTurn;
        public Chess()
        {
            InitializeComponent();
            CreatePlayers();
            SetupControls();
            Play();
        }
        public async void Play()
        {
            _currentPlayer = _players[0];
            _currentTurn = 1;
            _result = GameResult.Unfinished;
            while (_result == GameResult.Unfinished)
            {
                _currentPlayerLabel.Text = _currentPlayer.Colour.ToString();
                if (Rulebook.IsInCheck(_board, _currentPlayer))
                {
                    _currentPlayerLabel.Text += " - Check";
                }

                ChessMove move = await GetPlayerMove();
                Console.WriteLine("Hi");
                Rulebook.MakeMove(_board, _currentPlayer, move);
                Console.WriteLine(move);
                if (_currentPlayer == _players[1])
                {
                    _currentTurn += 1;
                    _currentPlayer = _players[0];
                }
                else
                {
                    _currentPlayer = _players[1];
                }

                _result = Rulebook.GetGameResult(_board, _currentPlayer);
            }
            OnGameOver();
        }

        private async Task<ChessMove> GetPlayerMove()
        {

            _fromCoords = new Coords();
            _toCoords = new Coords();
            ChessMove move = new ChessMove();
            bool validMove = false;
            while (!validMove)
            {
                await _semaphoreClick.WaitAsync();
                if (_board.GetPieceAt(_clickedCoords) != null && _board.GetPieceAt(_clickedCoords).Owner == _currentPlayer)
                {
                    _fromCoords = _clickedCoords;
                    _toCoords = new Coords();
                    _board.ClearHighlights();
                    _board.HighlightAt(_fromCoords, System.Drawing.Color.AliceBlue);
                    foreach (ChessMove m in Rulebook.GetPossibleMoves(_board, _board.GetPieceAt(_fromCoords)))
                    {
                        _board.HighlightAt(m.EndCoords, System.Drawing.Color.Green);
                    }
                }
                else if (!_fromCoords.Equals(new Coords()))
                {
                    _toCoords = _clickedCoords;
                }
                // Check if move is valid now

                if (!_toCoords.Equals(new Coords()) && !_fromCoords.Equals(new Coords()))
                {
                    move = new ChessMove(_fromCoords, _toCoords);
                    validMove = Rulebook.CheckLegalMove(_board, _currentPlayer, move);
                }
            }
            _board.ClearHighlights();
            return move;
        }


        private void CreatePlayers()
        {
            _players = new Player[2];
            _players[0] = new HumanPlayer(PieceColour.White);
            _players[1] = new HumanPlayer(PieceColour.Black);
        }

        private void SetupControls()
        {
            // Layout
            _layoutPanel = new TableLayoutPanel()
            {
                Parent = this,
                Dock = DockStyle.Fill,
            };
            _layoutPanel.ColumnStyles.Clear();
            _layoutPanel.RowStyles.Clear();

            _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90));
            _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 90));

            // Board
            _board = new Board(_players)
            {
                Parent = _layoutPanel
            };
            _layoutPanel.SetCellPosition(_board, new TableLayoutPanelCellPosition(0, 1));
            foreach (Square square in _board.GetSquares())
            {
                square.Click += OnSquareClicked;
            }

            // Current player 
            _currentPlayerLabel = new Label()
            {
                Parent = _layoutPanel
            };
            _layoutPanel.SetCellPosition(_currentPlayerLabel, new TableLayoutPanelCellPosition(0, 0));
        }

        private void OnSquareClicked(object sender, EventArgs e)
        {
            if (sender is Square square)
            {
                _clickedCoords = square.Coords;
                Console.WriteLine(_clickedCoords);
                _semaphoreClick.Release();
            }
        }

        private void OnGameOver()
        {
            foreach(Square s in _board.GetSquares())
            {
                s.Click -= OnSquareClicked;
            }

            Player winner = null;
            if (_result ==  GameResult.Checkmate)
            {
                winner = _currentPlayer == _players[0] ? _players[1] : _players[0];
            }

            MessageBox.Show(_result.ToString() + " ," + (winner != null ? winner.Colour.ToString() : "Nobody") + " Wins");
        }
    }



}
