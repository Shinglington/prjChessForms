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
        private TaskCompletionSource<Coords> _squareClickedSource;
        public HumanPlayer(PieceColour colour) : base(colour) 
        {
        }

        public override async Task<ChessMove> GetMove(Board board) 
        {
            _squareClickedSource = new TaskCompletionSource<Coords>();
            board.RaiseSquareClicked += ReceiveSquareClickInfo;
            var Start = await _squareClickedSource.Task;
            board.RaiseSquareClicked -= ReceiveSquareClickInfo;

            Console.WriteLine("Start received");

            _squareClickedSource = new TaskCompletionSource<Coords>();
            board.RaiseSquareClicked += ReceiveSquareClickInfo;
            var End = await _squareClickedSource.Task;
            board.RaiseSquareClicked -= ReceiveSquareClickInfo;

            Console.WriteLine("End received");

            return new ChessMove(Start, End);
        }
        public void ReceiveSquareClickInfo(object sender, SquareClickedEventArgs e)
        {
            _squareClickedSource.SetResult(e.Square.Coords);
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
