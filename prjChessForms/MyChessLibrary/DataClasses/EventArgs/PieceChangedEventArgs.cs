using System;

namespace prjChessForms.MyChessLibrary
{
    public class PieceChangedEventArgs : EventArgs
    {
        public PieceChangedEventArgs(ISquare square, IPiece newPiece)
        {
            NewPiece = newPiece;
            Square = square;
        }
        public ISquare Square { get; set; }
        public IPiece NewPiece { get; set; }
    }
}
