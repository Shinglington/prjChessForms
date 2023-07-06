using System;

namespace prjChessForms.MyChessLibrary
{
    public interface IBoardCreator
    {
        event EventHandler<PieceChangedEventArgs> PieceInSquareChanged;
        void SetBoard(IBoard board);
        void CreateBoard();
    }

    public interface IStartingPositionSetup
    {
        void SetBoard(IBoard board);
        void PlaceStartingPieces();
    }

    public interface IPiecePlacer
    {
        void SetBoard(IBoard board);
        void PlacePiece(IPiece piece, Coords coords);
    }

}
