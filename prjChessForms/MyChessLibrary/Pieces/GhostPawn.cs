namespace prjChessForms.MyChessLibrary.Pieces
{
    class GhostPawn : Piece
    {
        public GhostPawn(Player player, Pawn referencedPawn) : base(player)
        {
            LinkedPawn = referencedPawn;
        }
        public Pawn LinkedPawn { get; }

        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            return false;
        }
    }
}
