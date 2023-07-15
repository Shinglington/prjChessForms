using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class Board : IBoard
    {
        private readonly IBoardCreator _boardCreator;
        private readonly ISquareProvider _squareProvider;
        private readonly IPieceProvider _pieceProvider;
        private readonly IMoveMaker _moveMaker;


        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private ISquare[,] _squares;

        public event EventHandler<PieceChangedEventArgs> PieceInSquareChanged
        {
            add { _boardCreator.PieceInSquareChanged += value; }
            remove { _boardCreator.PieceInSquareChanged -= value; }
        }


        public Board(IBoardCreator boardCreator, ISquareProvider squareProvider, IPieceProvider pieceProvider, IMoveMaker moveMaker)
        {
            _boardCreator = boardCreator;
            _squareProvider = squareProvider;
            _pieceProvider = pieceProvider;
            _moveMaker = moveMaker;

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
    }
}