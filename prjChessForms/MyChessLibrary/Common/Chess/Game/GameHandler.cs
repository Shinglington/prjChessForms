using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class GameHandler : IGameHandler
    {
        private readonly IBoard _board;
        private readonly IChessObserver _chessObserver;

        private readonly IChessEventManager _chessEventManager;
        private readonly IChessInputController _chessInputController;

        private readonly ITimeManager _timeManager;
        private readonly IPlayerHandler _playerManager;
        private readonly IGameFinishedChecker _gameFinishedChecker;
        private readonly IMoveHandler _moveHandler;
        private readonly ICoordSelectionHandler _coordsSelectionHandler;
        private readonly IPromotionHandler _promotionHandler;


        public GameHandler(IBoard board,
            IChessEventManager chessEventManager, IChessInputController chessInputController,
            IPlayerHandler playerManager, ITimeManager timeManager, IGameFinishedChecker gameFinishedChecker, 
            ICoordSelectionHandler coordSelectionHandler, IPromotionHandler promotionHandler, IMoveHandler moveHandler,
            IChessObserver chessObserver)
        {
            _board = board;

            _chessEventManager = chessEventManager;
            _chessInputController = chessInputController;
            _chessObserver = chessObserver;

            _gameFinishedChecker = gameFinishedChecker;
            _timeManager = timeManager;
            _playerManager = playerManager;
            _coordsSelectionHandler = coordSelectionHandler;

            _moveHandler = moveHandler;
            _promotionHandler = promotionHandler;

            _chessInputController.ConnectHandlers(_moveHandler, _promotionHandler);
            _chessEventManager.ConnectEvents(_chessObserver, _coordsSelectionHandler, _timeManager, _gameFinishedChecker, _board);
        }

        public async Task<GameOverEventArgs> PlayGame(TimeSpan playerTime)
        {
            _playerManager.SetupPlayers(playerTime);
            GameOverEventArgs e = _gameFinishedChecker.GetGameResult();
            _timeManager.StartTimer();
            while (e.Result == GameResult.Unfinished)
            {
                try
                {
                    IChessMove move = await _moveHandler.GetChessMove(_playerManager.GetCurrentPlayer().Colour, _gameFinishedChecker.cToken);
                    _moveHandler.AttemptMakeMove(_board, move);
                    _playerManager.NextPlayerTurn();
                }
                catch when (_gameFinishedChecker.cToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Cancelled Play");
                }
                e = _gameFinishedChecker.GetGameResult();
            }
            _timeManager.StopTimer();
            return e;
        }
    }
}
