using System;

namespace prjChessForms.MyChessLibrary.DataClasses.ChessMoves
{
    public class PieceRemoval : IChessMove
    {
        public PieceRemoval(IPiece capturedPiece, Coords originalCoords)
        {
            CapturedPiece = capturedPiece;
            OriginalCoords = originalCoords;
        }
        public IPiece CapturedPiece { get; }
        public Coords OriginalCoords { get; }

        public void ExecuteMove(IBoard board)
        {
            if (board.GetSquareAt(OriginalCoords).Piece != CapturedPiece)
            {
                throw new Exception("Piece was not in expected position");
            }
            board.GetSquareAt(OriginalCoords).Piece = null;
        }

        public void ReverseMove(IBoard board)
        {
            if (board.GetSquareAt(OriginalCoords).Piece != null)
            {
                throw new Exception("Position is not empty, so cannot place Piece");
            }
            board.GetSquareAt(OriginalCoords).Piece = CapturedPiece;
        }
    }
}
