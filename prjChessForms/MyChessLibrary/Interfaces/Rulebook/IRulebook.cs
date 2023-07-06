using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    public interface IRulebook
    {
        bool CheckLegalMove(ChessMove move);
        void MakeMove(ChessMove move);
        ICollection<ChessMove> GetPossibleMoves(IPiece p);
    }
}
