using System;
using System.Collections.Generic;
using prjChessForms.MyChessLibrary.Pieces;

namespace prjChessForms.MyChessLibrary
{
    public enum GameResult 
    { 
        Unfinished,
        Checkmate,
        Stalemate,
        Time
    }
    class Rulebook
    {
        public static Piece MakeMove(Board board, PieceColour colour, ChessMove move)
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

        public static bool CheckLegalMove(Board board, PieceColour colour, ChessMove move)
        {
            Coords start = move.StartCoords;
            Coords end = move.EndCoords;

            bool legal = false;
            Piece movedPiece = board.GetPieceAt(start);
            Piece capturedPiece = board.GetPieceAt(end);
            if (movedPiece != null && movedPiece.Colour == colour && !start.Equals(end))
            {
                if (IsEnPassant(board, move) || IsCastle(board, move))
                {
                    legal = true;
                }
                else if (movedPiece.CanMove(board, start, end))
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
                        if (CheckLegalMove(board, p.Colour, move))
                        {
                            possibleMoves.Add(move);
                        }
                    }
                }
            }
            return possibleMoves;
        }

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

        public static bool IsInCheck(Board board, PieceColour colour)
        {
            bool check = false;
            King king = board.GetKing(colour);
            if (king == null)
            {
                return true;
            }
            Coords kingCoords = board.GetCoordsOfPiece(king);
            foreach (Square square in board.GetSquares())
            {
                if (square.Piece != null && square.Piece.Colour != colour)
                {
                    if (CheckLegalMove(board, square.Piece.Colour, new ChessMove(square.Coords, kingCoords)))
                    {
                        check = true;
                        break;
                    }
                }
            }
            return check;
        }

        public static bool RequiresPromotion(Board board, Coords pieceCoords)
        {
            bool requiresPromotion = false;
            Piece p = board.GetPieceAt(pieceCoords);
            if (p.GetType() == typeof(Pawn))
            {
                if (pieceCoords.Y == 0 || pieceCoords.Y == board.RowCount - 1)
                {
                    requiresPromotion = true;
                }
            }
            return requiresPromotion;
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
                            if (board.GetPieceAt(currCoords) != null || board.CheckMoveInCheck(p.Colour, new ChessMove(move.StartCoords, currCoords)))
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
    }
}
