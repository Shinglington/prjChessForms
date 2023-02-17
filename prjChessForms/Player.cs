using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Coords StartCoords { get { return _startCoords; } }
        public Coords EndCoords { get { return _endCoords; } }
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

        public 
    }

    class HumanPlayer : Player
    {
        public HumanPlayer(PieceColour colour) : base(colour) { }
    }

    class ComputerPlayer : Player 
    { 
        public ComputerPlayer(PieceColour colour) : base(colour) { }
    }


}
