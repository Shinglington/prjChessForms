namespace prjChessForms.MyChessLibrary
{
   public struct Coords
    {
        public static readonly Coords Null = new Coords(-1, -1);
        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }

        public bool IsNull => this.Equals(Null);

        public override string ToString() => $"{(char)('a' + X)}{Y + 1}";

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Coords))
            {
                return false;
            }
            else
            {
                Coords other = (Coords)obj;
                return other.X == X && other.Y == Y;
            }
        }
    }
}
