using System.Drawing;

namespace prjChessForms.MyChessLibrary
{
    interface IPiece
    {
        bool HasMoved { get; set; }
        Image Image { get; }
        PieceColour Colour { get; }
        string Name { get; }
        string Fullname { get; } 
        bool CanMove(Board board, Coords startCoords, Coords endCoords);
    }
}
