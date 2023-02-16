using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Piece(PieceColour colour)
        {
            _colour = colour;
        }

        public PieceColour Colour
        {
            get 
            { 
                return _colour; 
            }
        }

        public Image Image
        {
            get
            {
                return _image;
            }
            protected set
            {
                _image = value;
            }
        }
    }
}
