namespace prjChessForms.MyChessLibrary
{
    public interface IBoardCreator
    {
        void SetupBoard();
    }

    public interface IStartingPositionSetup
    {
        void PlaceStartingPieces();
    }

    public interface IPiecePlacer
    {
        void PlacePiece(IPiece piece, Coords coords);
    }

}
