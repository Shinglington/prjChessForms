using System;

namespace prjChessForms.MyChessLibrary.Pieces
{
    class King : Piece
    {
        public King(Player player) : base(player) { }

        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
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
