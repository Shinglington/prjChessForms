using prjChessForms.MyChessLibrary;
using System;

namespace prjChessForms.PresentationUI
{
    class SquareClickedEventArgs : EventArgs
    {
        public SquareClickedEventArgs(Coords coords)
        {
            ClickedCoords = coords;
        }
        public Coords ClickedCoords { get; set; }
    }

    class PromotionSelectedEventArgs : EventArgs 
    {
        public PromotionSelectedEventArgs(PromotionOption option)
        {
            SelectedOption = option;
        }

        public PromotionOption SelectedOption { get; set; }
    }

}