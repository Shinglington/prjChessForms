using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class ModelChangedEventArgs : EventArgs 
    {
        public ModelChangedEventArgs(Player white, Player black, Square[,] squares, Piece selectedPiece, List<Coords> possibleMoves)
        {
            WhitePlayer = white;
            BlackPlayer = black;
            Squares = squares;
            SelectedPiece = selectedPiece;
            PossibleMoves = possibleMoves;
        }

        public Player WhitePlayer { get; }
        public Player BlackPlayer { get; }
        public Square[,] Squares { get; }
        public Piece SelectedPiece { get; }
        public List<Coords> PossibleMoves { get; }

    }

    class GameOverEventArgs : EventArgs 
    {
        public GameOverEventArgs(GameResult result, Player winner)
        {
            Result = result;
            Winner = winner;
        }
        
        public GameResult Result { get; set; }
        public Player Winner { get; set; }
    }

}
