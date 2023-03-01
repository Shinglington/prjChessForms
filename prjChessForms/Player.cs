using System;
using System.Timers;
using System.Windows.Forms;

namespace prjChessForms
{
    abstract class Player
    {
        public Label TimeLabel;

        public event EventHandler<ElapsedEventArgs> TimeExpired;
        private System.Timers.Timer _timer;
        private const int _INTERVAL = 1000;
        public Player(PieceColour colour, TimeSpan initialTime)
        {
            Colour = colour;
            RemainingTime = initialTime;
            _timer = new System.Timers.Timer(_INTERVAL);
            _timer.Elapsed += OnTimerTick;
            TimeLabel.Text = RemainingTime.ToString();
        }

        public TimeSpan RemainingTime { get; private set; }
        public PieceColour Colour { get; }

        public void StartTimer()
        {
            _timer.Start();
        }
        public void StopTimer()
        {
            _timer.Stop();
        }
        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            RemainingTime.Subtract(new TimeSpan(0, 0, _INTERVAL));
            TimeLabel.Text = RemainingTime.ToString();
            if (TimeSpan.Compare(RemainingTime, new TimeSpan(0,0,0)) < 1)
            {
                _timer.Stop();
                TimeExpired(this, e);
            }
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
