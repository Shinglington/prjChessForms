namespace prjChessForms.MyChessLibrary
{
    public interface IBoard
    {
        int ColumnCount { get; }
        int RowCount { get; }
        ISquare[,] GetSquares();
        void SetSquares(ISquare[,] squares);
        ISquare GetSquareAt(Coords coords);
        IPiece GetPieceAt(Coords coords);
    }








}
