﻿using System;

namespace prjChessForms.MyChessLibrary.Pieces
{
    class Pawn : Piece
    {
        public Pawn(Player player) : base(player) { }

        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            int direction = yChange > 0 ? 1 : -1;
            if (direction != (Owner.Colour == PieceColour.White ? 1 : -1))
            {
                return false;
            }
            if (xChange == 0 && board.GetPieceAt(endCoords) == null)
            {
                if (Math.Abs(yChange) == 1)
                {
                    allowed = true;
                }
                else if (Math.Abs(yChange) == 2 && !HasMoved
                    && board.GetPieceAt(new Coords(startCoords.X, startCoords.Y + direction)) == null)
                {
                    allowed = true;
                }
            }
            if (Math.Abs(xChange) == 1 && Math.Abs(yChange) == 1)
            {
                if (board.GetPieceAt(endCoords) != null)
                {
                    allowed = true;
                }
            }
            return allowed;
        }
    }
}