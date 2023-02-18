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
        protected System.Windows.Forms.Timer moveTimer;
        public Player(PieceColour colour, System.Windows.Forms.Timer timer)
        {
            _colour = colour;
            moveTimer = timer;
        }

        public PieceColour Colour 
        {
            get 
            { 
                return _colour; 
            }
        }
        public abstract ChessMove GetMove(Board board);
    }

    class HumanPlayer : Player
    {
        private AutoResetEvent startSquareClicked = new AutoResetEvent(false);
        private AutoResetEvent endSquareClicked = new AutoResetEvent(false);

        private Coords _selected;
        public HumanPlayer(PieceColour colour, System.Windows.Forms.Timer timer) : base(colour, timer) 
        {
            _selected = new Coords();
        }

        public override ChessMove GetMove(Board board) 
        {
            // Get start move
            Coords Start = new Coords();
            board.RaiseSquareClicked += ReceiveStartSquareClickInfo;
            moveTimer.Start();
            Start = _selected;
            board.RaiseSquareClicked -= ReceiveStartSquareClickInfo;

            Coords End = new Coords();
            board.RaiseSquareClicked += ReceiveEndSquareClickInfo;
            moveTimer.Start();
            End = _selected;
            board.RaiseSquareClicked -= ReceiveEndSquareClickInfo;
            moveTimer.Stop();
            return new ChessMove(Start, End);
        }

        public void ReceiveStartSquareClickInfo(object sender, SquareClickedEventArgs e)
        {
            _selected = e.Square.Coords;
            moveTimer.Stop();
        }

        public void ReceiveEndSquareClickInfo(object sender, SquareClickedEventArgs e)
        {
            _selected = e.Square.Coords;
            moveTimer.Stop();
        }
    }

    class ComputerPlayer : Player 
    { 
        public ComputerPlayer(PieceColour colour, System.Windows.Forms.Timer timer) : base(colour, timer) { }

        public override ChessMove GetMove(Board board) 
        {
            Coords Start = new Coords();
            Coords End = new Coords();


            return new ChessMove(Start, End);
        }
    }


}
