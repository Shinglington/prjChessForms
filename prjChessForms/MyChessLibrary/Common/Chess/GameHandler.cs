using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class GameHandler : IGameHandler
    {
        private IChess _chess;
        private readonly IBoard _board;
        private readonly ITimeManager _timeManager;
        private readonly IPlayerHandler _playerManager;
        private readonly IGameFinishedChecker _gameFinishedChecker;
        private readonly IMoveHandler _moveHandler;

        private CancellationToken _cToken;

        public GameHandler(IBoard board, IPlayerHandler playerManager, ITimeManager timeManager, IGameFinishedChecker gameFinishedChecker)
        {
            _timeManager = timeManager;
            _playerManager = playerManager;
        }

        public void SetupNewGame(IChess chess)
        {
            _chess = chess;
            _playerManager.SetupPlayers(new TimeSpan(0,3,0));
            _timeManager.SetupWithPlayers(_playerManager);
        }

        public async Task<GameOverEventArgs> PlayGame()
        {
            _cToken = new CancellationToken();
            GameOverEventArgs e = _gameFinishedChecker.GetGameResult();
            _timeManager.StartTimer();
            while (e.Result == GameResult.Unfinished)
            {
                try
                {
                    IChessMove move = await _moveHandler.GetChessMove(_cToken);
                    _moveHandler.AttemptMakeMove(_board, move);
                    _playerManager.NextPlayerTurn();
                }
                catch when (_cToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Cancelled Play");
                }
                e = _gameFinishedChecker.GetGameResult();
            }
            _timeManager.StopTimer();
            cts.Cancel();
            return e;
        }
    }
}
