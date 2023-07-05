namespace prjChessForms.MyChessLibrary
{
    class SquareProvider : ISquareProvider
    {
        private IBoard _board;
        public void SetBoard(IBoard board) => _board = board;
        public ISquare GetSquareAt(Coords coords) => _board.GetSquares()[coords.X, coords.Y];
        public IPiece GetPieceAt(Coords coords) => GetSquareAt(coords).Piece;

    }
}
