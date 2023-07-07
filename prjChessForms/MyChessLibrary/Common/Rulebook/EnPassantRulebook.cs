using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class EnPassantRulebook : IRulebook
    {
        private readonly IBoard _board;
        public EnPassantRulebook(IBoard board) 
        {
            _board = board;
        }
        public bool CheckLegalMove(Move move)
        {
            return IsEnPassant(move);
        }

        public ICollection<Move> GetPossibleMovesForPiece(IPiece p)
        {
            throw new NotImplementedException();
        }

        public void MakeMove(Move move)
        {
            throw new NotImplementedException();
        }
        private bool IsEnPassant(Move move)
        {
            if (_board.GetSquareAt(move.StartCoords).Piece.GetType() == typeof(Pawn))  
            {
                Pawn piece = (Pawn)_board.GetSquareAt(move.StartCoords).Piece;
                int legalDirection = (piece.Colour == PieceColour.White ? 1 : -1);
                if (Math.Abs(move.EndCoords.X - move.StartCoords.X) == 1 && move.EndCoords.Y - move.StartCoords.Y == legalDirection)
                {
                    GhostPawn ghostPawn = _board.GetSquareAt(move.EndCoords).GetGhostPawn();
                    if (ghostPawn != null && ghostPawn.Colour != piece.Colour)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
