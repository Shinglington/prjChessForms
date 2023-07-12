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


        private PromotionOption _selectedPromotion;
        private bool _waitingForClick, _waitingForPromotion;
        public OldChess(IBoard board)
        {
            CreatePlayers(new TimeSpan(0, 10, 0));
            _board = board;
            _timer = new System.Timers.Timer(1000);
            _waitingForClick = false;
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
