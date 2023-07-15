using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Threading;
using System.Threading.Tasks;

//namespace prjChessForms.MyChessLibrary
//{

//    class OldChess
//    {

//        public event EventHandler<PlayerCapturedPiecesChangedEventArgs> PlayerCapturedPiecesChanged;
//        public event EventHandler<PromotionEventArgs> PlayerPromotion;

//        private CancellationTokenSource cts = new CancellationTokenSource();


//        public void AttachModelObserver(IModelObserver observer)
//        {

//            PlayerCapturedPiecesChanged += new EventHandler<PlayerCapturedPiecesChangedEventArgs>(observer.OnPlayerCapturedPiecesChanged);
//            PlayerPromotion += new EventHandler<PromotionEventArgs>(observer.OnPromotion);
//            foreach (Square s in _board.GetSquares())
//            {
//                observer.OnPieceInSquareChanged(this, new PieceChangedEventArgs(s, s.Piece));
//            }
//            observer.OnPlayerTimerTick(this, new PlayerTimerTickEventArgs(WhitePlayer));
//            observer.OnPlayerTimerTick(this, new PlayerTimerTickEventArgs(BlackPlayer));
//        }



//        private void CapturePiece(Piece p)
//        {
//            if (p != null)
//            {
//                CurrentPlayer.AddCapturedPiece(p);
//                if (PlayerCapturedPiecesChanged != null)
//                {
//                    PlayerCapturedPiecesChanged.Invoke(this, new PlayerCapturedPiecesChangedEventArgs(CurrentPlayer, p));
//                }
//            }
//        }



//        private async Task Promotion(Coords promotionCoords, CancellationToken cToken)
//        {
//            PieceColour colour = GetPieceAt(promotionCoords).Colour;
//            Piece promotedPiece = new Queen(colour);
//            if (PlayerPromotion != null)
//            {
//                PlayerPromotion.Invoke(this, new PromotionEventArgs(colour, promotionCoords));
//                _waitingForPromotion = true;
//                await _semaphoreReceiveClick.WaitAsync(cToken);
//                _waitingForPromotion = false;
//                switch (_selectedPromotion)
//                {
//                    case PromotionOption.Knight:
//                        promotedPiece = new Knight(colour);
//                        break;
//                    case PromotionOption.Bishop:
//                        promotedPiece = new Bishop(colour);
//                        break;
//                    case PromotionOption.Rook:
//                        promotedPiece = new Rook(colour);
//                        break;
//                }
//            }
//            _board.GetSquareAt(promotionCoords).Piece = promotedPiece;
//        }
//    }
//}
