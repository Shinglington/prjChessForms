namespace prjChessForms.MyChessLibrary
{
    class ChessEventManager : IChessEventManager
    {
        public void ConnectEvents(IChessObserver chessChangesObserver,
            ICoordSelectionHandler coordSelectionHandler, ITimeManager timeManager, IGameFinishedChecker gameFinishedChecker)
        {
            coordSelectionHandler.CoordsSelectionChanged += chessChangesObserver.OnCoordSelectionChanged;
            timeManager.PlayerTimerTick += chessChangesObserver.OnPlayerTimerTick;
            gameFinishedChecker.GameOver += chessChangesObserver.OnGameOver;
        }
    }
}
