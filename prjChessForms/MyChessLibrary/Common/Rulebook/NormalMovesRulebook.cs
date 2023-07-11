using prjChessForms.MyChessLibrary.DataClasses.ChessMoves;
using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class NormalMovesRulebook : IRulebook
    {
        private readonly IBoard _board;
        public NormalMovesRulebook(IBoard board)
        {
            _board = board;
        }

        public IChessMove ProcessChessMove(Coords StartCoords, Coords EndCoords)
        {
            IPiece movedPiece = _board.GetSquareAt(StartCoords).Piece;
            IPiece capturedPiece = _board.GetSquareAt(EndCoords).Piece;
            IChessMove chessMove = null;

            if (movedPiece != null) 
            {
                if (capturedPiece != null)
                {
                    chessMove = ProcessCapture(StartCoords, EndCoords);
                }
                else
                {
                    chessMove = ProcessMove(StartCoords, EndCoords);
                }
            }

            return chessMove;
        }

        public ICollection<IChessMove> IRulebook.GetPossibleMovesForPiece(IPiece piece)
        {
            ICollection<IChessMove> possibleMoves = new List<IChessMove>();
            Coords pieceCoords = _board.GetCoordsOfPiece(piece);
            IChessMove move;
            for (int y = 0; y < _board.RowCount; y++)
            {
                for (int x = 0; x < _board.ColumnCount; x++)
                {
                    move = ProcessChessMove(pieceCoords, new Coords(x, y));
                    if (move != null)
                    {
                        possibleMoves.Add(move);
                    }
                }
            }
            return possibleMoves;
        }

        private IChessMove ProcessCapture(Coords StartCoords, Coords EndCoords)
        {
            ChessMove chessMove = null; 
            Capture capture = null;
            Move move = null;
            PieceColour colour = movedPiece.Colour;
            if (movedPiece.CanMove(_board, move))
            {
                if (capturedPiece == null || capturedPiece.Colour != colour)
                {
                    if (!_board.CheckMoveInCheck(colour, move))
                    {
                        legal = true;
                    }
                }
            }
            return chessMove;
        }

        private IChessMove ProcessMove(Coords StartCoords, Coords EndCoords)
        {
            ChessMove chessMove = null;



            return chessMove;
        }

    }
}
