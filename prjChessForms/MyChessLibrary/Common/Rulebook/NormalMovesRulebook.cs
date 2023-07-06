using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;

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

            bool legal = false;
            IPiece movedPiece = _board.GetPieceAt(start);
            IPiece capturedPiece = _board.GetPieceAt(end);
            if (movedPiece != null && !start.Equals(end))
            {
                if (movedPiece.CanMove(start, end))
                {
                    if (capturedPiece == null || (capturedPiece.Colour != colour))
                    {
                        if (!board.CheckMoveInCheck(colour, move))
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
            if (!CheckLegalMove(board, colour, move))
            {
                throw new ArgumentException(string.Format("Move {0} is not a valid move", move));
            }

            Piece capturedPiece = board.GetPieceAt(move.EndCoords);
            if (IsEnPassant(board, move))
            {
                GhostPawn ghostPawn = board.GetSquareAt(move.EndCoords).GetGhostPawn();
                Coords linkedPawnCoords = board.GetCoordsOfPiece(ghostPawn.LinkedPawn);
                capturedPiece = ghostPawn.LinkedPawn;
                board.GetSquareAt(linkedPawnCoords).Piece = null;
            }
            // Remove ghost pawns
            board.RemoveGhostPawns();
            if (IsDoublePawnMove(board, move))
            {
                Coords ghostPawnCoords = new Coords(move.StartCoords.X, move.StartCoords.Y + (move.EndCoords.Y - move.StartCoords.Y) / 2);
                board.GetSquareAt(ghostPawnCoords).Piece = new GhostPawn(colour, (Pawn)board.GetPieceAt(move.StartCoords));
            }
            else if (IsCastle(board, move))
            {
                int direction = move.EndCoords.X - move.StartCoords.X > 0 ? 1 : -1;
                Coords rookCoords = direction > 0 ? new Coords(board.ColumnCount - 1, move.StartCoords.Y) : new Coords(0, move.StartCoords.Y);
                board.MakeMove(new ChessMove(rookCoords, new Coords(move.EndCoords.X + direction * -1, move.EndCoords.Y)));
            }
            board.MakeMove(move);
            return capturedPiece;
        }

        public ICollection<ChessMove> GetPossibleMoves(IPiece piece)
        {
            List<ChessMove> possibleMoves = new List<ChessMove>();
            Coords pieceCoords = board.GetCoordsOfPiece(piece);
            if (board.GetPieceAt(pieceCoords) != null)
            {
                Piece piece = board.GetPieceAt(pieceCoords);
                ChessMove move;
                for (int y = 0; y < board.RowCount; y++)
                {
                    for (int x = 0; x < board.ColumnCount; x++)
                    {
                        move = new ChessMove(pieceCoords, new Coords(x, y));
                        if (CheckLegalMove(board, piece.Colour, move))
                        {
                            possibleMoves.Add(move);
                        }
                    }
                }
            }
            return possibleMoves;
        }
    }
}
