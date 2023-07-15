using prjChessForms.MyChessLibrary.Pieces;

namespace prjChessForms.MyChessLibrary
{
    class CheckHandler : ICheckHandler
    {
        private readonly IBoard _board;
        private IRulebook _rulebook;

        public CheckHandler(IBoard board)
        {
            _board = board;
        }

        public void AddRulebook(IRulebook rulebook)
        {
            _rulebook = rulebook;
        }

        public bool IsInCheck(PieceColour colour)
        {
            bool check = false;
            IPiece king = GetKing(colour);
            Coords kingCoords = _board.GetCoordsOfPiece(king);

            PieceColour oppositeColour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;

            foreach (IPiece piece in _board.GetPieces(oppositeColour))
            {
                if (piece.CanMove(_board, _board.GetCoordsOfPiece(piece), kingCoords))
                {
                    check = true;
                    break;
                }
            }
            return check;
        }

        public bool IsInCheckmate(PieceColour colour)
        {
            return IsInCheck(colour) && !CheckIfThereAreRemainingMovesAvailable(colour);
        }

        private IPiece GetKing(PieceColour colour)
        {
            foreach(IPiece piece in _board.GetPieces(colour))
            {
                if (piece.GetType() == typeof(King))
                {
                    return piece;
                }
            }
            throw new System.Exception($"Could not locate king of colour {colour}");
        }

        private bool CheckIfThereAreRemainingMovesAvailable(PieceColour colour)
        {
            bool remainingMoves = false;
            foreach(IPiece piece in _board.GetPieces(colour))
            {
                if (_rulebook.GetPossibleMovesForPiece(piece).Count > 0)
                {
                    remainingMoves = true;
                    break;
                }
            }
            return remainingMoves;
        }
    }
}
