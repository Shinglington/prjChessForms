using prjChessForms.PresentationUI;

namespace prjChessForms.MyChessLibrary
{
    public interface IChessInputController
    {
        void OnBoardClickReceived(object sender, SquareClickedEventArgs e);
        void OnPromotionReceived(object sender, PromotionSelectedEventArgs e);
    }
}
