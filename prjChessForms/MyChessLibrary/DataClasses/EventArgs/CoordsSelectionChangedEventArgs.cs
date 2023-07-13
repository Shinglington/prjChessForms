using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    public class CoordsSelectionChangedEventArgs : EventArgs
    {
        public CoordsSelectionChangedEventArgs(IPiece piece, Coords selectedPieceCoords, ICollection<Coords> validMoves)
        {
            SelectedPiece = piece;
            SelectedPieceCoords = selectedPieceCoords;
            PossibleEndCoords = validMoves;
        }
        public IPiece SelectedPiece { get; set; }
        public Coords SelectedPieceCoords { get; set; }
        public ICollection<Coords> PossibleEndCoords { get; set; }
    }
}
