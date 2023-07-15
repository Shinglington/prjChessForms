using prjChessForms.MyChessLibrary.Pieces;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{

    class FullRulebook : IRulebook
    {
        private readonly IBoard _board;
        private readonly ICollection<IRulebook> _rulebooks;
        private readonly ICheckHandler _checkHandler;
        public FullRulebook(IBoard board, ICollection<IRulebook> rulebooks, ICheckHandler checkHandler)
        {
            _board = board;
            _rulebooks = rulebooks;
            _checkHandler = checkHandler;
            _checkHandler.AddRulebook(this);
        }

        public IChessMove ProcessChessMove(PieceColour colourOfMover, Coords startCoords, Coords endCoords)
        {
            foreach (IRulebook rulebook in _rulebooks)
            {
                IChessMove processedMove = rulebook.ProcessChessMove(colourOfMover, startCoords, endCoords);
                if (processedMove != null && !MovePutsPlayerInCheck(colourOfMover, processedMove))
                {
                    return processedMove;
                }
            }
            return null;
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

        public bool CheckFirstSelectedCoords(PieceColour colourOfMover, Coords coords)
        {
            bool validFirstSelection = false;
            foreach (IRulebook rulebook in _rulebooks)
            {
                validFirstSelection = rulebook.CheckFirstSelectedCoords(colourOfMover, coords);
                if (validFirstSelection)
                {
                    break;
                }
            }
            return validFirstSelection;
        }

        private bool MovePutsPlayerInCheck(PieceColour colourOfMover, IChessMove chessMove)
        {
            chessMove.ExecuteMove(_board);
            bool putsPlayerInCheck = _checkHandler.IsInCheck(colourOfMover);
            chessMove.ReverseMove(_board);
            return putsPlayerInCheck;
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
    }
}
