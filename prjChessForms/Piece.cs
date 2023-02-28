using System;
using System.Drawing;

namespace prjChessForms
{
    public enum PieceColour
    {
        White,
        Black
    }
    abstract class Piece
    {
        public Piece(Player player)
        {
            Owner = player;
            string imageName = Colour.ToString() + "_" + this.GetType().Name;
            Image = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
        }

        public bool HasMoved { get; set; }

        public Player Owner { get; }

        public Image Image { get; }

        public PieceColour Colour { get { return Owner.Colour; } }

        public abstract bool CanMove(Board board, Coords startCoords, Coords endCoords);
    }
    class Pawn : Piece
    {
        public Pawn(Player player) : base(player) { }

        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;

            int direction = yChange > 0 ? 1 : -1;
            if (direction != (Owner.Colour == PieceColour.White ? 1 : -1))
            {
                return false;
            }

            if (xChange == 0 && board.GetPieceAt(endCoords) == null)
            {
                if (Math.Abs(yChange) == 1)
                {
                    allowed = true;
                }
                else if (Math.Abs(yChange) == 2 && !HasMoved
                    && board.GetPieceAt(new Coords(startCoords.X, startCoords.Y + direction)) == null)
                {
                    allowed = true;
                }
            }

            if (Math.Abs(xChange) == 1 && Math.Abs(yChange) == 1)
            {
                if (board.GetPieceAt(endCoords) != null)
                {
                    allowed = true;
                }
            }
            return allowed;
        }
    }

    class Knight : Piece
    {
        public Knight(Player player) : base(player) { }

        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = Math.Abs(endCoords.X - startCoords.X);
            int yChange = Math.Abs(endCoords.Y - startCoords.Y);
            if ((xChange == 2 && yChange == 1) || (xChange == 1 && yChange == 2))
            {
                allowed = true;
            }
            return allowed;
        }
    }

    class Bishop : Piece
    {
        public Bishop(Player player) : base(player) { }
        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            if (Math.Abs(xChange) == Math.Abs(yChange))
            {
                allowed = true;
                int xDirection = xChange > 0 ? 1 : -1;
                int yDirection = yChange > 0 ? 1 : -1;
                for (int delta = 1; delta < Math.Abs(xChange); delta += 1)
                {
                    Coords checkCoords = new Coords(startCoords.X + delta * xDirection, startCoords.Y + delta * yDirection);
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
        public Rook(Player player) : base(player) { }
        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            if (xChange == 0 && yChange != 0)
            {
                allowed = true;
                int direction = yChange > 0 ? 1 : -1;
                for (int deltaY = 1; deltaY < Math.Abs(yChange); deltaY += 1)
                {
                    Coords checkCoords = new Coords(startCoords.X, startCoords.Y + deltaY * direction);
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
                    Coords checkCoords = new Coords(startCoords.X + deltaX * direction, startCoords.Y);
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
        public Queen(Player player) : base(player) { }
        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            return BishopMove(board, startCoords, endCoords) || RookMove(board, startCoords, endCoords);
        }

        private bool BishopMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            if (Math.Abs(xChange) == Math.Abs(yChange))
            {
                allowed = true;
                int xDirection = xChange > 0 ? 1 : -1;
                int yDirection = yChange > 0 ? 1 : -1;
                for (int delta = 1; delta < Math.Abs(xChange); delta += 1)
                {
                    Coords checkCoords = new Coords(startCoords.X + delta * xDirection, startCoords.Y + delta * yDirection);
                    if (board.GetPieceAt(checkCoords) != null)
                    {
                        allowed = false;
                        break;
                    }
                }
            }
            return allowed;
        }

        private bool RookMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            if (xChange == 0 && yChange != 0)
            {
                allowed = true;
                int direction = yChange > 0 ? 1 : -1;
                for (int deltaY = 1; deltaY < Math.Abs(yChange); deltaY += 1)
                {
                    Coords checkCoords = new Coords(startCoords.X, startCoords.Y + deltaY * direction);
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
                    Coords checkCoords = new Coords(startCoords.X + deltaX * direction, startCoords.Y);
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
        public King(Player player) : base(player) { }
        public override bool CanMove(Board board, Coords startCoords, Coords endCoords)
        {
            bool allowed = false;
            int xChange = endCoords.X - startCoords.X;
            int yChange = endCoords.Y - startCoords.Y;
            if (Math.Abs(xChange) <= 1 && Math.Abs(yChange) <= 1)
            {
                allowed = true;
            }
            return allowed;
        }
    }
}
