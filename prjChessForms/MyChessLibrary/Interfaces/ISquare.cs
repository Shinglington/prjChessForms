using prjChessForms.MyChessLibrary.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.Interfaces
{
    public interface ISquare
    {
        IPiece Piece { get; set; }
        Coords Coords { get; }
    }

    public interface IGhostPawnGettableSquare : ISquare 
    {
        GhostPawn GetGhostPawn();
    }

}
