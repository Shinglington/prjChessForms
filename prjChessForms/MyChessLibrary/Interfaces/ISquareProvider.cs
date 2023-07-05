namespace prjChessForms.MyChessLibrary
{
    public interface ISquareProvider
    {
        ISquare GetSquareAt(Coords coords);
        IPiece GetPieceAt(Coords coords);
    }

}
