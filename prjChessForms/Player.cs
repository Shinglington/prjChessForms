using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace prjChessForms
{

    public struct Move 
    {
        private Coords _startCoords;
        private Coords _endCoords;

        public Move(Coords startCoords, Coords endCoords)
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
        public abstract Move GetMove(Board board);
    }

    class HumanPlayer : Player
    {
        public HumanPlayer(PieceColour colour) : base(colour) { }

        public override Move GetMove(Board board) 
        {
            // Get start move
            Coords Start = new Coords();
            board.AddClickEvents(OnPanelClick);

            Coords End = new Coords();


            return new Move(Start, End);
        }
        
        private void OnPanelClick(object sender, EventArgs e)
        {

        }
    }

    class ComputerPlayer : Player 
    { 
        public ComputerPlayer(PieceColour colour) : base(colour) { }

        public override Move GetMove(Board board) 
        {
            Coords Start = new Coords();
            Coords End = new Coords();


            return new Move(Start, End);
        }
    }


}
