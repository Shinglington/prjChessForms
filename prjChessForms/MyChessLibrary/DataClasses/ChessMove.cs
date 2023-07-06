namespace prjChessForms.MyChessLibrary
{
    public readonly struct ChessMove
    {
        public ChessMove(IPiece piece, Coords startCoords, Coords endCoords)
        {
            MovedPiece = piece;
            CapturedPiece = null;
            StartCoords = startCoords;
            EndCoords = endCoords;
        }
        public IPiece MovedPiece { get; }
        public IPiece CapturedPiece { get; }
        public Coords StartCoords { get; }
        public Coords EndCoords { get; }
        public bool IsCastle { get; }
        public 
        public override string ToString()
        {
            return StartCoords.ToString() + " -> " + EndCoords.ToString();
        }
    }
}
