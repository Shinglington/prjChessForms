using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms
{
    class Rulebook
    {

        public static bool CheckLegalMove(Board board, Player player, Coords start, Coords end)
        {
            bool legal = false;
            Piece movedPiece = board.GetPieceAt(start);
            Piece capturedPiece = board.GetPieceAt(end);
            if (movedPiece != null && movedPiece.Colour == player.Colour && !start.Equals(end))
            {
                if (movedPiece.CanMoveTo(board, end))
                {
                    if (capturedPiece == null || (capturedPiece.Colour != player.Colour))
                    {
                        legal = true;
                    }
                }
            }
            if (legal)
            {
                Console.WriteLine(start.ToString() + "->" + end.ToString() + "is legal");
            }
            else
            {
                Console.WriteLine(start.ToString() + "->" + end.ToString() + "is illegal");
            }
            return legal;
        }
    }
}
