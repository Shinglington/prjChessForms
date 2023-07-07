namespace prjChessForms.MyChessLibrary
{
    public interface IChessMove
    {
        string ToString();
        void ExecuteMove(IBoard board);
        void ReverseMove(IBoard board);
    }
}
