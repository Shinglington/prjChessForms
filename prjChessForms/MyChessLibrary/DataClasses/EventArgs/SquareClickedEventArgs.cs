using System;

namespace prjChessForms.MyChessLibrary
{
    public class SquareClickedEventArgs : EventArgs
    {
        public SquareClickedEventArgs(Coords coords)
        {
            ClickedCoords = coords;
        }
        public Coords ClickedCoords { get; set; }
    }
}
