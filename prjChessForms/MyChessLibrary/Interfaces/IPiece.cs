using System.Drawing;

namespace prjChessForms.MyChessLibrary.Interfaces
{
    public interface IPiece
    {
        bool HasMoved { get; set; }
        Image Image { get; }
        PieceColour Colour { get; }
        string Name { get; }
        string Fullname { get; } 
        bool CanMove(IBoard board, Coords startCoords, Coords endCoords);
    }
}
