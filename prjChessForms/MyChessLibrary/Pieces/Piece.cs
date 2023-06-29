using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace prjChessForms.MyChessLibrary.Pieces
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
            try
            {
                Image = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
            }
            catch
            {
                Image = null;
            }
        }
        public bool HasMoved { get; set; }
        public Player Owner { get; }
        public Image Image { get; }
        public PieceColour Colour { get { return Owner.Colour; } }
        public string Name { get { return GetType().Name; } }
        public string Fullname { get { return $"{Colour} {Name}"; } }

        public override string ToString()
        {
            return Fullname;
        }
        public abstract bool CanMove(Board board, Coords startCoords, Coords endCoords);
    }
}
