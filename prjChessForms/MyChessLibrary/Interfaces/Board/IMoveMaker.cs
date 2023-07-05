namespace prjChessForms.MyChessLibrary
{
    public interface IMoveMaker
    {
        void SetBoard(IBoard board);
        void MakeMove(ChessMove Move);
    }
}
