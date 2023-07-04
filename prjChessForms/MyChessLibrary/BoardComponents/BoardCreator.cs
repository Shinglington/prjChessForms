using prjChessForms.MyChessLibrary.Interfaces;
using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public class BoardCreator : IBoardCreator
    {
        private readonly IBoard _board;
        private readonly IStartingPositionSetup _startingPositionSetup;
        private readonly IPiecePlacer _piecePlacer;
        public BoardCreator(IBoard board, IStartingPositionSetup startingPositionSetup, IPiecePlacer piecePlacer)
        {
            _board = board;
            _startingPositionSetup = startingPositionSetup;
            _piecePlacer = piecePlacer;
        }
        public void SetupBoard()
        {
            _board.SetSquares(new Square[_board.ColumnCount, _board.RowCount]);
            Square s;
            for (int y = 0; y < _board.RowCount; y++)
            {
                for (int x = 0; x < _board.ColumnCount; x++)
                {
                    s = new Square(x, y);
                    s.PieceChanged += OnPieceInSquareChanged;
                    _board.GetSquares()[x, y] = s;
                }
            }
            _startingPositionSetup.PlaceStartingPieces();
        }
    }

    public class StartingPositionSetup : IStartingPositionSetup
    {
        private readonly IBoard _board;
        private readonly IPiecePlacer _piecePlacer;
        public StartingPositionSetup(IBoard board, IPiecePlacer piecePlacer)
        {
            _piecePlacer = piecePlacer;
        }
        public void PlaceStartingPieces()
        {
            PlaceBlackStartingPieces();
            PlaceWhiteStartingPieces();
        }
        private void PlaceBlackStartingPieces()
        {
            PieceColour col = PieceColour.Black;
            for (int x = 0; x < _board.ColumnCount; x++)
            {
                _piecePlacer.PlacePiece(new Pawn(col), new Coords(x, 6));
            }
            _piecePlacer.PlacePiece(new Rook(col), new Coords(0, 7));
            _piecePlacer.PlacePiece(new Knight(col), new Coords(1, 7));
            _piecePlacer.PlacePiece(new Bishop(col), new Coords(2, 7));
            _piecePlacer.PlacePiece(new Queen(col), new Coords(3, 7));
            _piecePlacer.PlacePiece(new King(col), new Coords(4, 7));
            _piecePlacer.PlacePiece(new Bishop(col), new Coords(5, 7));
            _piecePlacer.PlacePiece(new Knight(col), new Coords(6, 7));
            _piecePlacer.PlacePiece(new Rook(col), new Coords(7, 7));
        }

        private void PlaceWhiteStartingPieces()
        {
            PieceColour col = PieceColour.White;
            for (int x = 0; x < _board.ColumnCount; x++)
            {
                _piecePlacer.PlacePiece(new Pawn(col), new Coords(x, 1));
            }
            _piecePlacer.PlacePiece(new Rook(col), new Coords(0, 0));
            _piecePlacer.PlacePiece(new Knight(col), new Coords(1, 0));
            _piecePlacer.PlacePiece(new Bishop(col), new Coords(2, 0));
            _piecePlacer.PlacePiece(new Queen(col), new Coords(3, 0));
            _piecePlacer.PlacePiece(new King(col), new Coords(4, 0));
            _piecePlacer.PlacePiece(new Bishop(col), new Coords(5, 0));
            _piecePlacer.PlacePiece(new Knight(col), new Coords(6, 0));
            _piecePlacer.PlacePiece(new Rook(col), new Coords(7, 0));
        }
    }

    public class PiecePlacer : IPiecePlacer
    {
        private readonly IBoard _board;
        public PiecePlacer(IBoard board)
        {
            _board = board;
        }
        public void PlacePiece(IPiece piece, Coords coords)
        {
            ISquare square = _board.GetSquareAt(coords);
            square.Piece = piece;
        }
    }

}
