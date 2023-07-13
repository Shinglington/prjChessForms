using System;

namespace prjChessForms.MyChessLibrary
{
    class TimeManager : ITimeManager
    {
        public event EventHandler<PlayerTimerTickEventArgs> PlayerTimerTick;

        public void SetupWithPlayers(IPlayerHandler playerManager)
        {
            throw new NotImplementedException();
        }

        public void StartTimer()
        {
            throw new NotImplementedException();
        }

        public void StopTimer()
        {
            throw new NotImplementedException();
        }
    }
}
