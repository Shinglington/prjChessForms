namespace prjChessForms.MyChessLibrary
{
    public interface ISquareProvider
    {
        ISquare GetSquareAt(Coords coords);
    }

}
