namespace prjChessForms.MyChessLibrary
{
    public interface ISquare
    {
        IPiece Piece { get; set; }
        Coords Coords { get; }
    }
}
