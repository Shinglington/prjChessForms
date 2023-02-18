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
        public abstract Task<ChessMove> GetMoveAsync(Board board);
    }

    class HumanPlayer : Player
    {
        private Coords? _selected;
        public HumanPlayer(PieceColour colour) : base(colour) 
        {
            _selected = new Coords();
        }

        public override async Task<ChessMove> GetMoveAsync(Board board) 
        {
            Coords Start = new Coords();
            _selected = null;
            board.RaiseSquareClicked += ReceiveStartSquareClickInfo;
            while(_selected == null)
            {
                await TaskDelay();
            }
            Start = (Coords)_selected;
            board.RaiseSquareClicked -= ReceiveStartSquareClickInfo;

            Coords End = new Coords();
            _selected = null;
            board.RaiseSquareClicked += ReceiveEndSquareClickInfo;
            // Wait for other event to be triggered?
            while (_selected == null)
            {
                await TaskDelay();
            }
            End = (Coords) _selected;
            board.RaiseSquareClicked -= ReceiveEndSquareClickInfo;
            return new ChessMove(Start, End);
        }

        public void ReceiveStartSquareClickInfo(object sender, SquareClickedEventArgs e)
        {
            _selected = e.Square.Coords;
        }

        public void ReceiveEndSquareClickInfo(object sender, SquareClickedEventArgs e)
        {
            _selected = e.Square.Coords;
        }

        async Task TaskDelay()
        {
            await Task.Delay(100);
        }
    }

    class ComputerPlayer : Player 
    { 
        public ComputerPlayer(PieceColour colour) : base(colour) { }

        public override async Task<ChessMove> GetMoveAsync(Board board) 
        {
            Coords Start = new Coords();
            Coords End = new Coords();


            return new ChessMove(Start, End);
        }
    }


}
