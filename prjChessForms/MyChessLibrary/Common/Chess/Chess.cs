using prjChessForms.MyChessLibrary.Interfaces.Chess;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class Chess : IChess
    {
        private readonly IBoard _board;
        private readonly IGameHandler _gameHandler;
        private readonly IPlayerHandler _playerHandler;
        private readonly IMoveHandler _moveHandler;
        private readonly IChessChangesObserver _userInterfaceObserver;
        private readonly IChessInputController _chessInputController;
        private readonly ITimeManager _timeManager;
        private readonly IGameFinishedChecker _gameFinishedChecker;
        private readonly IPromotionHandler _promotionHandler;
        private readonly ICoordSelectionHandler _coordsSelectionHandler;

        public Chess(IChessChangesObserver userInterfaceObserver)
        {
            _userInterfaceObserver = userInterfaceObserver;
        }

        public Chess(IBoard board, IGameHandler gameHandler, IPlayerHandler playerHandler, IMoveHandler moveInputHandler,
            IChessChangesObserver userInterfaceObserver, IChessInputController chessInputController,
            ITimeManager timeManager, IGameFinishedChecker gameFinishedChecker, IPromotionHandler promotionHandler,
            ICoordSelectionHandler coordSelectionHandler)
        {
            _board = board;
            _gameHandler = gameHandler;
            _playerHandler = playerHandler;
            _playerHandler.SetupPlayers(new System.TimeSpan(0, 3, 0));
            _moveHandler = moveInputHandler;
            _userInterfaceObserver = userInterfaceObserver;
            _chessInputController = chessInputController;
            _timeManager = timeManager;
            _promotionHandler = promotionHandler;
        }
        public IPlayer GetPlayer(PieceColour colour) => _playerHandler.GetPlayer(colour);
        public IPlayer GetCurrentPlayer() => _playerHandler.GetCurrentPlayer();
        public Task<GameOverEventArgs> PlayGame() => _gameHandler.PlayGame();
        public void SendCoords(Coords coords) => _moveHandler.ReceiveMoveInput(coords);
        public void SendPromotion(PromotionOption option) => _promotionHandler.ReceivePromotion(option);
    }
}
