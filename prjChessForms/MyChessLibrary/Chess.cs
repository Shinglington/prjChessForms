using prjChessForms.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace prjChessForms.MyChessLibrary
{
    class Chess
    {
        public event EventHandler<PieceSelectionChangedEventArgs> PieceSelectionChanged;
        public event EventHandler<PlayerTimerTickEventArgs> PlayerTimerTick;
        public event EventHandler<PlayerCapturedPiecesChangedEventArgs> PlayerCapturedPiecesChanged;

        public event EventHandler<GameOverEventArgs> GameOver;



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
            CreatePlayers(new TimeSpan(0, 3, 0));
            _board = new Board(_players);
            _timer = new System.Timers.Timer(1000);
            _waitingForClick = false;
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
        public void SendCoords(Coords coords)
        {
            if (_waitingForClick)
            {
                Debug.WriteLine("Model received coords input of {0}", coords);
                _clickedCoords = coords;
                _semaphoreReceiveClick.Release();
            }
        }

        public void AttachModelObserver(IModelObserver observer)
        {
            _board.PieceInSquareChanged += new EventHandler<PieceChangedEventArgs>(observer.OnPieceInSquareChanged);
            PieceSelectionChanged += new EventHandler<PieceSelectionChangedEventArgs>(observer.OnPieceSelectionChanged);
            PlayerTimerTick += new EventHandler<PlayerTimerTickEventArgs>(observer.OnPlayerTimerTick);
            PlayerCapturedPiecesChanged += new EventHandler<PlayerCapturedPiecesChangedEventArgs>(observer.OnPlayerCapturedPiecesChanged);
            foreach(Square s in _board.GetSquares())
            {
                observer.OnPieceInSquareChanged(this, new PieceChangedEventArgs(s, s.Piece));
            }

            observer.OnPlayerTimerTick(this, new PlayerTimerTickEventArgs(WhitePlayer));
            observer.OnPlayerTimerTick(this, new PlayerTimerTickEventArgs(BlackPlayer));
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
                        CapturePiece(Rulebook.MakeMove(_board, CurrentPlayer, move));
                        ChangeSelection(null);
                        _result = Rulebook.GetGameResult(_board, CurrentPlayer);
                        _turnCount++;
                    }
                }
                catch when (cToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Cancelled Play");
                    _result = GameResult.Time;
                }
            }
            cts.Cancel();
            _timer.Elapsed -= OnPlayerTimerTick;
        }

        private async Task<ChessMove> GetChessMove(CancellationToken cToken)
        {
            Coords fromCoords = Coords.Null;
            Coords toCoords = Coords.Null;
            ChessMove move = new ChessMove();
            bool completeInput = false;
            _waitingForClick = true;
            _timer.Start();
            while (!completeInput)
            {
                Debug.WriteLine("Waiting for click");
                await _semaphoreReceiveClick.WaitAsync(cToken);
                Debug.WriteLine("Received click at {0}", _clickedCoords);

                if (GetPieceAt(_clickedCoords) != null && GetPieceAt(_clickedCoords).Owner.Equals(CurrentPlayer))
                {
                    ChangeSelection(GetPieceAt(_clickedCoords));
                    fromCoords = _clickedCoords;
                    toCoords = Coords.Null;
                }
                else if (!fromCoords.Equals(Coords.Null))
                {
                    toCoords = _clickedCoords;
                }
                // Check if move is valid now
                if (!toCoords.IsNull && !fromCoords.IsNull)
                {
                    move = new ChessMove(fromCoords, toCoords);
                    completeInput = true;
                }
            }
            _waitingForClick = false;
            _timer.Stop();
            return move;
        }
        private void CreatePlayers(TimeSpan time)
        {
            _players = new Player[2];
            _players[0] = new HumanPlayer(PieceColour.White, time);
            _players[1] = new HumanPlayer(PieceColour.Black, time);
        }
        private void OnPlayerTimerTick(object sender, ElapsedEventArgs e)
        {
            CurrentPlayer.TickTime(new TimeSpan(0, 0, 1));
            if (PlayerTimerTick != null)
            {
                PlayerTimerTick.Invoke(this, new PlayerTimerTickEventArgs(CurrentPlayer));
            }
            if (CurrentPlayer.RemainingTime < TimeSpan.Zero)
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
                GameOver.Invoke(this, new GameOverEventArgs(_result, winner));
            }
        }

        private void CapturePiece(Piece p)
        {
            CurrentPlayer.AddCapturedPiece(p);
            if (PlayerCapturedPiecesChanged != null)
            {
                PlayerCapturedPiecesChanged.Invoke(this, new PlayerCapturedPiecesChangedEventArgs(CurrentPlayer));
            }
        }

        private void ChangeSelection(Piece selectedPiece)
        {
            if (PieceSelectionChanged != null)
            {
                List<Coords> endCoords = new List<Coords>();
                Coords selectedCoords = new Coords();
                if (selectedPiece != null)
                {
                    selectedCoords = GetCoordsOf(selectedPiece);
                    foreach (ChessMove m in Rulebook.GetPossibleMoves(_board, selectedPiece))
                    {
                        endCoords.Add(m.EndCoords);
                    }
                }
                PieceSelectionChanged.Invoke(this, new PieceSelectionChangedEventArgs(selectedPiece, selectedCoords, endCoords));
            }
        }
    }
}
