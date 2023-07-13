using System;
using System.Runtime.InteropServices.ComTypes;
using System.Timers;
namespace prjChessForms.MyChessLibrary
{
    class TimeManager : ITimeManager
    {
        public event EventHandler<PlayerTimerTickEventArgs> PlayerTimerTick;
        public event EventHandler<TimeExpiredEventArgs> TimeExpired;
        private readonly IPlayerHandler _playerHandler;

        private Timer _timer;
        private TimeSpan _interval;
        public TimeManager(IPlayerHandler playerHandler, int tickInterval)
        {
            _playerHandler = playerHandler;
            _interval = new TimeSpan(tickInterval);
            _timer = new Timer(tickInterval);
            _timer.Elapsed += TickCurrentPlayer;
        }

        public void StartTimer() => _timer.Start();
        public void StopTimer() => _timer.Stop();

        private void TickCurrentPlayer(object sender, ElapsedEventArgs e)
        {
            IPlayer currentPlayer = _playerHandler.GetCurrentPlayer();
            currentPlayer.TickTime(_interval);
            if (PlayerTimerTick != null)
            {
                PlayerTimerTick.Invoke(this, new PlayerTimerTickEventArgs(currentPlayer));
            }

            if (currentPlayer.RemainingTime < TimeSpan.Zero)
            {
                StopTimer();
                if (TimeExpired != null)
                {
                    TimeExpired.Invoke(this, new TimeExpiredEventArgs(currentPlayer));
                }
            }

        }
    }
}
