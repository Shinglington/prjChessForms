using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Debug.WriteLine("Controller received coords clicked at {0} ", e.ClickedCoords);
            _chessModel.SendCoords(e.ClickedCoords);
        }
    }
}

