using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    abstract class Player
    {
        public event EventHandler TimeExpiredEvent;
        public Player(PieceColour colour, TimeSpan initialTime)
        {
            Colour = colour;
            RemainingTime = initialTime;
            CapturedPieces = new List<Piece>();
        }
        public TimeSpan RemainingTime { get; private set; }
        public PieceColour Colour { get; }
        public List<Piece> CapturedPieces { get; private set; }

        public void TickTime(TimeSpan time)
        {
            RemainingTime = RemainingTime.Subtract(time);
            if (RemainingTime < TimeSpan.Zero)
            {
                TimeExpiredEvent.Invoke(this, new EventArgs());
            }
        }

        public void AddCapturedPiece(Piece piece)
        {
            CapturedPieces.Add(piece);
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
