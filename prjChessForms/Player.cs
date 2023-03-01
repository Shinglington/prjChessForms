namespace prjChessForms
{
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
