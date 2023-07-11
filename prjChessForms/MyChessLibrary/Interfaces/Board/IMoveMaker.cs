namespace prjChessForms.MyChessLibrary
{
    public interface IMoveMaker
    {
        void SetBoard(IBoard board);
        void MakeMove(IChessMove move);
        void UndoLastMove();
        IChessMove GetLastMove();
    }
}
