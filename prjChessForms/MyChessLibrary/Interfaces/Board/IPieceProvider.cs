using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface IPieceProvider
    {
        void SetBoard(IBoard board);
        ICollection<IPiece> GetPieces(PieceColour colour);
        Coords GetCoordsOfPiece(IPiece piece);
    }
}
