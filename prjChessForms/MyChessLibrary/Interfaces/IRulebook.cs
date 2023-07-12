using System.Collections.Generic;
namespace prjChessForms.MyChessLibrary
{
    public interface IRulebook
    {
        IChessMove ProcessChessMove(PieceColour colourOfMover, Coords startCoords, Coords endCoords);
        ICollection<IChessMove> GetPossibleMovesForPiece(IPiece piece);
        bool CheckFirstSelectedCoords(PieceColour colourOfMover, Coords coords);
    }
}
