using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class GameFinishedChecker : IGameFinishedChecker
    {
        public static GameResult GetGameResult(Board board, Player current)
        {
            if (IsInStalemate(board, current.Colour))
            {
                return GameResult.Stalemate;
            }
            else if (IsInCheckmate(board, current.Colour))
            {
                return GameResult.Checkmate;
            }
            else
            {
                return GameResult.Unfinished;
            }

        }
        private static bool IsInCheckmate(Board board, PieceColour colour)
        {
            if (!IsInCheck(board, colour))
            {
                return false;
            }
            return !CheckIfThereAreRemainingLegalMoves(board, colour);
        }

        private static bool IsInStalemate(Board board, PieceColour colour)
        {
            if (IsInCheck(board, colour))
            {
                return false;
            }
            return !CheckIfThereAreRemainingLegalMoves(board, colour);
        }

        private static bool CheckIfThereAreRemainingLegalMoves(Board board, PieceColour colour)
        {
            bool anyLegalMoves = false;
            foreach (Piece p in board.GetPieces(colour))
            {
                List<ChessMove> moves = GetPossibleMoves(board, p);
                if (moves.Count > 0)
                {
                    anyLegalMoves = true;
                    break;
                }
            }
            return anyLegalMoves;
        }
    }
}
