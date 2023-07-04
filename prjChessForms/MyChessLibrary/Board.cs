using System;
using System.Collections.Generic;
using prjChessForms.MyChessLibrary.Pieces;

namespace prjChessForms.MyChessLibrary
{
    class Board
    {
        public event EventHandler<PieceChangedEventArgs> PieceInSquareChanged;

        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private Square[,] _squares;
        public Board()
        {
            SetupBoard();
        }
        public int RowCount { get { return ROW_COUNT; } }
        public int ColumnCount { get { return COL_COUNT; } }

        public void MakeMove(ChessMove Move)
        {
            Coords StartCoords = Move.StartCoords;
            Coords EndCoords = Move.EndCoords;
            Piece p = GetPieceAt(StartCoords);
            if (p != null)
            {
                GetSquareAt(EndCoords).Piece = p;
                GetSquareAt(StartCoords).Piece = null;
                p.HasMoved = true;
            }
        }

        public King GetKing(PieceColour colour)
        {
            King king = null;
            foreach (Piece p in GetPieces(colour))
            {
                if (p.GetType() == typeof(King))
                {
                    king = (King)p;
                    break;
                }
            }
            return king;
        }

        public List<Piece> GetPieces(PieceColour colour)
        {
            List<Piece> pieces = new List<Piece>();
            Piece p;
            for (int y = 0; y < ROW_COUNT; y++)
            {
                for (int x = 0; x < COL_COUNT; x++)
                {
                    p = GetPieceAt(new Coords(x, y));
                    if (p != null && p.Colour == colour)
                    {
                        pieces.Add(p);
                    }
                }
            }
            return pieces;
        }

        public Piece GetPieceAt(Coords coords)
        {
            return (GetSquareAt(coords).Piece);
        }

        public Coords GetCoordsOfPiece(Piece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException();
            }
            foreach (Square s in GetSquares())
            {
                if (s.Piece == piece)
                {
                    return s.Coords;
                }
            }
            throw new Exception("Piece could not be located");
        }

        public Square[,] GetSquares()
        {
            return _squares;
        }

        public Square GetSquareAt(Coords coords)
        {
            return _squares[coords.X, coords.Y];
        }

        public void RemoveGhostPawns()
        {
            foreach (Square s in GetSquares())
            {
                if (s.GetGhostPawn() != null)
                {
                    s.Piece = null;
                }
            }
        }

        public bool CheckMoveInCheck(PieceColour colour, ChessMove move)
        {
            Coords start = move.StartCoords;
            Coords end = move.EndCoords;
            bool startPieceHasMoved = GetPieceAt(start).HasMoved;
            Piece originalEndPiece = GetPieceAt(end);

            MakeMove(move);
            bool SelfCheck = Rulebook.IsInCheck(this, colour);
            MakeMove(new ChessMove(end, start));

            GetSquareAt(start).Piece.HasMoved = startPieceHasMoved;
            GetSquareAt(end).Piece = originalEndPiece;

            return SelfCheck;
        }

        private void SetupBoard()
        {
            _squares = new Square[COL_COUNT, ROW_COUNT];
            Square s;
            for (int y = 0; y < ROW_COUNT; y++)
            {
                for (int x = 0; x < COL_COUNT; x++)
                {
                    s = new Square(x, y);
                    s.PieceChanged += OnPieceInSquareChanged;
                    _squares[x, y] = s;
                }
            }
            PlaceStartingPieces();
        }

        private void PlaceStartingPieces()
        {
            PlaceBlackStartingPieces();
            PlaceWhiteStartingPieces();
        }

        private void PlaceBlackStartingPieces()
        {
            PieceColour col = PieceColour.Black;
            for (int x = 0; x < COL_COUNT; x++)
            {
                PlacePiece(new Pawn(col), new Coords(x, 6));
            }
            PlacePiece(new Rook(col), new Coords(0, 7));
            PlacePiece(new Knight(col), new Coords(1, 7));
            PlacePiece(new Bishop(col), new Coords(2, 7));
            PlacePiece(new Queen(col), new Coords(3, 7));
            PlacePiece(new King(col), new Coords(4, 7));
            PlacePiece(new Bishop(col), new Coords(5, 7));
            PlacePiece(new Knight(col), new Coords(6, 7));
            PlacePiece(new Rook(col), new Coords(7, 7));
        }

        private void PlaceWhiteStartingPieces()
        {
            PieceColour col = PieceColour.White;
            for (int x = 0; x < COL_COUNT; x++)
            {
                PlacePiece(new Pawn(col), new Coords(x, 1));
            }
            PlacePiece(new Rook(col), new Coords(0, 0));
            PlacePiece(new Knight(col), new Coords(1, 0));
            PlacePiece(new Bishop(col), new Coords(2, 0));
            PlacePiece(new Queen(col), new Coords(3, 0));
            PlacePiece(new King(col), new Coords(4, 0));
            PlacePiece(new Bishop(col), new Coords(5, 0));
            PlacePiece(new Knight(col), new Coords(6, 0));
            PlacePiece(new Rook(col), new Coords(7, 0));
        }

        private void PlacePiece(Piece piece, Coords coords)
        {
            Square square = GetSquareAt(coords);
            square.Piece = piece;
        }

        private void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e)
        {
            if (PieceInSquareChanged != null)
            {
                PieceInSquareChanged.Invoke(this, e);
            }
        }
    }
}