using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class GameHandler : IGameHandler
    {
        private IChess _chess;
        private readonly ITimeManager _timeManager;
        private readonly IPlayerManager _playerManager;
        private readonly IGameFinishedChecker _gameFinishedChecker;
        public GameHandler(IPlayerManager playerManager, ITimeManager timeManager, IGameFinishedChecker gameFinishedChecker)
        {
            _timeManager = timeManager;
            _playerManager = playerManager;
        }
        
        public void SetupNewGame(IChess chess)
        {
            _chess = chess;
            _playerManager.SetupPlayers();
            _timeManager.SetupWithPlayers(_playerManager);
        }

        public GameResult PlayGame()
        {
            GameResult result = GameResult.Unfinished;
            _timeManager.StartTimer();
            while (result == GameResult.Unfinished)
            {

                try
                {
                    PieceMovement move = await _inputHandler.GetChessMove(cToken);
                    CapturePiece(FullRulebook.MakeMove(_board, CurrentPlayer.Colour, move));
                    ChangeSelection(null);
                    if (FullRulebook.RequiresPromotion(_board, move.EndCoords))
                    {
                        await Promotion(move.EndCoords, cToken);
                    }
                    _turnCount++;
                    result = FullRulebook.GetGameResult(_board, CurrentPlayer);
                }
                catch when (cToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Cancelled Play");
                    _waitingForClick = false;
                    result = GameResult.Time;
                }

                result = _gameFinishedChecker.GetGameResult();

            }
            _timeManager.StopTimer();
            cts.Cancel();
            return result;
        }
    }
}
