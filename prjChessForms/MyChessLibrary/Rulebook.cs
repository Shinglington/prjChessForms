using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    public enum GameResult 
    { 
        Unfinished,
        Checkmate,
        Stalemate,
        Time
    }

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
        public static void MakeMove(Board board, Player player, ChessMove move)
        {
            if (!CheckLegalMove(board, player, move))
            {
                throw new ArgumentException(string.Format("Move {0} is not a valid move", move));
            }

            if (IsEnPassant(board, move))
            {
                GhostPawn ghostPawn = board.GetSquareAt(move.EndCoords).GetGhostPawn();
                Coords linkedPawnCoords = board.GetCoordsOfPiece(ghostPawn.LinkedPawn);
                board.GetSquareAt(linkedPawnCoords).Piece = null;
            }
            // Remove ghost pawns
            board.RemoveGhostPawns();
            if (IsDoublePawnMove(board, move))
            {
                Coords ghostPawnCoords = new Coords(move.StartCoords.X, move.StartCoords.Y + (move.EndCoords.Y - move.StartCoords.Y) / 2);
                board.GetSquareAt(ghostPawnCoords).Piece = new GhostPawn(player, (Pawn)board.GetPieceAt(move.StartCoords));
            }
            else if (IsCastle(board, move))
            {
                int direction = move.EndCoords.X - move.StartCoords.X > 0 ? 1 : -1;
                Coords rookCoords = direction > 0 ? new Coords(board.ColumnCount - 1, move.StartCoords.Y) : new Coords(0, move.StartCoords.Y);
                board.MakeMove(new ChessMove(rookCoords, new Coords(move.EndCoords.X + direction * -1, move.EndCoords.Y)));
            }
            board.MakeMove(move);
            Promotions(board, move.EndCoords);



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
                if (IsEnPassant(board, move) || IsCastle(board, move))
                {
                    legal = true;
                }
                else if (movedPiece.CanMove(board, start, end))
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


        public static GameResult GetGameResult(Board board, Player currentPlayer)
        {
            if (IsInStalemate(board, currentPlayer))
            {
                return GameResult.Stalemate;
            }
            else if (IsInCheckmate(board, currentPlayer))
            {
                return GameResult.Checkmate;
            }
            else
            {
                return GameResult.Unfinished;
            }

        }

        public static bool IsInCheck(Board board, Player currentPlayer)
        {
            bool check = false;
            King king = board.GetKing(currentPlayer.Colour);
            if (king == null)
            {
                return true;
            }
            Coords kingCoords = board.GetCoordsOfPiece(king);
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

        private static bool IsDoublePawnMove(Board board, ChessMove move)
        {
            if (board.GetPieceAt(move.StartCoords).GetType() == typeof(Pawn))
            {
                if (Math.Abs(move.EndCoords.Y - move.StartCoords.Y) == 2)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsCastle(Board board, ChessMove move)
        {
            bool isCastleMove = false;
            if (board.GetPieceAt(move.StartCoords).GetType() == typeof(King) && !board.GetPieceAt(move.StartCoords).HasMoved)
            {
                if (Math.Abs(move.EndCoords.Y - move.StartCoords.Y) == 0 && Math.Abs(move.EndCoords.X - move.StartCoords.X) == 2)
                {
                    int direction = move.EndCoords.X - move.StartCoords.X > 0 ? 1 : -1;
                    Coords rookCoords = direction > 0 ? new Coords(board.ColumnCount - 1, move.StartCoords.Y) : new Coords(0, move.StartCoords.Y);
                    Piece p = board.GetPieceAt(rookCoords);
                    if (p != null && p.GetType() == typeof(Rook) && !p.HasMoved)
                    {
                        isCastleMove = true;
                        Coords currCoords = new Coords(move.StartCoords.X + direction, move.StartCoords.Y);
                        while (!currCoords.Equals(rookCoords))
                        {
                            if (board.GetPieceAt(currCoords) != null || board.CheckMoveInCheck(p.Owner, new ChessMove(move.StartCoords, currCoords)))
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

        private static bool IsEnPassant(Board board, ChessMove move)
        {
            if (board.GetPieceAt(move.StartCoords).GetType() == typeof(Pawn))
            {
                Pawn piece = (Pawn)board.GetPieceAt(move.StartCoords);
                int legalDirection = (piece.Colour == PieceColour.White ? 1 : -1);
                if (Math.Abs(move.EndCoords.X - move.StartCoords.X) == 1 && move.EndCoords.Y - move.StartCoords.Y == legalDirection)
                {
                    GhostPawn ghostPawn = board.GetSquareAt(move.EndCoords).GetGhostPawn();
                    if (ghostPawn != null && ghostPawn.Colour != piece.Colour)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private static void Promotions(Board board, Coords endCoords)
        {
            Piece p = board.GetPieceAt(endCoords);
            if (p.GetType() == typeof(Pawn)) 
            {
                if (endCoords.Y == 0 || endCoords.Y == board.RowCount - 1)
                {
                    board.GetSquareAt(endCoords).Piece = new Queen(p.Owner);
                }
            }
        }

    }
}
