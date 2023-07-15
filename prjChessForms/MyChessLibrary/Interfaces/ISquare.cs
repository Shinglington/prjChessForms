using System;

namespace prjChessForms.MyChessLibrary
{
    public interface ISquare
    {
        event EventHandler<PieceChangedEventArgs> PieceChanged;
        IPiece Piece { get; set; }
        Coords Coords { get; }
    }
}
