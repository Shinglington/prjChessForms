using System;

namespace prjChessForms.MyChessLibrary
{
    public interface IChessObserver
    {
        event EventHandler<SquareClickedEventArgs> SquareClicked;
        event EventHandler<PromotionSelectedEventArgs> PromotionSelected;
        void OnCoordSelectionChanged(object sender, CoordsSelectionChangedEventArgs e);
        void OnPlayerCapturedPiecesChanged(object sender, PlayerCapturedPiecesChangedEventArgs e);
        void OnPromotion(object sender, PromotionEventArgs e);
        void OnGameOver(object sender, GameOverEventArgs e);
        void OnPlayerTimerTick(object sender, PlayerTimerTickEventArgs e);
        void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e);
    }
}
