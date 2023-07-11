using prjChessForms.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

using prjChessForms.MyChessLibrary.Pieces;

namespace prjChessForms.MyChessLibrary
{

    class OldChess
    {
        public event EventHandler<PieceSelectionChangedEventArgs> PieceSelectionChanged;
        public event EventHandler<PlayerTimerTickEventArgs> PlayerTimerTick;
        public event EventHandler<PlayerCapturedPiecesChangedEventArgs> PlayerCapturedPiecesChanged;
        public event EventHandler<PromotionEventArgs> PlayerPromotion; 
        public event EventHandler<GameOverEventArgs> GameOver;

        private IBoard _board;
        private System.Timers.Timer _timer;
        private CancellationTokenSource cts = new CancellationTokenSource();

        private GameResult _result;

        private Player[] _players;
        private int _turnCount;

        private SemaphoreSlim _semaphoreReceiveClick = new SemaphoreSlim(0, 1);
        private Coords _clickedCoords;
        private PromotionOption _selectedPromotion;
        private bool _waitingForClick, _waitingForPromotion;
        public OldChess(IBoard board)
        {
            CreatePlayers(new TimeSpan(0, 10, 0));
            _board = board;
            _timer = new System.Timers.Timer(1000);
            _waitingForClick = false;
        }
        public Player CurrentPlayer { get { return _players[_turnCount % 2]; } }
        public Player WhitePlayer { get { return _players[0]; } }
        public Player BlackPlayer { get { return _players[1]; } }
        public ISquare[,] BoardSquares { get { return _board.GetSquares(); } }
        public IPiece GetPieceAt(Coords coords) => _board.GetSquareAt(coords).Piece;
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

        public void SendPromotion(PromotionOption option)
        {
            if (_waitingForPromotion)
            {
                Debug.WriteLine("Promotion received to {0}", option.ToString());
                _selectedPromotion = option;
                _semaphoreReceiveClick.Release();
            }
        }

        public void AttachModelObserver(IModelObserver observer)
        {
            _board.PieceInSquareChanged += new EventHandler<PieceChangedEventArgs>(observer.OnPieceInSquareChanged);
            PieceSelectionChanged += new EventHandler<PieceSelectionChangedEventArgs>(observer.OnPieceSelectionChanged);
            PlayerTimerTick += new EventHandler<PlayerTimerTickEventArgs>(observer.OnPlayerTimerTick);
            PlayerCapturedPiecesChanged += new EventHandler<PlayerCapturedPiecesChangedEventArgs>(observer.OnPlayerCapturedPiecesChanged);
            PlayerPromotion += new EventHandler<PromotionEventArgs>(observer.OnPromotion);
            GameOver += new EventHandler<GameOverEventArgs>(observer.OnGameOver);
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
                    PieceMovement move = await GetChessMove(cToken);
                    CapturePiece(FullRulebook.MakeMove(_board, CurrentPlayer.Colour, move));
                    ChangeSelection(null);
                    if (FullRulebook.RequiresPromotion(_board, move.EndCoords))
                    {
                        await Promotion(move.EndCoords, cToken);
                    }
                    _turnCount++;
                    _result = FullRulebook.GetGameResult(_board, CurrentPlayer);
                }
                catch when (cToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Cancelled Play");
                    _waitingForClick = false;
                    _result = GameResult.Time;
                }
            }
            _timer.Elapsed -= OnPlayerTimerTick;
            cts.Cancel();
        }

        private async Task<PieceMovement> GetChessMove(CancellationToken cToken)
        {
            Coords fromCoords = Coords.Null;
            Coords toCoords = Coords.Null;
            PieceMovement move = new ChessMove();
            bool completeInput = false;
            _waitingForClick = true;
            _timer.Start();
            while (!completeInput)
            {
                Debug.WriteLine("Waiting for click");
                await _semaphoreReceiveClick.WaitAsync(cToken);
                Debug.WriteLine("Received click at {0}", _clickedCoords);

                if (GetPieceAt(_clickedCoords) != null && GetPieceAt(_clickedCoords).Colour.Equals(CurrentPlayer.Colour))
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
                if (!toCoords.IsNull && !fromCoords.IsNull && FullRulebook.CheckLegalMove(_board, CurrentPlayer.Colour, new ChessMove(fromCoords, toCoords)))
                {
                    move = new ChessMove(fromCoords, toCoords);
                    completeInput = true;
                }
                else
                {
                    toCoords = Coords.Null;
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
            TimeSpan interval = new TimeSpan(0, 0, 1);
            if (CurrentPlayer.RemainingTime.Subtract(interval) < TimeSpan.Zero)
            {
                _timer.Elapsed -= OnPlayerTimerTick;
                cts.Cancel();
            }
            else
            {
                CurrentPlayer.TickTime(interval);
                if (PlayerTimerTick != null)
                {
                    PlayerTimerTick.Invoke(this, new PlayerTimerTickEventArgs(CurrentPlayer));
                }
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
            if (p != null)
            {
                CurrentPlayer.AddCapturedPiece(p);
                if (PlayerCapturedPiecesChanged != null)
                {
                    PlayerCapturedPiecesChanged.Invoke(this, new PlayerCapturedPiecesChangedEventArgs(CurrentPlayer, p));
                }
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
                    foreach (PieceMovement m in FullRulebook.GetPossibleMoves(_board, selectedPiece))
                    {
                        endCoords.Add(m.EndCoords);
                    }
                }
                PieceSelectionChanged.Invoke(this, new PieceSelectionChangedEventArgs(selectedPiece, selectedCoords, endCoords));
            }
        }

        private async Task Promotion(Coords promotionCoords, CancellationToken cToken)
        {
            PieceColour colour = GetPieceAt(promotionCoords).Colour;
            Piece promotedPiece = new Queen(colour);
            if (PlayerPromotion != null)
            {
                PlayerPromotion.Invoke(this, new PromotionEventArgs(colour, promotionCoords));
                _waitingForPromotion = true;
                await _semaphoreReceiveClick.WaitAsync(cToken);
                _waitingForPromotion = false;
                switch (_selectedPromotion) 
                {
                    case PromotionOption.Knight:
                        promotedPiece = new Knight(colour);
                        break;
                    case PromotionOption.Bishop:
                        promotedPiece = new Bishop(colour);
                        break;
                    case PromotionOption.Rook:
                        promotedPiece = new Rook(colour);
                        break;
                }
            }
            _board.GetSquareAt(promotionCoords).Piece = promotedPiece;
        }
    }
}
