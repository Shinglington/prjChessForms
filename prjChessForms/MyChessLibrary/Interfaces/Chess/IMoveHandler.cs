namespace prjChessForms.MyChessLibrary
{
    public interface IMoveHandler
    {
        void AttemptMakeMove(IBoard board, Coords StartCoords, Coords EndCoords);
    }
}
