using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjChessForms.Controller
{
    class ChessController
    {
        private CancellationToken cts = new CancellationToken();

        private Chess _chessModel;
        private ChessForm _chessView;

        public ChessController(Chess chessModel, ChessForm chessView)
        {
            _chessModel = chessModel;
            _chessView = chessView;

            _chessView.Controller = this;
            _chessModel.AttachModelObserver(_chessView);

            _chessView.SquareClicked += (sender, e) => _chessModel.SendCoords(e.ClickedCoords);
        }

        private void OnGameOver(object sender, GameOverEventArgs e)
        {
            MessageBox.Show(e.Result.ToString() + " ," + (e.Winner != null ? e.Winner.Colour.ToString() : "Nobody") + " Wins");
        }
    }
}

