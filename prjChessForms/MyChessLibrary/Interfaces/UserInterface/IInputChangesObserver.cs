namespace prjChessForms.MyChessLibrary
{
    public interface IInputChangesObserver
    {
        void OnSquareClicked(object sender, SquareClickedEventArgs e);
        void OnPromotionChosen(object sender, PromotionSelectedEventArgs e);
    }
}
