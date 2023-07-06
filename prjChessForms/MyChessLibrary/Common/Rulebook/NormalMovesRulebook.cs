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

        public bool CheckLegalMove(ChessMove move)
        {
            Coords start = move.StartCoords;
            Coords end = move.EndCoords;
            IPiece movedPiece = _board.GetPieceAt(move.StartCoords);
            IPiece capturedPiece = _board.GetPieceAt(move.EndCoords);
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

        public void MakeMove(ChessMove move)
        {
            if (!CheckLegalMove(move))
            {
                throw new ArgumentException(string.Format("Move {0} is not a valid move", move));
            }
            _board.MakeMove(move);
        }

        public ICollection<ChessMove> GetPossibleMovesForPiece(IPiece piece)
        {
            ICollection<ChessMove> possibleMoves = new List<ChessMove>();
            Coords pieceCoords = _board.GetCoordsOfPiece(piece);
            ChessMove move;
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
