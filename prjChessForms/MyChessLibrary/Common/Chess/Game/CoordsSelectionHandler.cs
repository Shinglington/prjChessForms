using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class CoordsSelectionHandler : ICoordSelectionHandler
    {
        public event EventHandler<CoordsSelectionChangedEventArgs> CoordsSelectionChanged;

        public void ChangeCoordsSelection(Coords selectedCoords)
        {
            if (CoordsSelectionChanged != null)
            {
                List<Coords> endCoords = new List<Coords>();
                CoordsSelectionChanged.Invoke(this, new CoordsSelectionChangedEventArgs(selectedCoords));
            }
        }
    }
}
