using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface IPieceProvider
    {
        IPiece GetPieceAt(Coords coords);
        ICollection<IPiece> GetPieces(PieceColour colour);
    }
}
