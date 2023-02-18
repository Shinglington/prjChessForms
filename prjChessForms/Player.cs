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
        private Coords _selected;
        private TaskCompletionSource<bool> _squareClicked;
        public HumanPlayer(PieceColour colour) : base(colour) 
        {
            _selected = new Coords();
        }

        public override async Task<ChessMove> GetMove(Board board) 
        {
            Coords Start = new Coords();
            _squareClicked = new TaskCompletionSource<bool>();
            board.RaiseSquareClicked += ReceiveStartSquareClickInfo;
            _squareClicked.Task.Wait();
            Start = _selected;
            board.RaiseSquareClicked -= ReceiveStartSquareClickInfo;

            Coords End = new Coords();
            _squareClicked = new TaskCompletionSource<bool>();
            board.RaiseSquareClicked += ReceiveEndSquareClickInfo;
            _squareClicked.Task.Wait();
            End =  _selected;
            board.RaiseSquareClicked -= ReceiveEndSquareClickInfo;
            return new ChessMove(Start, End);
        }
        public async void ReceiveStartSquareClickInfo(object sender, SquareClickedEventArgs e)
        {
            _squareClicked.SetResult(true);
            _selected = e.Square.Coords;
        }

        public async void ReceiveEndSquareClickInfo(object sender, SquareClickedEventArgs e)
        {
            _squareClicked.SetResult(true);
            _selected = e.Square.Coords;
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
