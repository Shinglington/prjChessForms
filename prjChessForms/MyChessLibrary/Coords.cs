namespace prjChessForms.MyChessLibrary
{
   public struct Coords
    {
        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }

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

        public override int GetHashCode()
        {
            int hashCode = 367829482;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
