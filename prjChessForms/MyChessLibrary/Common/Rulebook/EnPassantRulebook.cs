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
            throw new NotImplementedException();
        }

        private bool CheckMovementVector(PieceColour colour, Coords StartCoords, Coords EndCoords)
        {
            bool verifiedVector = false;
            int ChangeX = EndCoords.X - StartCoords.X;
            int ChangeY = EndCoords.Y - StartCoords.Y;

            int expectedYDirection = (colour == PieceColour.White ? 1 : -1);
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
