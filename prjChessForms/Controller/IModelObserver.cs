using prjChessForms.MyChessLibrary;
using System;


namespace prjChessForms.Controller
{
    interface IModelObserver
    {
        void OnModelChanged(object sender, ModelChangedEventArgs e);
        void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e);
        void OnPieceSelectionChanged(object sender, PieceSelectionChangedEventArgs e);
        void OnPlayerCapturedPiecesChanged(object sender, PlayerCapturedPiecesChangedEventArgs e);
        void OnPlayerTimerTick(object sender, PlayerTimerTickEventArgs e)
    }
}
