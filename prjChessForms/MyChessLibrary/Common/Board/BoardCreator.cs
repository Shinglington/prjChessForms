using prjChessForms.MyChessLibrary.Pieces;
using System;

namespace prjChessForms.MyChessLibrary
{
    class BoardCreator : IBoardCreator
    {
        private IBoard _board;
        private readonly IStartingPositionSetup _startingPositionSetup;
        private readonly IPiecePlacer _piecePlacer;

        public event EventHandler<PieceChangedEventArgs> PieceInSquareChanged;
        public BoardCreator(IStartingPositionSetup startingPositionSetup, IPiecePlacer piecePlacer)
        {
            _startingPositionSetup = startingPositionSetup;
            _piecePlacer = piecePlacer;
        }
        public void SetBoard(IBoard board) => _board = board;
        public void CreateBoard()
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
        private void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e)
        {
            if (PieceInSquareChanged != null)
            {
                PieceInSquareChanged.Invoke(this, e);
            }
        }
    }

    class StartingPositionSetup : IStartingPositionSetup
    {
        private IBoard _board;
        private readonly IPiecePlacer _piecePlacer;
        public StartingPositionSetup(IPiecePlacer piecePlacer)
        {
            _piecePlacer = piecePlacer;
        }
        public void SetBoard(IBoard board)
        {
            _board = board;
            _piecePlacer.SetBoard(_board);
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

    class PiecePlacer : IPiecePlacer
    {
        private IBoard _board;
        public void SetBoard(IBoard board) => _board = board;
        public void PlacePiece(IPiece piece, Coords coords)
        {
            ISquare square = _board.GetSquareAt(coords);
            square.Piece = piece;
        }
    }

}
