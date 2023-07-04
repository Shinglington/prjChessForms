using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.Interfaces
{
    public interface ISquareProvider
    {
        ISquare GetSquareAt(Coords coords);
        IPiece GetPieceAt(Coords coords);
    }

}
