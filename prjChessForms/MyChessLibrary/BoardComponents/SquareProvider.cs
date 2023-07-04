using prjChessForms.MyChessLibrary.Interfaces;

namespace prjChessForms.MyChessLibrary.BoardComponents
{
    public class SquareProvider : ISquareProvider
    {
        private readonly IBoard _board;
        public SquareProvider(IBoard board) 
        {
            _board = board;
        }

        public ISquare GetSquareAt(Coords coords)
        {
            return _board.GetSquares()[coords.X, coords.Y];
        }

        public IPiece GetPieceAt(Coords coords)
        {
            return GetSquareAt(coords).Piece;
        }

    }
}
