using System;
using System.Collections.Generic;
using prjChessForms.MyChessLibrary.Pieces;
using prjChessForms.MyChessLibrary.Interfaces;
using prjChessForms.MyChessLibrary.BoardComponents;

namespace prjChessForms.MyChessLibrary
{
    class Board : IBoard
    {
        private readonly IBoardCreator _boardCreator;
        private readonly ISquareProvider _squareProvider;

        public event EventHandler<PieceChangedEventArgs> PieceInSquareChanged;
        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private ISquare[,] _squares;
        public Board(IBoardCreator boardCreator)
        {
            _boardCreator = boardCreator;


            _boardCreator.SetupBoard();
        }
        public int RowCount { get { return ROW_COUNT; } }
        public int ColumnCount { get { return COL_COUNT; } }

        public void MakeMove(ChessMove Move)
        {
            Coords StartCoords = Move.StartCoords;
            Coords EndCoords = Move.EndCoords;
            IPiece p = GetPieceAt(StartCoords);
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

        public List<IPiece> GetPieces(PieceColour colour)
        {
            List<IPiece> pieces = new List<IPiece>();
            IPiece p;
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

        public IPiece GetPieceAt(Coords coords)
        {
            return GetSquareAt(coords).Piece;
        }

        public Coords GetCoordsOfPiece(IPiece piece)
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

        public ISquare[,] GetSquares()
        {
            return _squares;
        }

        public void SetSquares(ISquare[,] squares)
        {
            _squares = squares;
        }

        public ISquare GetSquareAt(Coords coords)
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
            IPiece originalEndPiece = GetPieceAt(end);

            MakeMove(move);
            bool SelfCheck = Rulebook.IsInCheck(this, colour);
            MakeMove(new ChessMove(end, start));

            GetSquareAt(start).Piece.HasMoved = startPieceHasMoved;
            GetSquareAt(end).Piece = originalEndPiece;

            return SelfCheck;
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