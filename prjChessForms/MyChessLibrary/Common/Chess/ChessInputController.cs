using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;

namespace prjChessForms.Controller
{
    class ChessInputController : IChessInputController
    {
        private IChess _chess;
        private IChessInterface _userInterface;
        public ChessInputController(IChess chess, IChessInterface chessInterface)
        {
            _chess = chess;
            _userInterface= chessInterface;

            _userInterface.SquareClicked += OnBoardClickReceived;
            _userInterface.PromotionSelected += OnPromotionReceived;
        }

        public void OnBoardClickReceived(object sender, SquareClickedEventArgs e)
        {
            _chess.SendCoords(e.ClickedCoords);
        }

        public void OnPromotionReceived(object sender, PromotionSelectedEventArgs e)
        {
            _chess.SendPromotion(e.SelectedOption);
        }
    }
}

