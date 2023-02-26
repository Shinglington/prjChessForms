using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace prjChessForms
{

    public struct ChessMove
    {
        private Coords _startCoords;
        private Coords _endCoords;

        public ChessMove(Coords startCoords, Coords endCoords)
        {
            _startCoords = startCoords;
            _endCoords = endCoords;
        }

        public Coords StartCoords 
        { 
            get 
            { 
                return _startCoords; 
            } 
        }
        public Coords EndCoords 
        { 
            get 
            { 
                return _endCoords; 
            } 
        }

        public override string ToString()
        {
            return StartCoords.ToString() + " -> " + EndCoords.ToString();
        }
    }

    abstract class Player
    {
        private PieceColour _colour;
        public Player(PieceColour colour)
        {
            _colour = colour;
        }

        public PieceColour Colour 
        {
            get 
            { 
                return _colour; 
            }
        }
        public abstract Task<ChessMove> GetMove(Board board);
    }

    class HumanPlayer : Player
    {
        private TaskCompletionSource<Coords> _squareClickedSource;
        public HumanPlayer(PieceColour colour) : base(colour) { }

        public override async Task<ChessMove> GetMove(Board board) 
        {
            bool validMove = false;
            Coords Start = new Coords();
            Coords End = new Coords();
            bool validStart = false;
            do
            {
                while (!validStart)
                {
                    Start = await GetCoordsOfClickedSquare(board);
                    if (board.GetPieceAt(Start) != null && board.GetPieceAt(Start).Colour == Colour)
                    {
                        validStart = true;
                    }
                }
                End = await GetCoordsOfClickedSquare(board);
                // If second selected square is of own player's piece, swap that to start square
                if (board.GetPieceAt(End) != null && board.GetPieceAt(Start).Colour == Colour)
                {
                    Start = End;
                }
                else if (Rulebook.CheckLegalMove(board, this, Start, End))
                {
                    validMove = true;
                }
            } while (!validMove);

            return new ChessMove(Start, End);
        }

        private void ReceiveSquareClickInfo(object sender, EventArgs e)
        {
            _squareClickedSource.SetResult(((Square) sender).Coords);
        }

        private async Task<Coords> GetCoordsOfClickedSquare(Board board)
        {
            _squareClickedSource = new TaskCompletionSource<Coords>();
            board.RaiseSquareClicked += ReceiveSquareClickInfo;
            Coords coords = await _squareClickedSource.Task;
            board.RaiseSquareClicked -= ReceiveSquareClickInfo;
            return coords;
        }
    }

    class ComputerPlayer : Player 
    { 
        public ComputerPlayer(PieceColour colour) : base(colour) { }

        public override async Task<ChessMove> GetMove(Board board) 
        {
            Coords Start = new Coords();
            Coords End = new Coords();


            return new ChessMove(Start, End);
        }
    }


}
