using System;

namespace prjChessForms.MyChessLibrary
{
    public interface ITimeManager
    {
        event EventHandler<PlayerTimerTickEventArgs> PlayerTimerTick;
        event EventHandler<TimeExpiredEventArgs> TimeExpired;
        void StartTimer();
        void StopTimer();
    }
}
