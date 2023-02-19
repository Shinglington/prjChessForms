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

        public abstract bool CanMoveTo(Board board, Coords endCoords);

    }
    class Pawn : Piece
    {
        public Pawn(PieceColour colour, Square square) : base(colour, square) { }

        public override bool CanMoveTo(Board board, Coords endCoords)
        {
            int xChange = endCoords.X - Coords.X;
            int yChange = endCoords.Y - Coords.Y;


            return false;
        }
    }
    class Knight : Piece
    {
        public Knight(PieceColour colour, Square square) : base(colour, square) { }

        public override bool CanMoveTo(Board board, Coords endCoords)
        {
            bool allowed = false;
            int xChange = Math.Abs(endCoords.X - Coords.X);
            int yChange = Math.Abs(endCoords.Y - Coords.Y);
            if ((xChange == 2 && yChange == 1) || (xChange == 1 && yChange == 2))
            {
                allowed = true;
            }
            return allowed;
        }
    }
    class Bishop : Piece
    {
        public Bishop(PieceColour colour, Square square) : base(colour, square) { }
        public override bool CanMoveTo(Board board, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - Coords.X;
            int yChange = endCoords.Y - Coords.Y;
            if (Math.Abs(xChange) == Math.Abs(yChange))
            {
                allowed = true;
                int xDirection = xChange > 0 ? 1 : -1;
                int yDirection = yChange > 0 ? 1 : -1;
                for (int delta = 1; delta < Math.Abs(xChange); delta += 1)
                {
                    Coords checkCoords = new Coords(Coords.X + delta * xDirection, Coords.Y + delta * yDirection);
                    if (board.GetPieceAt(checkCoords) != null) 
                    {
                        allowed = false;
                        break;
                    }
                }
            }
            return allowed;
        }
    }
    class Rook : Piece
    {
        public Rook(PieceColour colour, Square square) : base(colour, square) { }
        public override bool CanMoveTo(Board board, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - Coords.X;
            int yChange = endCoords.Y - Coords.Y;
            if (xChange == 0 && yChange != 0)
            {
                allowed = true;
                int direction = yChange > 0 ? 1 : -1;
                for (int deltaY = 1; deltaY < Math.Abs(yChange); deltaY += 1)
                {
                    Coords checkCoords = new Coords(Coords.X, Coords.Y + deltaY * direction);
                    if (board.GetPieceAt(checkCoords) != null)
                    {
                        allowed = false;
                        break;
                    }
                }

            }
            else if (yChange == 0 && xChange != 0)
            {
                allowed = true;
                int direction = xChange > 0 ? 1 : -1;
                for (int deltaX = 1; deltaX < Math.Abs(xChange); deltaX += 1)
                {
                    Coords checkCoords = new Coords(Coords.X + deltaX * direction, Coords.Y);
                    if (board.GetPieceAt(checkCoords) != null)
                    {
                        allowed = false;
                        break;
                    }
                }
            }
            return allowed;
        }
    }
    class Queen : Piece
    {
        public Queen(PieceColour colour, Square square) : base(colour, square) { }
        public override bool CanMoveTo(Board board, Coords endCoords)
        {
            return BishopMove(board, endCoords) || RookMove(board, endCoords);
        }

        private bool BishopMove(Board board, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - Coords.X;
            int yChange = endCoords.Y - Coords.Y;
            if (Math.Abs(xChange) == Math.Abs(yChange))
            {
                allowed = true;
                int xDirection = xChange > 0 ? 1 : -1;
                int yDirection = yChange > 0 ? 1 : -1;
                for (int delta = 1; delta < Math.Abs(xChange); delta += 1)
                {
                    Coords checkCoords = new Coords(Coords.X + delta * xDirection, Coords.Y + delta * yDirection);
                    if (board.GetPieceAt(checkCoords) != null)
                    {
                        allowed = false;
                        break;
                    }
                }
            }
            return allowed;
        }

        private bool RookMove(Board board, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - Coords.X;
            int yChange = endCoords.Y - Coords.Y;
            if (xChange == 0 && yChange != 0)
            {
                allowed = true;
                int direction = yChange > 0 ? 1 : -1;
                for (int deltaY = 1; deltaY < Math.Abs(yChange); deltaY += 1)
                {
                    Coords checkCoords = new Coords(Coords.X, Coords.Y + deltaY * direction);
                    if (board.GetPieceAt(checkCoords) != null)
                    {
                        allowed = false;
                        break;
                    }
                }

            }
            else if (yChange == 0 && xChange != 0)
            {
                allowed = true;
                int direction = xChange > 0 ? 1 : -1;
                for (int deltaX = 1; deltaX < Math.Abs(xChange); deltaX += 1)
                {
                    Coords checkCoords = new Coords(Coords.X + deltaX * direction, Coords.Y);
                    if (board.GetPieceAt(checkCoords) != null)
                    {
                        allowed = false;
                        break;
                    }
                }
            }
            return allowed;
        }
    }
    
    class King : Piece 
    {
        public King(PieceColour colour, Square square) : base(colour, square) { }
        public override bool CanMoveTo(Board board, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - Coords.X;
            int yChange = endCoords.Y - Coords.Y;
            if (Math.Abs(xChange) <= 1 && Math.Abs(yChange) <= 1)
            {
                allowed = true;
            }
            return allowed;
        }
    }
}
