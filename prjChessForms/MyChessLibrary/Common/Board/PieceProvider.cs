using System.Collections.Generic;
using System.Drawing;

namespace prjChessForms.MyChessLibrary
{
    class PieceProvider : IPieceProvider
    {
        private IBoard _board;
        public void SetBoard(IBoard board) => _board = board;
        public ICollection<IPiece> GetPieces(PieceColour colour)
        {
            ICollection<IPiece> pieces = new List<IPiece>();
            IPiece p;
            for (int y = 0; y < _board.RowCount; y++)
            {
                for (int x = 0; x < _board.ColumnCount; x++)
                {
                    p = _board.GetSquareAt(new Coords(x, y)).Piece;
                    if (p != null && p.Colour == colour)
                    {
                        pieces.Add(p);
                    }
                }
            }
            return pieces;
        }

        public Coords GetCoordsOfPiece(IPiece piece)
        {
            if (piece == null) throw new System.ArgumentNullException(nameof(piece));
            for (int y = 0; y < _board.RowCount; y++)
            {
                for (int x = 0; x < _board.ColumnCount; x++)
                {
                    if (_board.GetSquares()[x, y].Piece == piece)
                    {
                        return new Coords(x, y);
                    }
                }
            }
            throw new System.ArgumentOutOfRangeException(nameof(piece), "Could not find piece");
        }
    }
}
