using System;

namespace prjChessForms.MyChessLibrary
{
    class PieceChangedEventArgs : EventArgs
    {
        public PieceChangedEventArgs(Piece newPiece)
        {
            NewPiece = newPiece;
        }
        public Piece NewPiece { get; set; }
    }

    class Square
    {
        public event EventHandler<PieceChangedEventArgs> PieceChanged;
        private Piece _piece;
        public Square(int x, int y)
        {
            Coords = new Coords(x, y);
            _piece = null;
        }
        public Piece Piece
        {
            get
            {
                if (_piece != null && _piece.GetType() == typeof(GhostPawn))
                {
                    return null;
                }
                return _piece;
            }
            set
            {
                _piece = value;
                if (PieceChanged != null)
                {
                    PieceChanged(this, new PieceChangedEventArgs(Piece));
                }
            }
        }
        public Coords Coords { get; }

        public GhostPawn GetGhostPawn()
        {
            return (_piece != null && _piece.GetType() == typeof(GhostPawn)) ? (GhostPawn)_piece : null;
        }
    }
}
