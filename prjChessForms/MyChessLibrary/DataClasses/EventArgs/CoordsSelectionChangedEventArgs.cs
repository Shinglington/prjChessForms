using System;

namespace prjChessForms.MyChessLibrary
{
    public class CoordsSelectionChangedEventArgs : EventArgs
    {
        public CoordsSelectionChangedEventArgs(Coords coords)
        {
            SelectedCoords = coords;
        }
        public Coords SelectedCoords { get; }
    }
}
