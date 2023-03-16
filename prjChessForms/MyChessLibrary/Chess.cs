using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace prjChessForms.MyChessLibrary
{
    class GameOverEventArgs : EventArgs
    {
        public GameOverEventArgs(Player winner, GameResult result) 
        {
            Result = result;
            Winner = winner;
        }
        public Player Winner { get; set; }
        public GameResult Result { get; set; }
    }
    
    class PieceSelectionChangedEventArgs : EventArgs
    {
        public PieceSelectionChangedEventArgs(Piece selectedPiece, List<ChessMove> validMoves)
        {
            SelectedPiece = selectedPiece;
            ValidMoves = validMoves;
        }
        public Piece SelectedPiece { get; set; }
        public List<ChessMove> ValidMoves { get; set; }
    }

    class Chess
    {
        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<PieceSelectionChangedEventArgs> PieceSelectionChanged;

        private Board _board;
        private System.Timers.Timer _timer;
        private CancellationTokenSource cts = new CancellationTokenSource();

        private GameResult _result;
        private Player[] _players;
        private int _turnCount;

        private Coords _receivedClick;
        private SemaphoreSlim _semaphoreReceiveClick = new SemaphoreSlim(0, 1);
        private bool _waitingForClick;
        public Chess(Controller controller)
        {
            Controller = controller;
            CreatePlayers();
            _board = new Board(_players);
            _timer = new System.Timers.Timer(1000);
            _waitingForClick = false;
            SetupControllerEvents();
        }
        public Controller Controller { get; }
        public Player CurrentPlayer { get { return _players[_turnCount % 2]; } }
        public Player WhitePlayer { get { return _players[0]; } }
        public Player BlackPlayer { get { return _players[1]; } }
        public Square[,] BoardSquares { get { return _board.GetSquares(); } }
        public Piece GetPieceAt(Coords coords) => _board.GetPieceAt(coords);

        public async Task StartGame()
        {
            await Play(cts.Token);
            OnGameOver();
        }

        public async Task Play(CancellationToken cToken)
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
            _timer.Start();
            RequestMoveInput.Invoke(this, new RequestMoveInputEventArgs(CurrentPlayer, cToken));
            await _semaphoreReceiveClick.WaitAsync(cToken);
            _timer.Stop();
                _fromCoords = new Coords();
                _toCoords = new Coords();
                ChessMove move = new ChessMove();
                bool completeInput = false;
                while (!completeInput)
                {
                    await _semaphoreClick.WaitAsync(cToken);
                    if (_game.GetPieceAt(_clickedCoords) != null && _game.GetPieceAt(_clickedCoords).Owner.Equals(_game.CurrentPlayer))
                    {
                        _fromCoords = _clickedCoords;
                        _toCoords = new Coords();
                    }
                    else if (!_fromCoords.Equals(new Coords()))
                    {
                        _toCoords = _clickedCoords;
                    }
                    // Check if move is valid now

                    if (!_toCoords.Equals(new Coords()) && !_fromCoords.Equals(new Coords()))
                    {
                        move = new ChessMove(_fromCoords, _toCoords);
                        completeInput = true;
                    }
                }
                return move;
        }



        private void SetupControllerEvents()
        {
            MoveReceived += OnMoveReceived;
        }
        private void CreatePlayers()
        {
            _players = new Player[2];
            _players[0] = new HumanPlayer(PieceColour.White, new TimeSpan(0, 3, 0));
            _players[1] = new HumanPlayer(PieceColour.Black, new TimeSpan(0, 3, 0));
        }

        private void OnReceiveCoords(object sender, ReceiveCoordsEventArgs e) 
        {
            if (_waitingForClick)
            {
                _receivedClick = e.Coords;
                _semaphoreReceiveClick.Release();
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
