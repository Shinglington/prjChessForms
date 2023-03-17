using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class PieceChangedEventArgs : EventArgs
    {
        public PieceChangedEventArgs(Square square, Piece newPiece)
        {
            NewPiece = newPiece;
            Square = square;
        }
        public Square Square { get; set; }
        public Piece NewPiece { get; set; }
    }

    class PieceSelectionChangedEventArgs : EventArgs
    {
        public PieceSelectionChangedEventArgs(Piece selectedPiece, List<ChessMove> validMoves)
        {
            SelectedPiece = selectedPiece;
            ValidMoves = validMoves;
        }
        public Piece SelectedPiece { get; set; }
        public List<ChessMove> ValidMoves { get; set; }
    }

    class GameOverEventArgs : EventArgs
    {
        public GameOverEventArgs(Player winner, GameResult result)
        {
            Result = result;
            Winner = winner;
        }
        public Player Winner { get; set; }
        public GameResult Result { get; set; }
    }




}
