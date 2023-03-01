using System;
using System.Collections.Generic;

namespace prjChessForms
{
    public struct ChessMove
    {
        public ChessMove(Coords startCoords, Coords endCoords)
        {
            StartCoords = startCoords;
            EndCoords = endCoords;
        }

        public Coords StartCoords { get; }
        public Coords EndCoords { get; }
        public override string ToString()
        {
            return StartCoords.ToString() + " -> " + EndCoords.ToString();
        }
    }
    class Rulebook
    {
        public enum GameResult
        {
            Checkmate,
            Stalemate,
            Time
        }

        public static void MakeMove(Board board, Player player, ChessMove move)
        {
            if (!CheckLegalMove(board, player, move))
            {
                throw new ArgumentException(string.Format("Move {0} is not a valid move", move));
            }

            board.MakeMove(move);



        }

        public static bool CheckLegalMove(Board board, Player player, ChessMove move)
        {
            Coords start = move.StartCoords;
            Coords end = move.EndCoords;

            bool legal = false;
            Piece movedPiece = board.GetPieceAt(start);
            Piece capturedPiece = board.GetPieceAt(end);
            if (movedPiece != null && movedPiece.Colour == player.Colour && !start.Equals(end))
            {
                if (movedPiece.CanMove(board, start, end))
                {
                    if (capturedPiece == null || (capturedPiece.Colour != player.Colour))
                    {
                        if (!board.CheckMoveInCheck(player, move))
                        {
                            legal = true;
                        }
                    }
                }
            }
            Console.Write(move.ToString() + " is ");

            if (legal)
            {
                Console.Write("legal");
            }
            else
            {
                Console.Write("illegal");
            }
            Console.WriteLine();
            return legal;
        }
        public static List<ChessMove> GetPossibleMoves(Board board, Piece p)
        {
            List<ChessMove> possibleMoves = new List<ChessMove>();
            Coords pieceCoords = board.GetCoordsOfPiece(p);
            if (board.GetPieceAt(pieceCoords) != null)
            {
                Piece piece = board.GetPieceAt(pieceCoords);
                ChessMove move;
                for (int y = 0; y < board.RowCount; y++)
                {
                    for (int x = 0; x < board.ColumnCount; x++)
                    {
                        move = new ChessMove(pieceCoords, new Coords(x, y));
                        if (CheckLegalMove(board, piece.Owner, move))
                        {
                            possibleMoves.Add(move);
                        }
                    }
                }
            }
            return possibleMoves;
        }

        public static bool CheckIfGameOver(Board board, Player currentPlayer)
        {
            return IsInStalemate(board, currentPlayer) || IsInCheckmate(board, currentPlayer);
        }

        public static bool IsInCheck(Board board, Player currentPlayer)
        {
            bool check = false;
            Coords kingCoords = board.GetCoordsOfPiece(board.GetKing(currentPlayer.Colour));
            foreach (Square square in board.GetSquares())
            {
                if (square.Piece != null && square.Piece.Owner != currentPlayer)
                {
                    if (CheckLegalMove(board, square.Piece.Owner, new ChessMove(square.Coords, kingCoords)))
                    {
                        check = true;
                        break;
                    }
                }
            }
            return check;
        }

        private static bool IsInCheckmate(Board board, Player currentPlayer)
        {
            if (!IsInCheck(board, currentPlayer))
            {
                return false;
            }
            return !CheckIfThereAreRemainingLegalMoves(board, currentPlayer);
        }

        private static bool IsInStalemate(Board board, Player currentPlayer)
        {
            if (IsInCheck(board, currentPlayer))
            {
                return false;
            }
            return !CheckIfThereAreRemainingLegalMoves(board, currentPlayer);
        }
        private static bool CheckIfThereAreRemainingLegalMoves(Board board, Player currentPlayer)
        {
            bool anyLegalMoves = false;
            foreach (Piece p in board.GetPieces(currentPlayer.Colour))
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
