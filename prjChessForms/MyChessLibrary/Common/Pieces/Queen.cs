namespace prjChessForms.MyChessLibrary.Pieces
{
    class Queen : Piece
    {
        private readonly Rook _rookMovement;
        private readonly Bishop _bishopMovement;
        public Queen(PieceColour colour) : base(colour)
        {
            _rookMovement = new Rook(colour);
            _bishopMovement = new Bishop(colour);
        }

        public override bool CanMove(IBoard board, Coords startCoords, Coords endCoords)
        {
            return _rookMovement.CanMove(board, startCoords, endCoords) || _bishopMovement.CanMove(board, startCoords, endCoords);
        }
    }
}
