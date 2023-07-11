using prjChessForms.MyChessLibrary.DataClasses.ChessMoves;
using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace prjChessForms.MyChessLibrary
{
    class EnPassantRulebook : IRulebook
    {
        private readonly IBoard _board;
        public EnPassantRulebook(IBoard board)
        {
            _board = board;
        }

        public IChessMove ProcessChessMove(Coords StartCoords, Coords EndCoords)
        {
            IChessMove chessMove = null;
            IPiece movingPiece = _board.GetSquareAt(StartCoords).Piece;
            if (movingPiece != null && movingPiece.GetType() == typeof(Pawn))
            {
                IPiece capturedPiece = GetCapturedPawn(StartCoords, EndCoords);
                if (CheckMovementVector(movingPiece.Colour, StartCoords, EndCoords)
                    && capturedPiece != null
                    && capturedPiece.Colour != movingPiece.Colour
                    && CheckPreviousMoveWasDoubleByCapturedPawn(capturedPiece, _board.GetCoordsOfPiece(capturedPiece)))
                {
                    PieceMovement movement = new PieceMovement(movingPiece, StartCoords, EndCoords);
                    PieceRemoval capture = new PieceRemoval(capturedPiece, _board.GetCoordsOfPiece(capturedPiece));
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
                for (int changeX = -1; changeX <= 1; changeX+=2)
                {
                    move = ProcessChessMove(pieceCoords, new Coords(pieceCoords.X + changeX, pieceCoords.Y + changeY));
                    if (move != null)
                    {
                        possibleMoves.Add(move);
                    }
                }
                return possibleMoves;
            }
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
