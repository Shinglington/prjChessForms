using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using prjChessForms.MyChessLibrary;

namespace prjChessForms.PresentationUI
{
    public partial class ChessForm : Form
    {
        private Chess _game;
        private TableLayoutPanel _layoutPanel;

        private Coords _clickedCoords;
        private Coords _fromCoords = new Coords();
        private Coords _toCoords = new Coords();

        public ChessForm()
        {
            InitializeComponent();

            CreateGame();

        }

        public async Task StartGame()
        {
            await _game.Play();
            OnGameOver();
        }


        public async Task Play(CancellationToken cToken)
        {
            _currentPlayer = _players[0];
            _result = GameResult.Unfinished;
            while (_result == GameResult.Unfinished)
            {
                try
                {
                    _timer.Elapsed += OnPlayerTimerTick;
                    _timer.Start();
                    ChessMove move = await GetPlayerMove(cToken);
                    _timer.Stop();
                    _timer.Elapsed -= OnPlayerTimerTick;
                    Rulebook.MakeMove(_board, _currentPlayer, move);
                    if (_currentPlayer == _players[1])
                    {
                        _currentPlayer = _players[0];
                    }
                    else
                    {
                        _currentPlayer = _players[1];
                    }
                    _result = Rulebook.GetGameResult(_board, _currentPlayer);
                }
                catch when (cToken.IsCancellationRequested)
                {
                    _result = GameResult.Time;
                }
            }
        }

        private async Task<ChessMove> GetPlayerMove(CancellationToken cToken)
        {

            _fromCoords = new Coords();
            _toCoords = new Coords();
            ChessMove move = new ChessMove();
            bool validMove = false;
            while (!validMove)
            {
                await _semaphoreClick.WaitAsync(cToken);
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

        private void CreateGame()
        {
            _game = new Chess();
        }
        private void CreatePlayers()
        {
            _players = new Player[2];
            _players[0] = new HumanPlayer(PieceColour.White, new TimeSpan(0, 3, 0));
            _players[1] = new HumanPlayer(PieceColour.Black, new TimeSpan(0, 3, 0));
        }

        private void SetupControls()
        {

            // Timer
            _timer = new System.Timers.Timer(1000);
            _timerLabels = new Label[2];

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

            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 5));

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

            // White player label
            TableLayoutPanel whiteTable = new TableLayoutPanel()
            {
                Parent = _layoutPanel,
                Dock = DockStyle.Fill,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 50), new ColumnStyle(SizeType.Percent, 50) },
                RowStyles = { new RowStyle(SizeType.Percent, 100) },
            };
            _layoutPanel.SetCellPosition(whiteTable, new TableLayoutPanelCellPosition(0, 2));
            Label whiteLabel = new Label()
            {
                Parent = whiteTable,
                Dock = DockStyle.Fill,
                Text = _players[0].Colour.ToString(),
            };
            whiteTable.SetCellPosition(whiteLabel, new TableLayoutPanelCellPosition(0, 0));
            _timerLabels[0] = new Label()
            {
                Parent = whiteTable,
                Dock = DockStyle.Fill,
                Text = _players[0].RemainingTime.ToString(),
            };
            whiteTable.SetCellPosition(_timerLabels[0], new TableLayoutPanelCellPosition(1, 0));





            // Black player label
            TableLayoutPanel blackTable = new TableLayoutPanel()
            {
                Parent = _layoutPanel,
                Dock = DockStyle.Fill,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 50), new ColumnStyle(SizeType.Percent, 50) },
                RowStyles = { new RowStyle(SizeType.Percent, 100) },
            };
            _layoutPanel.SetCellPosition(blackTable, new TableLayoutPanelCellPosition(0, 0));
            Label blackLabel = new Label()
            {
                Parent = blackTable,
                Dock = DockStyle.Fill,
                Text = _players[1].Colour.ToString(),
            };
            blackTable.SetCellPosition(blackLabel, new TableLayoutPanelCellPosition(0, 0));

            _timerLabels[1] = new Label()
            {
                Parent = blackTable,
                Dock = DockStyle.Fill,
                Text = _players[1].RemainingTime.ToString(),
            };
            whiteTable.SetCellPosition(_timerLabels[1], new TableLayoutPanelCellPosition(1, 0));
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

        private void OnPlayerTimerTick(object sender, ElapsedEventArgs e)
        {
            _currentPlayer.TickTime(new TimeSpan(0, 0, 1));
            Label timeLabel = _currentPlayer == _players[0] ? _timerLabels[0] : _timerLabels[1];
            timeLabel.Invoke((MethodInvoker)delegate
            {
                timeLabel.Text = _currentPlayer.RemainingTime.ToString();
            });

            if (_currentPlayer.RemainingTime > TimeSpan.Zero)
            {
                _timer.Elapsed -= OnPlayerTimerTick;
                cts.Cancel();
            }
        }


        private void OnGameOver()
        {
            cts.Cancel();
            foreach (Square s in _board.GetSquares())
            {
                s.Click -= OnSquareClicked;
            }
            Player winner = null;
            if (_result == GameResult.Checkmate || _result == GameResult.Time)
            {
                winner = _currentPlayer == _players[0] ? _players[1] : _players[0];
            }
            MessageBox.Show(_result.ToString() + " ," + (winner != null ? winner.Colour.ToString() : "Nobody") + " Wins");
        }
    }
}
