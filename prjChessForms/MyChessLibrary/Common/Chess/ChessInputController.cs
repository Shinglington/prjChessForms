using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;

namespace prjChessForms.Controller
{
    class ChessInputController : IChessInputController
    {
        private IChess _chess;
        private IInputChangesObserver _userInterface;
        public ChessInputController(IChess chess, IInputChangesObserver _chessUserInterface)
        {
            _chess = chess;
            _userInterface= _chessUserInterface;

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

