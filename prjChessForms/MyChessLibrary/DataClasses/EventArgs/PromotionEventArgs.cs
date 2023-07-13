using System;

namespace prjChessForms.MyChessLibrary
{
    public class PromotionEventArgs : EventArgs
    {
        public PromotionEventArgs(PieceColour colour, Coords coords)
        {
            PromotingCoords = coords;
            PromotingColour = colour;
        }
        public Coords PromotingCoords { get; set; }
        public PieceColour PromotingColour { get; set; }
    }
}
