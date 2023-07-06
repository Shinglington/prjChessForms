namespace prjChessForms.MyChessLibrary.Pieces
{
    class GhostPawn : Piece
    {
        public GhostPawn(PieceColour colour, Pawn referencedPawn) : base(colour)
        {
            LinkedPawn = referencedPawn;
        }
        public Pawn LinkedPawn { get; }

        public override bool CanMove(IBoard board, ChessMove move) => false;
    }
}
