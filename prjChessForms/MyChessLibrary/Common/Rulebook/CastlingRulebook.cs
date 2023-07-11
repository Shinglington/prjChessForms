using prjChessForms.MyChessLibrary.DataClasses.ChessMoves;
using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;

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
                if (CheckClearPath(StartCoords, EndCoords)
                    && rook != null && rook.GetType() == typeof(Rook)
                    && !king.HasMoved && !rook.HasMoved)
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

        private bool IsCastle(PieceMovement move)
        {
            bool isCastleMove = false;
            if (_board.GetSquareAt(move.StartCoords).Piece.GetType() == typeof(King) && !_board.GetSquareAt(move.StartCoords).Piece.HasMoved)
            {
                if (Math.Abs(move.EndCoords.Y - move.StartCoords.Y) == 0 && Math.Abs(move.EndCoords.X - move.StartCoords.X) == 2)
                {
                    int direction = move.EndCoords.X - move.StartCoords.X > 0 ? 1 : -1;
                    Coords rookCoords = direction > 0 ? new Coords(_board.ColumnCount - 1, move.StartCoords.Y) : new Coords(0, move.StartCoords.Y);
                    IPiece p = _board.GetSquareAt(rookCoords).Piece;
                    if (p != null && p.GetType() == typeof(Rook) && !p.HasMoved)
                    {
                        isCastleMove = true;
                        Coords currCoords = new Coords(move.StartCoords.X + direction, move.StartCoords.Y);
                        while (!currCoords.Equals(rookCoords))
                        {
                            if (_board.GetSquareAt(currCoords).Piece != null || _board.CheckMoveInCheck(p.Colour, new ChessMove(move.StartCoords, currCoords)))
                            {
                                isCastleMove = false;
                                break;
                            }
                            currCoords = new Coords(currCoords.X + direction, currCoords.Y);
                        }
                    }
                }
            }
            return isCastleMove;
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
