namespace prjChessForms.MyChessLibrary
{
    public interface IMoveInputHandler
    {
        void AttemptMakeMove(IBoard board, Coords StartCoords, Coords EndCoords);
    }
}
