using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    public interface IPlayer
    {
        PieceColour Colour { get; }
        TimeSpan RemainingTime { get; }
        ICollection<IPiece> CapturedPieces { get; }
        void TickTime(TimeSpan time);
        void AddCapturedPiece(IPiece piece);
    }
}
