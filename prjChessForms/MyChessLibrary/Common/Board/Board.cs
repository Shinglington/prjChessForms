using System;
using System.Collections.Generic;
using prjChessForms.MyChessLibrary.Pieces;

namespace prjChessForms.MyChessLibrary
{
    class Board : IBoard
    {
        private readonly IBoardCreator _boardCreator;
        private readonly ISquareProvider _squareProvider;
        private readonly IPieceProvider _pieceProvider;
        private readonly IMoveMaker _moveMaker;

        public event EventHandler<PieceChangedEventArgs> PieceInSquareChanged;
        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private ISquare[,] _squares;
        public Board(IBoardCreator boardCreator, ISquareProvider squareProvider, IPieceProvider pieceProvider, IMoveMaker moveMaker)
        {
            _boardCreator = boardCreator;
            _squareProvider = squareProvider;
            _pieceProvider = pieceProvider;
            _moveMaker = moveMaker;

            _boardCreator.SetupBoard();
        }
        public int RowCount { get { return ROW_COUNT; } }
        public int ColumnCount { get { return COL_COUNT; } }

        public ISquare[,] GetSquares() => _squares;

        public void SetSquares(ISquare[,] squares) => _squares = squares;

        public ISquare GetSquareAt(Coords coords) => _squareProvider.GetSquareAt(coords);

        public IPiece GetPieceAt(Coords coords) => _pieceProvider.GetPieceAt(coords);
        public ICollection<IPiece> GetPieces(PieceColour colour) => _pieceProvider.GetPieces(colour);

        public void MakeMove(ChessMove move) => _moveMaker.MakeMove(move);

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