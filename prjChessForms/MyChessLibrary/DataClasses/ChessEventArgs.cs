using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public class SquareClickedEventArgs : EventArgs
    {
        public SquareClickedEventArgs(Coords coords)
        {
            ClickedCoords = coords;
        }
        public Coords ClickedCoords { get; set; }
    }

    public class PromotionSelectedEventArgs : EventArgs
    {
        public PromotionSelectedEventArgs(PromotionOption option)
        {
            SelectedOption = option;
        }

        public PromotionOption SelectedOption { get; set; }
    }

    public class PieceChangedEventArgs : EventArgs
    {
        public PieceChangedEventArgs(ISquare square, IPiece newPiece)
        {
            NewPiece = newPiece;
            Square = square;
        }
        public ISquare Square { get; set; }
        public IPiece NewPiece { get; set; }
    }

    public class PieceSelectionChangedEventArgs : EventArgs
    {
        public PieceSelectionChangedEventArgs(IPiece piece, Coords selectedPieceCoords, ICollection<Coords> validMoves)
        {
            SelectedPiece = piece;
            SelectedPieceCoords = selectedPieceCoords;
            PossibleEndCoords = validMoves;
        }
        public IPiece SelectedPiece { get; set; }
        public Coords SelectedPieceCoords { get; set; }
        public ICollection<Coords> PossibleEndCoords { get; set; }
    }

    public class PlayerTimerTickEventArgs : EventArgs
    {
        public PlayerTimerTickEventArgs(IPlayer player)
        {
            CurrentPlayer = player;
            PlayerRemainingTime = player.RemainingTime;
        }
        public IPlayer CurrentPlayer { get; set; }
        public TimeSpan PlayerRemainingTime { get; set; }
    }

    public class PlayerCapturedPiecesChangedEventArgs : EventArgs
    {
        public PlayerCapturedPiecesChangedEventArgs(IPlayer player, IPiece capturedPiece)
        {
            Player = player;
            CapturedPiece = capturedPiece;
        }
        public IPlayer Player { get; set; }
        public IPiece CapturedPiece { get; set; }
    }

    public class PromotionEventArgs : EventArgs
    {
        public PromotionEventArgs(PieceColour colour, Coords coords)
        {
            PromotingCoords = coords;
            PromotingColour = colour;
        }
        public Coords PromotingCoords { get; set; }
        public PieceColour PromotingColour { get; set; }
    }

    public class GameOverEventArgs : EventArgs
    {
        public GameResult Result { get; }
        public IPlayer Winner { get; }
    }
}
