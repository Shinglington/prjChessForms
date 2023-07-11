﻿using prjChessForms.MyChessLibrary.Interfaces.Chess;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class Chess : IChess
    {
        private readonly IBoard _board;
        private readonly IGameHandler _gameHandler;
        private readonly IPlayerManager _playerHandler;
        private readonly IMoveInputHandler _moveInputHandler;
        private readonly IChessChangesObserver _userInterfaceObserver;
        private readonly IChessInputController _chessInputController;
        private readonly IInputHandler _inputHandler;
        private readonly ITimeManager _timeManager;
        private readonly IGameFinishedChecker _gameFinishedChecker;

        public Chess(IChessChangesObserver userInterfaceObserver)
        {
            _userInterfaceObserver = userInterfaceObserver;
        }

        public Chess(IBoard board, IGameHandler gameHandler, IPlayerManager playerHandler, IMoveInputHandler moveInputHandler,
            IChessChangesObserver userInterfaceObserver, IChessInputController chessInputController,
            IInputHandler inputHandler, ITimeManager timeManager, IGameFinishedChecker gameFinishedChecker)
        {
            _board = board;
            _gameHandler = _gameHandler;
            _playerHandler = playerHandler;
            _playerHandler.SetupPlayers();
            _moveInputHandler = moveInputHandler;
            _userInterfaceObserver = userInterfaceObserver;
            _chessInputController = chessInputController;
            _inputHandler = inputHandler;
            _timeManager = timeManager;
        }
        public IPlayer GetPlayer(PieceColour colour) => _playerHandler.GetPlayer(colour);

        public async Task PlayGame() => _gameHandler.PlayGame();

        public void SendCoords(Coords coords)
        {
            if (_waitingForClick)
            {
                Debug.WriteLine("Model received coords input of {0}", coords);
                _clickedCoords = coords;
                _semaphoreReceiveClick.Release();
            }
        }

        public void SendPromotion(PromotionOption option)
        {
            if (_waitingForPromotion)
            {
                Debug.WriteLine("Promotion received to {0}", option.ToString());
                _selectedPromotion = option;
                _semaphoreReceiveClick.Release();
            }
        }
    }
}
