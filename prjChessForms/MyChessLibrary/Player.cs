using System;
using System.Collections.Generic;
using prjChessForms.MyChessLibrary.Interfaces;
namespace prjChessForms.MyChessLibrary
{
    abstract class Player : IPlayer
    {
        public Player(PieceColour colour, TimeSpan initialTime)
        {
            Colour = colour;
            RemainingTime = initialTime;
            CapturedPieces = new List<IPiece>();
        }
        public TimeSpan RemainingTime { get; private set; }
        public PieceColour Colour { get; }
        public List<IPiece> CapturedPieces { get; private set; }

        public void TickTime(TimeSpan time)
        {
            RemainingTime = RemainingTime.Subtract(time);
        }

        public void AddCapturedPiece(IPiece piece)
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
