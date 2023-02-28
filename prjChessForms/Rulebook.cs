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

        public static bool CheckLegalMove(Board board, Player player, ChessMove move)
        {
            Coords start = move.StartCoords;
            Coords end = move.EndCoords;

            bool legal = false;
            Piece movedPiece = board.GetPieceAt(start);
            Piece capturedPiece = board.GetPieceAt(end);
            if (movedPiece != null && movedPiece.Colour == player.Colour && !start.Equals(end))
            {
                Console.WriteLine("passes first checks");
                if (movedPiece.CanMove(board, start, end))
                {
                    Console.WriteLine("Movable");
                    if (capturedPiece == null || (capturedPiece.Colour != player.Colour))
                    {
                        legal = true;
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
        public static List<Coords> GetPossibleMoves(Board board, Coords pieceCoords)
        {
            List<Coords> possibleMoves = new List<Coords>();
            if (board.GetPieceAt(pieceCoords) != null)
            {
                Piece piece = board.GetPieceAt(pieceCoords);
                Coords checkingCoords;
                for (int y = 0; y < board.RowCount; y++)
                {
                    for (int x = 0; x < board.ColumnCount; x++)
                    {
                        checkingCoords = new Coords(x, y);
                        if (CheckLegalMove(board, piece.Owner, new ChessMove(pieceCoords, checkingCoords)))
                        {
                            possibleMoves.Add(checkingCoords);
                        }
                    }
                }
            }
            return possibleMoves;
        }

        public static bool CheckIfGameOver(Board board, Player currentPlayer)
        {
            return false;
        }

        public static bool IsCheck(Board board, Player currentPlayer)
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

        private static bool IsCheckmate(Board board, Player currentPlayer)
        {
            if (!IsCheck(board, currentPlayer))
            {
                return false;
            }

            bool checkmate = false;



            return checkmate;

        }

    }
}
