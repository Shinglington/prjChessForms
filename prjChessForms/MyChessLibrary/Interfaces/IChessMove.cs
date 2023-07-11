namespace prjChessForms.MyChessLibrary
{
    public interface IChessMove
    {
        void ExecuteMove(IBoard board);
        void ReverseMove(IBoard board);

    }
}
