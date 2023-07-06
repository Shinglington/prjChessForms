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
        public bool CheckLegalMove(ChessMove move)
        {
            return IsEnPassant(move);
        }

        public ICollection<ChessMove> GetPossibleMovesForPiece(IPiece p)
        {
            throw new NotImplementedException();
        }

        public void MakeMove(ChessMove move)
        {
            throw new NotImplementedException();
        }
        private bool IsEnPassant(ChessMove move)
        {
            if (_board.GetPieceAt(move.StartCoords).GetType() == typeof(Pawn))
            {
                Pawn piece = (Pawn)_board.GetPieceAt(move.StartCoords);
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
