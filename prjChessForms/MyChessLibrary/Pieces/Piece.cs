using System.Drawing;
using prjChessForms.MyChessLibrary.Interfaces;
namespace prjChessForms.MyChessLibrary
{
    public abstract class Piece : IPiece
    {
        public Piece(PieceColour colour)
        {
            Colour = colour;
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
        public Image Image { get; }
        public PieceColour Colour { get; }
        public string Name { get { return GetType().Name; } }
        public string Fullname { get { return $"{Colour} {Name}"; } }

        public override string ToString()
        {
            return Fullname;
        }
        public abstract bool CanMove(IBoard board, Coords startCoords, Coords endCoords);
    }
}
