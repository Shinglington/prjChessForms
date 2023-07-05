namespace prjChessForms.MyChessLibrary
{
    class MoveMaker : IMoveMaker
    {
        private IBoard _board;
        public void SetBoard(IBoard board) => _board = board;
        public void MakeMove(ChessMove move)
        {
            Coords StartCoords = move.StartCoords;
            Coords EndCoords = move.EndCoords;
            IPiece p = _board.GetPieceAt(StartCoords);
            if (p != null)
            {
                _board.GetSquareAt(EndCoords).Piece = p;
                _board.GetSquareAt(StartCoords).Piece = null;
                p.HasMoved = true;
            }
        }
    }
}
