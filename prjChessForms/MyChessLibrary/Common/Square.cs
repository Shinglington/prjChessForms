using System;
using prjChessForms.MyChessLibrary.Pieces;
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
    }
}
