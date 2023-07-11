﻿using System;

namespace prjChessForms.MyChessLibrary.Pieces
{
    class Pawn : Piece
    {
        public Pawn(PieceColour colour) : base(colour) { }

        public override bool CanMove(IBoard board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            int direction = yChange > 0 ? 1 : -1;
            if (direction != (Colour == PieceColour.White ? 1 : -1))
            {
                return false;
            }
            if (xChange == 0 && board.GetSquareAt(endCoords).Piece == null)
            {
                if (Math.Abs(yChange) == 1)
                {
                    allowed = true;
                }
                else if (Math.Abs(yChange) == 2 && !HasMoved
                    && board.GetSquareAt(new Coords(startCoords.X, startCoords.Y + direction)).Piece == null)
                {
                    allowed = true;
                }
            }
            if (Math.Abs(xChange) == 1 && Math.Abs(yChange) == 1)
            {
                if (board.GetSquareAt(endCoords).Piece != null)
                {
                    allowed = true;
                }
            }
            return allowed;
        }
    }
}
