using System;

namespace prjChessForms.MyChessLibrary
{
    public interface IChessInterface
    {
        event EventHandler<SquareClickedEventArgs> SquareClicked;
        event EventHandler<PromotionSelectedEventArgs> PromotionSelected;
    }
}
