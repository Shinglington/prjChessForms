using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace prjChessForms.MyChessLibrary
{
    class MakeMoveEventArgs : EventArgs 
    { 
        public MakeMoveEventArgs(ChessMove move)
        {
            Move = move;
        }

        public ChessMove Move { get; set; }
    }



    abstract class Player
    {
        public event EventHandler<MakeMoveEventArgs> SendMoveRequest;
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
        public async Task<ChessMove> GetMove()
        {
            return new ChessMove();
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
