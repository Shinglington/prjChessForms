using prjChessForms.MyChessLibrary.DataClasses.ChessMoves;
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

        public IChessMove ProcessChessMove(PieceColour colourOfMover, Coords startCoords, Coords endCoords)
        {
            IChessMove chessMove = null;
            // Below is true if there is a piece in that square
            if (CheckFirstSelectedCoords(colourOfMover, startCoords))
            {
                IPiece movedPiece = _board.GetSquareAt(startCoords).Piece;
                IPiece capturedPiece = _board.GetSquareAt(endCoords).Piece;
                if (capturedPiece != null)
                {
                    chessMove = ProcessCapture(movedPiece, capturedPiece, startCoords, endCoords);
                }
                else
                {
                    chessMove = ProcessMove(movedPiece, startCoords, endCoords);
                }

            }
            return chessMove;
        }

        public ICollection<IChessMove> GetPossibleMovesForPiece(IPiece piece)
        {
            ICollection<IChessMove> possibleMoves = new List<IChessMove>();
            Coords pieceCoords = _board.GetCoordsOfPiece(piece);
            IChessMove move;
            for (int y = 0; y < _board.RowCount; y++)
            {
                for (int x = 0; x < _board.ColumnCount; x++)
                {
                    move = ProcessChessMove(piece.Colour, pieceCoords, new Coords(x, y));
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
            IPiece piece = _board.GetSquareAt(coords).Piece;
            return piece != null && piece.Colour == colourOfMover;
        }

        private IChessMove ProcessCapture(IPiece movingPiece, IPiece capturedPiece, Coords startCoords, Coords endCoords)
        {
            ChessMove chessMove = null;
            if (movingPiece.Colour != capturedPiece.Colour)
            {
                if (movingPiece.CanMove(_board, startCoords, endCoords))
                {
                    PieceMovement movement = new PieceMovement(movingPiece, startCoords, endCoords);
                    PieceRemoval capture = new PieceRemoval(capturedPiece, endCoords);
                    chessMove = new ChessMove(new List<IChessMove>() { capture, movement });
                }
            }
            return chessMove;
        }

        private IChessMove ProcessMove(IPiece movingPiece, Coords StartCoords, Coords EndCoords)
        {
            ChessMove chessMove = null;

            if (movingPiece.CanMove(_board, StartCoords, EndCoords))
            {
                PieceMovement movement = new PieceMovement(movingPiece, StartCoords, EndCoords);
                chessMove = new ChessMove(new List<IChessMove>() { movement });
            }
            return chessMove;
        }
    }
}
