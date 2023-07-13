﻿using System;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface IChess
    {
        Task<GameOverEventArgs> PlayGame(TimeSpan playerTime);
    }
}
