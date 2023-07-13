﻿using prjChessForms.MyChessLibrary.Interfaces.Chess;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class Chess : IChess
    {
        private readonly IGameHandler _gameHandler;

        public Chess(IGameHandler gameHandler, IChessObserver userInterfaceObserver)
        {
            _gameHandler = gameHandler;
        }
        public Task<GameOverEventArgs> PlayGame(TimeSpan playerTime) => _gameHandler.PlayGame(playerTime);
    }
}
