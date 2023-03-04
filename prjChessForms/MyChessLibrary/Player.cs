﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace prjChessForms.MyChessLibrary
{
    abstract class Player
    {
        private SemaphoreSlim _semaphoreMoveSend = new SemaphoreSlim(0, 1);
        private ChessMove _selectedMove;
        public Player(PieceColour colour, TimeSpan initialTime)
        {
            Colour = colour;
            RemainingTime = initialTime;
        }
        public TimeSpan RemainingTime { get; private set; }
        public PieceColour Colour { get; }

        public void TickTime(TimeSpan time)
        {
            RemainingTime = RemainingTime.Subtract(time);
        }
        public async Task<ChessMove> GetMove(CancellationToken cToken)
        {
            await _semaphoreMoveSend.WaitAsync(cToken);
            return _selectedMove;
        }

        public void SendMove(ChessMove move)
        {
            _selectedMove = move;
            _semaphoreMoveSend.Release();
        }

    }

    class HumanPlayer : Player
    {
        public HumanPlayer(PieceColour colour, TimeSpan initialTime) : base(colour, initialTime) { }
    }

    class ComputerPlayer : Player
    {
        public ComputerPlayer(PieceColour colour, TimeSpan initialTime) : base(colour, initialTime) { }

    }

}
