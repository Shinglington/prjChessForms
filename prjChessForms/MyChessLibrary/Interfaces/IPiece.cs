using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.Interfaces
{
    public interface IPiece : IPieceMoveable, IPieceProperties
    {

    }

    public interface IPieceMoveable 
    {
        bool CanMove(IBoard board, Coords startCoords, Coords endCoords);
    }

    public interface IPieceProperties
    {
        string Name { get; }
        string Fullname { get; }
        Image Image { get; }
        PieceColour Colour { get; }
        bool HasMoved { get; set;  }
    }



}
