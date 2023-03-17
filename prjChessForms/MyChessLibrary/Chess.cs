using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace prjChessForms.MyChessLibrary
{
    class Chess
    {
        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<PieceSelectionChangedEventArgs> PieceSelectionChanged;
        public event EventHandler<PieceChangedEventArgs> PieceChanged;

        private Board _board;
        private System.Timers.Timer _timer;
        private CancellationTokenSource cts = new CancellationTokenSource();

        private GameResult _result;
        private Player[] _players;
        private int _turnCount;

        private Coords _clickedCoords;
        private SemaphoreSlim _semaphoreReceiveClick = new SemaphoreSlim(0, 1);
        private bool _waitingForClick;
        public Chess()
        {
            CreatePlayers();
            _board = new Board(_players);
            _timer = new System.Timers.Timer(1000);
            _waitingForClick = false;
            SetupEvents();
        }
        public Player CurrentPlayer { get { return _players[_turnCount % 2]; } }
        public Player WhitePlayer { get { return _players[0]; } }
        public Player BlackPlayer { get { return _players[1]; } }
        public Square[,] BoardSquares { get { return _board.GetSquares(); } }
        public Piece GetPieceAt(Coords coords) => _board.GetPieceAt(coords);
        public Coords GetCoordsOf(Piece piece) => _board.GetCoordsOfPiece(piece);

        public async Task StartGame()
        {
            await Play(cts.Token);
            OnGameOver();
        }

        public void SyncGameAndBoard()
        {
            if (PieceChanged != null)
            {
                foreach (Square s in BoardSquares)
                {
                    PieceChanged.Invoke(this, new PieceChangedEventArgs(s, s.Piece));
                }
            }
        }

        private async Task Play(CancellationToken cToken)
        {
            _turnCount = 0;
            _result = GameResult.Unfinished;
            _timer.Elapsed += OnPlayerTimerTick;
            while (_result == GameResult.Unfinished)
            {
                try
                {
                    ChessMove move = await GetChessMove(cToken);
                    if (Rulebook.CheckLegalMove(_board, CurrentPlayer, move))
                    {
                        Rulebook.MakeMove(_board, CurrentPlayer, move);
                        _result = Rulebook.GetGameResult(_board, CurrentPlayer);
                        _turnCount++;
                    }
                }
                catch when (cToken.IsCancellationRequested)
                {
                    _result = GameResult.Time;
                }
            }
            cts.Cancel();
            _timer.Elapsed -= OnPlayerTimerTick;
        }

        private async Task<ChessMove> GetChessMove(CancellationToken cToken)
        {
            Coords fromCoords = new Coords();
            Coords toCoords = new Coords();
            ChessMove move = new ChessMove();
            bool completeInput = false;
            _waitingForClick = true;
            _timer.Start();
            while (!completeInput)
            {
                await _semaphoreReceiveClick.WaitAsync(cToken);
                if (GetPieceAt(_clickedCoords) != null && GetPieceAt(_clickedCoords).Owner.Equals(CurrentPlayer))
                {
                    Piece p = GetPieceAt(_clickedCoords);
                    if (PieceSelectionChanged != null)
                    {
                        PieceSelectionChanged.Invoke(this, new PieceSelectionChangedEventArgs(p, Rulebook.GetPossibleMoves(_board, p)));
                    }
                    fromCoords = _clickedCoords;
                    toCoords = new Coords();
                }
                else if (!fromCoords.Equals(new Coords()))
                {
                    toCoords = _clickedCoords;
                }
                // Check if move is valid now
                if (!toCoords.Equals(new Coords()) && !fromCoords.Equals(new Coords()))
                {
                    move = new ChessMove(fromCoords, toCoords);
                    completeInput = true;
                }
            }
            _timer.Stop();
            _waitingForClick = false;
            return move;
        }
        private void CreatePlayers()
        {
            _players = new Player[2];
            _players[0] = new HumanPlayer(PieceColour.White, new TimeSpan(0, 3, 0));
            _players[1] = new HumanPlayer(PieceColour.Black, new TimeSpan(0, 3, 0));
        }
        private void SetupEvents()
        {
            _board.PieceChanged += OnPieceChanged;
        }

        private void OnPieceChanged(object sender, PieceChangedEventArgs e)
        {
            if (PieceChanged != null)
            {
                PieceChanged.Invoke(this, e);
            }
        }

        private void OnPlayerTimerTick(object sender, ElapsedEventArgs e)
        {
            CurrentPlayer.TickTime(new TimeSpan(0, 0, 1));
            if (CurrentPlayer.RemainingTime > TimeSpan.Zero)
            {
                _timer.Elapsed -= OnPlayerTimerTick;
                cts.Cancel();
            }
        }
        private void OnGameOver()
        {
            cts.Cancel();
            Player winner = null;
            if (_result == GameResult.Checkmate || _result == GameResult.Time)
            {
                winner = CurrentPlayer == _players[0] ? _players[1] : _players[0];
            }
            if (GameOver != null)
            {
                GameOver.Invoke(this, new GameOverEventArgs(winner, _result));
            }
        }
    }
}
