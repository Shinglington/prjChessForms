using prjChessForms.MyChessLibrary.DataClasses.ChessMoves;
using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace prjChessForms.MyChessLibrary
{
    class CastlingRulebook : IRulebook
    {
        // TO DO: DISALLOW CASTLING THROUGH CHECK
        private readonly IBoard _board;
        public CastlingRulebook(IBoard board)
        {
            _board = board;
        }

        public IChessMove ProcessChessMove(Coords StartCoords, Coords EndCoords)
        {
            IChessMove chessMove = null;
            IPiece king = _board.GetSquareAt(StartCoords).Piece;
            if (king != null && king.GetType() == typeof(King) && CheckMovementVector(StartCoords, EndCoords))
            {
                Coords rookStartCoords = GetRookStartCoords(StartCoords, EndCoords);
                Coords rookEndCoords = GetRookEndCoords(StartCoords, EndCoords);
                IPiece rook = _board.GetSquareAt(rookStartCoords).Piece;
                if (IsValidCastling(StartCoords, EndCoords, rookStartCoords, rookEndCoords))
                {
                    PieceMovement kingMovement = new PieceMovement(king, StartCoords, EndCoords);
                    PieceMovement rookMovement = new PieceMovement(rook, rookStartCoords, rookEndCoords);
                    chessMove = new ChessMove(new List<IChessMove> { rookMovement, kingMovement });
                }
            }
            return chessMove;
        }

        public ICollection<IChessMove> GetPossibleMovesForPiece(IPiece piece)
        {
            ICollection<IChessMove> possibleMoves = new List<IChessMove>();
            Coords pieceCoords = _board.GetCoordsOfPiece(piece);
            if (piece.GetType() == typeof(King))
            {

            }
            return possibleMoves;
        }

        public bool CheckFirstSelectedCoords(Coords coords)
        {
            IPiece king = _board.GetSquareAt(coords).Piece;
            return king != null && king.GetType() == typeof(King) && !king.HasMoved;
        }

        private bool IsValidCastling(Coords kingStart, Coords kingEnd, Coords rookStart, Coords rookEnd)
        {
            IPiece rook = _board.GetSquareAt(rookStart).Piece;
            IPiece king = _board.GetSquareAt(kingStart).Piece;
            return CheckClearPath(kingStart, kingEnd) && rook != null && rook.GetType() == typeof(Rook) && !king.HasMoved && !rook.HasMoved;
        }


        private Coords GetRookStartCoords(Coords KingStartCoords, Coords KingEndCoords)
        {
            int kingXMovement = KingEndCoords.X - KingStartCoords.X;
            // Rook located on either end of board
            // If king's x vector is positive, then castling rook is on the right, so biggest x coord required
            // Else, x coord of castling rook is 0 (left)
            int RookXCoord = kingXMovement > 0 ? _board.ColumnCount - 1 : 0;
            return new Coords(RookXCoord, KingStartCoords.Y);
        }

        private Coords GetRookEndCoords(Coords KingStartCoords, Coords KingEndCoords)
        {
            int kingXDirection = KingEndCoords.X - KingStartCoords.X > 0 ? 1 : -1;
            // Rook new location is adjacent to new king location (or adjacent to old king location)
            return new Coords(KingStartCoords.X + kingXDirection, KingStartCoords.Y);
        }

        private bool CheckMovementVector(Coords StartCoords, Coords EndCoords)
        {
            bool verifiedVector = false;
            if (StartCoords.Y == EndCoords.Y && Math.Abs(StartCoords.X - EndCoords.X) == 2)
            {
                verifiedVector = true;
            }
            return verifiedVector;
        }

        private bool CheckClearPath(Coords StartCoords, Coords EndCoords)
        {
            bool clearPath = true;
            int kingXDirection = EndCoords.X - StartCoords.X > 0 ? 1 : -1;
            for (int x = StartCoords.X + kingXDirection; x != EndCoords.X; x += kingXDirection)
            {
                if (_board.GetSquareAt(new Coords(x, StartCoords.Y)).Piece != null)
                {
                    clearPath = false;
                    break;
                }
            }
            return clearPath;
        }
    }
}
