using System;
namespace prjChessForms.MyChessLibrary.Pieces
{
    class Bishop : Piece
    {
        public Bishop(PieceColour colour) : base(colour) { }

        public override bool CanMove(IBoard board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            if (Math.Abs(xChange) == Math.Abs(yChange))
            {
                allowed = true;
                int xDirection = xChange > 0 ? 1 : -1;
                int yDirection = yChange > 0 ? 1 : -1;
                for (int delta = 1; delta < Math.Abs(xChange); delta += 1)
                {
                    Coords checkCoords = new Coords(startCoords.X + delta * xDirection, startCoords.Y + delta * yDirection);
                    if (board.GetPieceAt(checkCoords) != null)
                    {
                        allowed = false;
                        break;
                    }
                }
            }
            return allowed;
        }
    }
}
