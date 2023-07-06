using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class PlayerHandler : IPlayerHandler
    {
        private ICollection<IPlayer> _players;

        public void SetupPlayers()
        {
            _players = new List<IPlayer>();
            foreach (PieceColour colour in Enum.GetValues(typeof(PieceColour)))
            {
                _players.Add(new HumanPlayer(colour, new TimeSpan(0, 3, 0)));
            }
        }

        public IPlayer GetPlayer(PieceColour colour)
        {
            foreach (IPlayer player in _players)
            {
                if (player.Colour == colour)
                {
                    return player;
                }
            }
            throw new ArgumentException(string.Format("Player of Colour {0} not found", colour));
        }
    }
}
