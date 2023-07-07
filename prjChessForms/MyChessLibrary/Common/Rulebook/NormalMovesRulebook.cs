using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class NormalMovesRulebook : IRulebook
    {
        private readonly IBoard _board;
        public NormalMovesRulebook(IBoard board)
        {
            _board = board;
        }

        public bool CheckLegalMove(Move move)
        {
            Coords start = move.StartCoords;
            Coords end = move.EndCoords;
            IPiece movedPiece = _board.GetSquareAt(move.StartCoords).Piece;
            IPiece capturedPiece = _board.GetSquareAt(move.EndCoords).Piece;
            bool legal = false;
            if (movedPiece != null && !start.Equals(end))
            {
                PieceColour colour = movedPiece.Colour;
                if (movedPiece.CanMove(_board, move))
                {
                    if (capturedPiece == null || capturedPiece.Colour != colour)
                    {
                        if (!_board.CheckMoveInCheck(colour, move))
                        {
                            legal = true;
                        }
                    }
                }
            }
            return legal;
        }

        public void MakeMove(Move move)
        {
            if (!CheckLegalMove(move))
            {
                throw new ArgumentException(string.Format("Move {0} is not a valid move", move));
            }
            _board.MakeMove(move);
        }

        public ICollection<Move> GetPossibleMovesForPiece(IPiece piece)
        {
            ICollection<Move> possibleMoves = new List<Move>();
            Coords pieceCoords = _board.GetCoordsOfPiece(piece);
            Move move;
            for (int y = 0; y < _board.RowCount; y++)
            {
                for (int x = 0; x < _board.ColumnCount; x++)
                {
                    move = new ChessMove(pieceCoords, new Coords(x, y));
                    if (CheckLegalMove(move))
                    {
                        possibleMoves.Add(move);
                    }
                }
            }
            return possibleMoves;
        }
    }
}
