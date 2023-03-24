using System;
using System.Collections.Generic;

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
        public PieceSelectionChangedEventArgs(Piece piece, Coords selectedPieceCoords, List<Coords> validMoves)
        {
            SelectedPiece = piece;
            SelectedPieceCoords = selectedPieceCoords;
            PossibleEndCoords = validMoves;
        }
        public Piece SelectedPiece { get; set; }
        public Coords SelectedPieceCoords { get; set; }
        public List<Coords> PossibleEndCoords { get; set; }
    }

    class PlayerTimerTickEventArgs : EventArgs
    {
        public PlayerTimerTickEventArgs(Player player)
        {
            CurrentPlayer = player;
            PlayerRemainingTime = player.RemainingTime;
        }
        public Player CurrentPlayer { get; set; }
        public TimeSpan PlayerRemainingTime { get; set; }
    }

    class PlayerCapturedPiecesChangedEventArgs : EventArgs
    {
        public PlayerCapturedPiecesChangedEventArgs(Player player, Piece capturedPiece)
        {
            Player = player;
            CapturedPiece = capturedPiece;
        }
        public Player Player { get; set; }
        public Piece CapturedPiece { get; set; }
    }

    class PromotionEventArgs : EventArgs
    {
        public PromotionEventArgs(PieceColour colour, Coords coords)
        {
            PromotingCoords = coords;
            PromotingColour = colour;
        }
        public Coords PromotingCoords { get; set; }
        public PieceColour PromotingColour { get; set; } 
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
