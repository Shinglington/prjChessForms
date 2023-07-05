namespace prjChessForms.MyChessLibrary
{
    public interface ISquareProvider
    {
        void SetBoard(IBoard board);
        ISquare GetSquareAt(Coords coords);
    }

}
