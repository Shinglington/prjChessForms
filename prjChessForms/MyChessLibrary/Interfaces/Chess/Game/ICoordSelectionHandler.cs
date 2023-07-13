using System;

namespace prjChessForms.MyChessLibrary
{
    public interface ICoordSelectionHandler
    {
        event EventHandler<CoordsSelectionChangedEventArgs> CoordsSelectionChanged;
        void ChangeCoordsSelection(Coords coords);
    }
}
