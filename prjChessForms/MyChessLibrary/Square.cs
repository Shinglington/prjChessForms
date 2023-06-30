using System;
using prjChessForms.MyChessLibrary.Pieces;
using prjChessForms.MyChessLibrary.Interfaces;
namespace prjChessForms.MyChessLibrary
{
    class Square : ISquare
    {
        public EventHandler<PieceChangedEventArgs> PieceChanged;
        private IPiece _piece;
        public Square(int x, int y)
        {
            Coords = new Coords(x, y);
            _piece = null;
        }
        public IPiece Piece
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
                    PieceChanged.Invoke(this, new PieceChangedEventArgs(this, Piece));
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
