using System;

namespace prjChessForms.MyChessLibrary
{
    public class PlayerTimerTickEventArgs : EventArgs
    {
        public PlayerTimerTickEventArgs(IPlayer player)
        {
            CurrentPlayer = player;
            PlayerRemainingTime = player.RemainingTime;
        }
        public IPlayer CurrentPlayer { get; set; }
        public TimeSpan PlayerRemainingTime { get; set; }
    }
}
