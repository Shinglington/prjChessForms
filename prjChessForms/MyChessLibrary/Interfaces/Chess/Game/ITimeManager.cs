using System;

namespace prjChessForms.MyChessLibrary
{
    public interface ITimeManager
    {
        event EventHandler<PlayerTimerTickEventArgs> PlayerTimerTick;
        void SetupWithPlayers(IPlayerHandler playerManager);
        void StartTimer();
        void StopTimer();
    }
}
