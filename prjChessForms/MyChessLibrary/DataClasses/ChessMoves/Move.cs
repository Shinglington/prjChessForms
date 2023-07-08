using System;

namespace prjChessForms.MyChessLibrary
{
    public class Move : IChessMove
    {
        public Move(IPiece piece, Coords startCoords, Coords endCoords)
        {
            MovedPiece = piece;
            StartCoords = startCoords;
            EndCoords = endCoords;
        }
        public IPiece MovedPiece { get; }
        public Coords StartCoords { get; }
        public Coords EndCoords { get; }

        public void ExecuteMove(IBoard board)
        {
            if (board.GetSquareAt(StartCoords).Piece != MovedPiece)
            {
                throw new Exception("Piece was not in expected position");
            }
            board.GetSquareAt(StartCoords).Piece = null;
            board.GetSquareAt(EndCoords).Piece = MovedPiece;
        }

        public void ReverseMove(IBoard board)
        {
            if (board.GetSquareAt(EndCoords).Piece != MovedPiece)
            {
                throw new Exception("Piece was not in expected position");
            }
            board.GetSquareAt(EndCoords).Piece = null;
            board.GetSquareAt(StartCoords).Piece = MovedPiece;
        }

        public override string ToString()
        {
            return StartCoords.ToString() + " -> " + EndCoords.ToString();
        }
    }
}
