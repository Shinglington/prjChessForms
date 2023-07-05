namespace prjChessForms.MyChessLibrary
{
    public struct ChessMove
    {
        public ChessMove(Coords startCoords, Coords endCoords)
        {
            StartCoords = startCoords;
            EndCoords = endCoords;
        }
        public Coords StartCoords { get; }
        public Coords EndCoords { get; }
        public override string ToString()
        {
            return StartCoords.ToString() + " -> " + EndCoords.ToString();
        }
    }
}
