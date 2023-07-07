using System;

namespace prjChessForms.MyChessLibrary.Pieces
{
    class King : Piece
    {
        public King(PieceColour colour) : base(colour) { }

        public override bool CanMove(IBoard board, Move move)
        {
            Coords startCoords = move.StartCoords;
            Coords endCoords = move.EndCoords;
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            if (Math.Abs(xChange) <= 1 && Math.Abs(yChange) <= 1)
            {
                allowed = true;
            }
            return allowed;
        }
    }
}
