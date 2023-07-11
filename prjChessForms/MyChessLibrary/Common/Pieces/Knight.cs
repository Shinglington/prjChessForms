using System;

namespace prjChessForms.MyChessLibrary.Pieces
{
    class Knight : Piece
    {
        public Knight(PieceColour colour) : base(colour) { }

        public override bool CanMove(IBoard board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = Math.Abs(endCoords.X - startCoords.X);
            int yChange = Math.Abs(endCoords.Y - startCoords.Y);
            if ((xChange == 2 && yChange == 1) || (xChange == 1 && yChange == 2))
            {
                allowed = true;
            }
            return allowed;
        }
    }
}
