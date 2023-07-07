using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.DataClasses.ChessMoves
{
    public class Capture : IChessMove
    {
        public Capture(IPiece capturedPiece, Coords originalCoords)
        {
            CapturedPiece= capturedPiece;
            OriginalCoords = originalCoords;
        }
        public IPiece CapturedPiece { get; }
        public Coords OriginalCoords { get; }

        public void ExecuteMove(IBoard board)
        {
            if (board.GetSquareAt())
        }

        public void ReverseMove(IBoard board)
        {
            throw new NotImplementedException();
        }
    }
}
