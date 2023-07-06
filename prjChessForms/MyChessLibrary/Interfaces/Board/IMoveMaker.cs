namespace prjChessForms.MyChessLibrary
{
    public interface IMoveMaker
    {
        void SetBoard(IBoard board);
        void MakeMove(ChessMove move);
        void UndoLastMove();
        ChessMove GetLastMove(ChessMove move);
    }
}
