using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.Common.Chess
{
    class CoordsSelectionHandler : ICoordSelectionHandler
    {
        private IChess _chess;
        public event EventHandler<CoordsSelectionChangedEventArgs> CoordsSelectionChanged;
        public CoordsSelectionHandler() 
        { 

        }
        public void SetChessConnection(IChess chess)
        {
            _chess = chess;
        }

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
