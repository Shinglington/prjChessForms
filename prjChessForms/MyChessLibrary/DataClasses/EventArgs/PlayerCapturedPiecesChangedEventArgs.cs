using System;

namespace prjChessForms.MyChessLibrary
{
    public class PlayerCapturedPiecesChangedEventArgs : EventArgs
    {
        public PlayerCapturedPiecesChangedEventArgs(IPlayer player, IPiece capturedPiece)
        {
            Player = player;
            CapturedPiece = capturedPiece;
        }
        public IPlayer Player { get; set; }
        public IPiece CapturedPiece { get; set; }
    }
}
