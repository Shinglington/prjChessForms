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

        public IChessMove ProcessChessMove(Coords StartCoords, Coords EndCoords)
        {
            IChessMove processedMove = null;
            foreach (IRulebook rulebook in _rulebooks)
            {
                processedMove = rulebook.ProcessChessMove(StartCoords, EndCoords);
                if (processedMove != null)
                {
                    break;
                }
            }
            return processedMove;
        }

        public ICollection<IChessMove> GetPossibleMovesForPiece(IPiece piece)
        {
            ICollection<IChessMove> PossibleMoves = new List<IChessMove>();
            foreach (IRulebook rulebook in _rulebooks)
            {
                foreach (IChessMove move in rulebook.GetPossibleMovesForPiece(piece))
                {
                    PossibleMoves.Add(move);
                }
            }
            return PossibleMoves;
        }

        public bool CheckFirstSelectedCoords(Coords coords)
        {
            bool validFirstSelection = false;
            foreach (IRulebook rulebook in _rulebooks)
            {
                validFirstSelection = rulebook.CheckFirstSelectedCoords(coords);
                if (validFirstSelection)
                {
                    break;
                }
            }
            return validFirstSelection;
        }


        public bool RequiresPromotion(Coords pieceCoords)
        {
            bool requiresPromotion = false;
            IPiece p = _board.GetSquareAt(pieceCoords).Piece;
            if (p.GetType() == typeof(Pawn))
            {
                if (pieceCoords.Y == 0 || pieceCoords.Y == _board.RowCount - 1)
                {
                    requiresPromotion = true;
                }
            }
            return requiresPromotion;
        }

        public bool IsInCheck(IBoard board, PieceColour colour)
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
                    if (ProcessChessMove(square.Coords, kingCoords) != null)
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
