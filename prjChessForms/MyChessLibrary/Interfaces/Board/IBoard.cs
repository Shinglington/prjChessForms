using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    public interface IBoard
    {
        int ColumnCount { get; }
        int RowCount { get; }
        ISquare[,] GetSquares();
        void SetSquares(ISquare[,] squares);
        ISquare GetSquareAt(Coords coords);
        ICollection<IPiece> GetPieces(PieceColour colour);
        Coords GetCoordsOfPiece(IPiece piece);
        void MakeMove(IChessMove move);
        void UndoLastMove();
        IChessMove GetPreviousMove();

    }
}
