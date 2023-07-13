using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface ICoordSelectionHandler
    {
        event EventHandler<CoordsSelectionChangedEventArgs> CoordsSelectionChanged;
        void ChangeCoordsSelection(Coords coords);
    }
}
