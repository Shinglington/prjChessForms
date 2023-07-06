using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class CastlingRulebook : IRulebook
    {
        private readonly IBoard _board;
        public CastlingRulebook(IBoard board)
        {
            _board = board;
        }
        public bool CheckLegalMove(ChessMove move)
        {
            return IsCastle(move);
        }

        public ICollection<ChessMove> GetPossibleMovesForPiece(IPiece piece)
        {
            throw new NotImplementedException();
        }

        public void MakeMove(ChessMove move)
        {
            throw new NotImplementedException();
        }
        private bool IsCastle(ChessMove move)
        {
            bool isCastleMove = false;
            if (_board.GetPieceAt(move.StartCoords).GetType() == typeof(King) && !_board.GetPieceAt(move.StartCoords).HasMoved)
            {
                if (Math.Abs(move.EndCoords.Y - move.StartCoords.Y) == 0 && Math.Abs(move.EndCoords.X - move.StartCoords.X) == 2)
                {
                    int direction = move.EndCoords.X - move.StartCoords.X > 0 ? 1 : -1;
                    Coords rookCoords = direction > 0 ? new Coords(_board.ColumnCount - 1, move.StartCoords.Y) : new Coords(0, move.StartCoords.Y);
                    IPiece p = _board.GetPieceAt(rookCoords);
                    if (p != null && p.GetType() == typeof(Rook) && !p.HasMoved)
                    {
                        isCastleMove = true;
                        Coords currCoords = new Coords(move.StartCoords.X + direction, move.StartCoords.Y);
                        while (!currCoords.Equals(rookCoords))
                        {
                            if (_board.GetPieceAt(currCoords) != null || _board.CheckMoveInCheck(p.Colour, new ChessMove(move.StartCoords, currCoords)))
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
