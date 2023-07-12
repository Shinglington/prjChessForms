using prjChessForms.MyChessLibrary.DataClasses.ChessMoves;
using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class EnPassantRulebook : IRulebook
    {
        private readonly IBoard _board;
        public EnPassantRulebook(IBoard board)
        {
            _board = board;
        }

        public IChessMove ProcessChessMove(PieceColour colourOfMover, Coords startCoords, Coords endCoords)
        {
            IChessMove chessMove = null;
            // Below only true if the piece at startCoords is a pawn
            if (CheckFirstSelectedCoords(colourOfMover, startCoords))
            {
                IPiece movingPiece = _board.GetSquareAt(startCoords).Piece;
                IPiece capturedPiece = GetCapturedPawn(startCoords, endCoords);
                if (IsValidEnPassant(movingPiece, capturedPiece, startCoords, endCoords))
                {
                    PieceMovement movement = new PieceMovement(movingPiece, startCoords, endCoords);
                    PieceRemoval capture = new PieceRemoval(capturedPiece, _board.GetCoordsOfPiece(capturedPiece));
                    chessMove = new ChessMove(new List<IChessMove> { capture, movement });
                }
            }
            return chessMove;
        }

        public ICollection<IChessMove> GetPossibleMovesForPiece(IPiece piece)
        {
            ICollection<IChessMove> possibleMoves = new List<IChessMove>();
            Coords pieceCoords = _board.GetCoordsOfPiece(piece);
            if (piece.GetType() == typeof(Pawn))
            {
                IChessMove move;
                int changeY = piece.Colour == PieceColour.White ? 1 : -1;
                // Only possible En Passant moves would be diagonal captures, so check only them
                for (int changeX = -1; changeX <= 1; changeX += 2)
                {
                    move = ProcessChessMove(piece.Colour, pieceCoords, new Coords(pieceCoords.X + changeX, pieceCoords.Y + changeY));
                    if (move != null)
                    {
                        possibleMoves.Add(move);
                    }
                }
            }
            return possibleMoves;
        }

        public bool CheckFirstSelectedCoords(PieceColour colourOfMover, Coords coords)
        {
            IPiece pawn = _board.GetSquareAt(coords).Piece;
            return pawn != null && pawn.GetType() == typeof(Pawn) && pawn.Colour == colourOfMover;
        }

        private bool IsValidEnPassant(IPiece movingPiece, IPiece capturedPiece, Coords StartCoords, Coords EndCoords)
        {
            return CheckMovementVector(movingPiece.Colour, StartCoords, EndCoords)
                    && capturedPiece != null
                    && capturedPiece.Colour != movingPiece.Colour
                    && CheckPreviousMoveWasDoubleByCapturedPawn(capturedPiece, _board.GetCoordsOfPiece(capturedPiece));
        }

        private bool CheckMovementVector(PieceColour colour, Coords StartCoords, Coords EndCoords)
        {
            bool verifiedVector = false;
            int ChangeX = EndCoords.X - StartCoords.X;
            int ChangeY = EndCoords.Y - StartCoords.Y;

            int expectedYDirection = colour == PieceColour.White ? 1 : -1;
            if (Math.Abs(ChangeX) == 1 && ChangeY == expectedYDirection)
            {
                verifiedVector = true;
            }
            return verifiedVector;
        }

        private IPiece GetCapturedPawn(Coords StartCoords, Coords EndCoords)
        {
            Coords ExpectedCapturePawnCoords = new Coords(EndCoords.X, StartCoords.Y);
            IPiece capturedPiece = _board.GetSquareAt(ExpectedCapturePawnCoords).Piece;
            if (capturedPiece != null && capturedPiece.GetType() == typeof(Pawn))
            {
                return capturedPiece;
            }
            return null;
        }

        private bool CheckPreviousMoveWasDoubleByCapturedPawn(IPiece capturedPawn, Coords currentCapturedPawnCoords)
        {
            bool wasDoubleByCapturedPawn = false;
            IChessMove previousMove = _board.GetPreviousMove();
            if (previousMove != null)
            {
                _board.UndoLastMove();
                Coords previousCapturedPawnCoords = _board.GetCoordsOfPiece(capturedPawn);
                _board.MakeMove(previousMove);

                if (Math.Abs(currentCapturedPawnCoords.Y - previousCapturedPawnCoords.Y) == 2)
                {
                    wasDoubleByCapturedPawn = true;
                }
            }
            return wasDoubleByCapturedPawn;
        }
    }
}
