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

        public static bool CheckIfGameOver(Board board, Player playerTurn)
        {
            return false;
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
                        if (CheckLegalMove(board, piece.Owner, pieceCoords, checkingCoords))
                        {
                            possibleMoves.Add(checkingCoords);
                        }
                    }
                }
            }
            return possibleMoves;
        }




    }
}
