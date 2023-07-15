using System.Drawing;
namespace prjChessForms.MyChessLibrary
{
    public interface IPiece
    {
        int TimesMoved { get; set; }
        bool HasMoved { get; }
        Image Image { get; }
        PieceColour Colour { get; }
        string Name { get; }
        string Fullname { get; }
        bool CanMove(IBoard board, Coords startCoords, Coords endCoords);
    }
}
