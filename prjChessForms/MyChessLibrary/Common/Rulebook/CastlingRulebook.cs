using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class CastlingRulebook : IRulebook
    {
        private readonly IBoard _board;
        public CastlingRulebook(IBoard board)
        {
            _board = board;
        }

        public bool CheckLegalMove(Move move)
        {
            return IsCastle(move);
        }

        public ICollection<Move> GetPossibleMovesForPiece(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void MakeMove(Move move)
        {
            throw new NotImplementedException();
        }

        private bool IsCastle(Move move)
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
    }
}
