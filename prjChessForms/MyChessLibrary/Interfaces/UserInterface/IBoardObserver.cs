namespace prjChessForms.MyChessLibrary
{
    public interface IBoardObserver
    {
        void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e);
    }
}
