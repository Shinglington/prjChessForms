using System;
using System.Timers;
using System.Windows.Forms;

namespace prjChessForms
{
    abstract class Player
    {
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
