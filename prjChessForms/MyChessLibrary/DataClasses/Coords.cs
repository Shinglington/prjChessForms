using System;
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
        public Coords(string str)
        {
            try
            {
                X = int.Parse(str[1].ToString());
                Y = Char.ToLower(str[0]) - 'a';
                if (Y < 0 || Y > 8)
                {
                    throw new ArgumentOutOfRangeException(String.Format("{0} is not a valid coordinate", str));
                }
            }
            catch
            {
                throw new ArgumentOutOfRangeException(String.Format("{0} is not a valid coordinate", str));
            }
        }
        public int X { get; }
        public int Y { get; }
        public bool IsNull => this.Equals(Null);
        public static implicit operator Coords(string str) => new Coords(str);

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
