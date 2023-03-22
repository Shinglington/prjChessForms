using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;

namespace prjChessForms.Controller
{
    class ChessController
    {
        private Chess _chessModel;
        private ChessForm _chessView;

        public ChessController(Chess chessModel, ChessForm chessView)
        {
            _chessModel = chessModel;
            _chessView = chessView;

            _chessView.Controller = this;
            _chessModel.AttachModelObserver(_chessView);

            _chessView.SquareClicked += OnBoardClickReceived;
            _chessModel.StartGame();
        }

        private void OnBoardClickReceived(object sender, SquareClickedEventArgs e)
        {
            _chessModel.SendCoords(e.ClickedCoords);
        }
    }
}

