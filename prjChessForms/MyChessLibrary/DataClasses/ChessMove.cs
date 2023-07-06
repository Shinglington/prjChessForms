namespace prjChessForms.MyChessLibrary
{
    public readonly struct ChessMove
    {
        public ChessMove(IPiece piece, Coords startCoords, Coords endCoords)
        {
            Piece = piece;
            StartCoords = startCoords;
            EndCoords = endCoords;
        }
        public IPiece Piece { get; }
        public Coords StartCoords { get; }
        public Coords EndCoords { get; }
        public override string ToString()
        {
            return StartCoords.ToString() + " -> " + EndCoords.ToString();
        }
    }
}
