using System;
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
            Square = square;

            string imageName = _colour.ToString() + "_" + this.GetType().Name;
            _image = (Image) Properties.Resources.ResourceManager.GetObject(imageName);
        }

        public PieceColour Colour
        {
            get
            {
                return _colour;
            }
        }

        public Coords Coords
        {
            get
            {
                return _square.Coords;
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
                if (_square != null && value != null)
                {
                    // Set the original square's PieceInSquare to null
                    _square.PieceInSquare = null;
                }
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
