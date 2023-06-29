using System;

namespace prjChessForms.MyChessLibrary.Pieces
{
    class Rook : Piece
    {
        public Rook(Player player) : base(player) { }

        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            if (xChange == 0 && yChange != 0)
            {
                allowed = true;
                int direction = yChange > 0 ? 1 : -1;
                for (int deltaY = 1; deltaY < Math.Abs(yChange); deltaY += 1)
                {
                    Coords checkCoords = new Coords(startCoords.X, startCoords.Y + deltaY * direction);
                    if (board.GetPieceAt(checkCoords) != null)
                    {
                        allowed = false;
                        break;
                    }
                }
            }
            else if (yChange == 0 && xChange != 0)
            {
                allowed = true;
                int direction = xChange > 0 ? 1 : -1;
                for (int deltaX = 1; deltaX < Math.Abs(xChange); deltaX += 1)
                {
                    Coords checkCoords = new Coords(startCoords.X + deltaX * direction, startCoords.Y);
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
