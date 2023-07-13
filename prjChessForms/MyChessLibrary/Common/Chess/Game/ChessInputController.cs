using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;

namespace prjChessForms.Controller
{
    class ChessInputController : IChessInputController
    {
        private IMoveHandler _moveHandler;
        private IPromotionHandler _promotionHandler;

        public void ConnectHandlers(IMoveHandler moveHandler, IPromotionHandler promotionHandler)
        {
            _moveHandler = moveHandler;
            _promotionHandler = promotionHandler;
        }

        public void OnBoardClickReceived(object sender, SquareClickedEventArgs e)
        {
            _moveHandler.ReceiveMoveInput(e.ClickedCoords);
        }

        public void OnPromotionReceived(object sender, PromotionSelectedEventArgs e)
        {
            _promotionHandler.ReceivePromotion(e.SelectedOption);
        }
    }
}

