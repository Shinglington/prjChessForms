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
        public CoordsSelectionHandler() 
        { 

        }
        public void SetChessConnection(IChess chess)
        {
            _chess= chess;
        }

        public void ChangeCoordsSelection(Coords selectedCoords)
        {
            if (PieceSelectionChanged != null)
            {
                List<Coords> endCoords = new List<Coords>();
                Coords selectedCoords = new Coords();
                if (selectedPiece != null)
                {
                    selectedCoords = GetCoordsOf(selectedPiece);
                    foreach (PieceMovement m in FullRulebook.GetPossibleMoves(_board, selectedPiece))
                    {
                        endCoords.Add(m.EndCoords);
                    }
                }
                PieceSelectionChanged.Invoke(this, new PieceSelectionChangedEventArgs(selectedPiece, selectedCoords, endCoords));
            }
        }
    }
}
