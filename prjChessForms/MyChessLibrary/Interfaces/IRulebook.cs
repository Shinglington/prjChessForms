using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    public interface IRulebook
    {
        bool CheckLegalMove(Move move);
        void MakeMove(Move move);
        ICollection<Move> GetPossibleMovesForPiece(IPiece piece);
    }
}
