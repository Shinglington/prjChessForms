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
        public PlayerCapturedPiecesChangedEventArgs(Player player)
        {
            CapturedPieces = player.CapturedPieces;
        }
        public List<Piece> CapturedPieces { get; set; }
    }

    class PromotionEventArgs : EventArgs
    {
        public PromotionEventArgs(Pawn piece, Coords coords)
        {
            PromotingPiece = piece;
            PromotingCoords = coords;
        }
        public Pawn PromotingPiece { get; set; }
        public Coords PromotingCoords { get; set; }
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
