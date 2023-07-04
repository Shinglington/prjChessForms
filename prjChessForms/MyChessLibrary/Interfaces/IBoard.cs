using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.Interfaces
{
    public interface IBoard
    {
        int ColumnCount { get; }
        int RowCount { get; }
        ISquare[,] GetSquares();
        void SetSquares(ISquare[,] squares);
        ISquare GetSquareAt(Coords coords);
    }








}
