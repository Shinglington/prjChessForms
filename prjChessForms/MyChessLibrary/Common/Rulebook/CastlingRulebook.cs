using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class CastlingRulebook : IRulebook
    {
        public bool CheckLegalMove(ChessMove move)
        {
            throw new NotImplementedException();
        }

        public ICollection<ChessMove> GetPossibleMoves(IPiece p)
        {
            throw new NotImplementedException();
        }

        public void MakeMove(ChessMove move)
        {
            throw new NotImplementedException();
        }
    }
}
