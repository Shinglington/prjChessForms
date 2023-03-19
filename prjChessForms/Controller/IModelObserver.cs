using prjChessForms.MyChessLibrary;
using System;


namespace prjChessForms.Controller
{
    interface IModelObserver
    {
        void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e);
        void OnPieceSelectionChanged(object sender, PieceSelectionChangedEventArgs e);

        void OnPlayerTimerTick(object sender, PlayerTimerTickEventArgs e)
    }
}
