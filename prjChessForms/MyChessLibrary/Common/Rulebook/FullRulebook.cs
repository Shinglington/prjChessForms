using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{

    class FullRulebook : IRulebook
    {
        private readonly IBoard _board;
        private readonly ICollection<IRulebook> _rulebooks;
        public FullRulebook(IBoard board, ICollection<IRulebook> rulebooks)
        {
            _board = board;
            _rulebooks = rulebooks;
        }

        public bool CheckLegalMove(ChessMove move)
        {
            foreach(IRulebook rulebook in _rulebooks)
            {
                if (rulebook.CheckLegalMove(move))
                {
                    return true;
                }
            }
            return false;
        }

        public ICollection<ChessMove> GetPossibleMovesForPiece(IPiece piece)
        {
            ICollection<ChessMove> PossibleMoves = new List<ChessMove>();
            foreach (IRulebook rulebook in _rulebooks)
            {
                foreach(ChessMove move in rulebook.GetPossibleMovesForPiece(piece))
                {
                    PossibleMoves.Add(move);
                }
            }
            return PossibleMoves;
        }

        public void MakeMove(ChessMove move)
        {
            foreach (IRulebook rulebook in _rulebooks)
            {
                if (rulebook.CheckLegalMove(move))
                {
                    try
                    {
                        rulebook.MakeMove(move);
                        return;
                    }
                    catch { }
                }
            }
            throw new ArgumentException(string.Format("{0} is not a legal move", move));
        }

        public bool RequiresPromotion(Coords pieceCoords)
        {
            bool requiresPromotion = false;
            IPiece p = _board.GetPieceAt(pieceCoords);
            if (p.GetType() == typeof(Pawn))
            {
                if (pieceCoords.Y == 0 || pieceCoords.Y == _board.RowCount - 1)
                {
                    requiresPromotion = true;
                }
            }
            return requiresPromotion;
        }

        public static bool IsInCheck(Board board, PieceColour colour)
        {
            bool check = false;
            King king = board.GetKing(colour);
            if (king == null)
            {
                return true;
            }
            Coords kingCoords = board.GetCoordsOfPiece(king);
            foreach (Square square in board.GetSquares())
            {
                if (square.Piece != null && square.Piece.Colour != colour)
                {
                    if (CheckLegalMove(board, square.Piece.Colour, new ChessMove(square.Coords, kingCoords)))
                    {
                        check = true;
                        break;
                    }
                }
            }
            return check;
        }

    }
}
