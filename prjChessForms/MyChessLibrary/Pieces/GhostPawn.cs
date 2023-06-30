namespace prjChessForms.MyChessLibrary.Pieces
    using prjChessForms.MyChessLibrary.Interfaces;
{
    class GhostPawn : Piece
    {
        public GhostPawn(PieceColour colour, Pawn referencedPawn) : base(colour)
        {
            LinkedPawn = referencedPawn;
        }
        public Pawn LinkedPawn { get; }

        public override bool CanMove(IBoard board, Coords startCoords, Coords endCoords)
        {
            return false;
        }
    }
}
