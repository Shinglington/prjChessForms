namespace prjChessForms.MyChessLibrary
{
    public interface IMoveMaker
    {
        void SetBoard(IBoard board);
        void MakeMove(Move move);
        void UndoLastMove();
        Move GetLastMove(Move move);
    }
}
