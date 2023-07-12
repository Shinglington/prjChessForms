using System.Collections.Generic;
namespace prjChessForms.MyChessLibrary
{
    public interface IRulebook
    {
        IChessMove ProcessChessMove(Coords StartCoords, Coords EndCoords);
        ICollection<IChessMove> GetPossibleMovesForPiece(IPiece piece);
        bool CheckFirstSelectedCoords(Coords coords);
    }
}
