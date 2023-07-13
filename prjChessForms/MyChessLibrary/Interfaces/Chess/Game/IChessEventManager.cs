namespace prjChessForms.MyChessLibrary
{
    public interface IChessEventManager
    {
        void ConnectEvents(IChessObserver chessChangesObserver,
            ICoordSelectionHandler coordSelectionHandler, ITimeManager timeManager, IGameFinishedChecker gameFinishedChecker)
    }
}
