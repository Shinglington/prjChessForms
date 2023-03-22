using prjChessForms.MyChessLibrary;

namespace prjChessForms.Controller
{
    interface IModelObserver
    { 
        void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e);
        void OnPieceSelectionChanged(object sender, PieceSelectionChangedEventArgs e);
        void OnPlayerCapturedPiecesChanged(object sender, PlayerCapturedPiecesChangedEventArgs e);
        void OnPlayerTimerTick(object sender, PlayerTimerTickEventArgs e);
        void OnGameOver(object sender, GameOverEventArgs e);
    }
}
