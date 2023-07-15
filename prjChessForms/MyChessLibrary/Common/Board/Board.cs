using prjChessForms.MyChessLibrary.Pieces;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class Board : IBoard
    {
        private readonly IBoardCreator _boardCreator;
        private readonly ISquareProvider _squareProvider;
        private readonly IPieceProvider _pieceProvider;
        private readonly IMoveMaker _moveMaker;
        private readonly IBoardObserver _boardObserver;


        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private ISquare[,] _squares;
        private IEnumerable<PieceMovement> _moveStack;
        public Board(IBoardCreator boardCreator, ISquareProvider squareProvider, IPieceProvider pieceProvider, IMoveMaker moveMaker, IBoardObserver boardObserver)
        {
            _boardCreator = boardCreator;
            _squareProvider = squareProvider;
            _pieceProvider = pieceProvider;
            _moveMaker = moveMaker;
            _boardObserver = boardObserver;

            _boardCreator.SetBoard(this);
            _squareProvider.SetBoard(this);
            _pieceProvider.SetBoard(this);
            _moveMaker.SetBoard(this);

            _boardCreator.CreateBoard();
        }
        public int RowCount { get { return ROW_COUNT; } }
        public int ColumnCount { get { return COL_COUNT; } }

        public ISquare[,] GetSquares() => _squares;
        public void SetSquares(ISquare[,] squares) => _squares = squares;
        public ISquare GetSquareAt(Coords coords) => _squareProvider.GetSquareAt(coords);
        public ICollection<IPiece> GetPieces(PieceColour colour) => _pieceProvider.GetPieces(colour);
        public Coords GetCoordsOfPiece(IPiece piece) => _pieceProvider.GetCoordsOfPiece(piece);
        public void MakeMove(IChessMove move) => _moveMaker.MakeMove(move);
        public IChessMove GetPreviousMove() => _moveMaker.GetLastMove();
        public void UndoLastMove() => _moveMaker.UndoLastMove();

    //    public King GetKing(PieceColour colour)
    //    {
    //        King king = null;
    //        foreach (Piece p in GetPieces(colour))
    //        {
    //            if (p.GetType() == typeof(King))
    //            {
    //                king = (King)p;
    //                break;
    //            }
    //        }
    //        return king;
    //    }

    //    public bool CheckMoveInCheck(PieceColour colour, PieceMovement move)
    //    {
    //        Coords start = move.StartCoords;
    //        Coords end = move.EndCoords;
    //        bool startPieceHasMoved = GetSquareAt(start).Piece.HasMoved;
    //        IPiece originalEndPiece = GetSquareAt(end).Piece;

    //        MakeMove(move);
    //        bool SelfCheck = FullRulebook.IsInCheck(this, colour);
    //        MakeMove(new ChessMove(end, start));

    //        GetSquareAt(start).Piece.HasMoved = startPieceHasMoved;
    //        GetSquareAt(end).Piece = originalEndPiece;

    //        return SelfCheck;
    //    }
    }
}