using System;

namespace prjChessForms.MyChessLibrary
{
    class Square
    {
        public EventHandler PieceChanged;
        private Piece _piece;
        public Square(int x, int y)
        {
            Coords = new Coords(x, y);
            Piece = null;
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
                PieceChanged.Invoke(this, EventArgs.Empty);
            }
        }
        public Coords Coords { get; }

        public GhostPawn GetGhostPawn()
        {
            return (_piece != null && _piece.GetType() == typeof(GhostPawn)) ? (GhostPawn)_piece : null;
        }
    }
}
