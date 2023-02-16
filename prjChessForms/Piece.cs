using System.Drawing;
using System.IO;

namespace prjChessForms
{
    public enum PieceColour
    {
        White,
        Black
    }
    abstract class Piece
    {
        private Image _image;
        private PieceColour _colour;
        private Square _square;
        public Piece(PieceColour colour, Square square)
        {
            _colour = colour;
            _square = square;

            string imagePath = "Images\\" + _colour.ToString() + "\\" + this.GetType().Name + ".png";
            _image = File.Exists(imagePath) ? Image.FromFile(imagePath) : null;
        }

        public PieceColour Colour
        {
            get
            {
                return _colour;
            }
        }

        public int X
        {
            get
            {
                return _square.X;
            }
        }

        public int Y
        {
            get
            {
                return _square.Y;
            }
        }

        public Square Square 
        {
            get
            {
                return _square;
            }
            set
            {
                _square.PieceInSquare = null;
                _square = value;
                _square.PieceInSquare = this;
            }
        }

        public Image Image
        {
            get
            {
                return _image;
            }
        }
    }
    class Pawn : Piece
    {
        public Pawn(PieceColour colour, Square square) : base(colour, square) { }
    }
    class Knight : Piece
    {
        public Knight(PieceColour colour, Square square) : base(colour, square) { }
    }
    class Bishop : Piece
    {
        public Bishop(PieceColour colour, Square square) : base(colour, square) { }
    }
    class Rook : Piece
    {
        public Rook(PieceColour colour, Square square) : base(colour, square) { }
    }
    class Queen : Piece
    {
        public Queen(PieceColour colour, Square square) : base(colour, square) { }
    }
    class King : Piece 
    {
        public King(PieceColour colour, Square square) : base(colour, square) { }
    }
}
