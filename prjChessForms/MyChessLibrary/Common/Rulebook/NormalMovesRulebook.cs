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

        public IChessMove ProcessChessMove(Coords StartCoords, Coords EndCoords)
        {
            IPiece movedPiece = _board.GetSquareAt(StartCoords).Piece;
            IPiece capturedPiece = _board.GetSquareAt(EndCoords).Piece;
            IChessMove chessMove = null;

            if (movedPiece != null)
            {
                if (capturedPiece != null)
                {
                    chessMove = ProcessCapture(movedPiece, capturedPiece, StartCoords, EndCoords);
                }
                else
                {
                    chessMove = ProcessMove(movedPiece, StartCoords, EndCoords);
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
                    move = ProcessChessMove(pieceCoords, new Coords(x, y));
                    if (move != null)
                    {
                        possibleMoves.Add(move);
                    }
                }
            }
            return possibleMoves;
        }

        private IChessMove ProcessCapture(IPiece movingPiece, IPiece capturedPiece, Coords StartCoords, Coords EndCoords)
        {
            ChessMove chessMove = null;
            if (movingPiece.Colour != capturedPiece.Colour)
            {
                if (movingPiece.CanMove(_board, StartCoords, EndCoords))
                {
                    Move movement = new Move(movingPiece, StartCoords, EndCoords);
                    Capture capture = new Capture(capturedPiece, EndCoords);
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
                Move movement = new Move(movingPiece, StartCoords, EndCoords);
                chessMove = new ChessMove(new List<IChessMove>() { movement });
            }
            return chessMove;
        }

    }
}
