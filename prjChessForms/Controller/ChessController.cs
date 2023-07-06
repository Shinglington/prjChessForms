﻿using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;

namespace prjChessForms.Controller
{
    class ChessController
    {
        private OldChess _chessModel;
        private ChessForm _chessView;

        public ChessController(OldChess chessModel, ChessForm chessView)
        {
            _chessModel = chessModel;
            _chessView = chessView;

            _chessView.Controller = this;
            _chessModel.AttachModelObserver(_chessView);

            _chessView.SquareClicked += OnBoardClickReceived;
            _chessView.PromotionSelected += OnPromotionReceived;
            _chessModel.StartGame();
        }

        private void OnBoardClickReceived(object sender, SquareClickedEventArgs e)
        {
            _chessModel.SendCoords(e.ClickedCoords);
        }

        private void OnPromotionReceived(object sender, PromotionSelectedEventArgs e)
        {
            _chessModel.SendPromotion(e.SelectedOption);
        }
    }
}

