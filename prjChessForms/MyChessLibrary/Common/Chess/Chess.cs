namespace prjChessForms.MyChessLibrary
{
    class Chess : IChess
    {
        private readonly IBoard _board;
        private readonly IPlayerHandler _playerHandler;
        public Chess(IBoard board, IPlayerHandler playerHandler)
        {
            _board = board;
            _playerHandler = playerHandler;
            _playerHandler.SetupPlayers();
        }
        public IPlayer GetPlayer(PieceColour colour) => _playerHandler.GetPlayer(colour);
    }
}
