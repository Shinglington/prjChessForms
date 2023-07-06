using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class EnPassantRulebook : IRulebook
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
