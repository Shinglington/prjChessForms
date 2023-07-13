using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    class PlayerHandler : IPlayerHandler
    {
        private List<IPlayer> _players;
        private int _currentPlayerIndex;
       
        public void SetupPlayers(TimeSpan gameTime)
        {
            _players = new List<IPlayer>();
            foreach (PieceColour colour in Enum.GetValues(typeof(PieceColour)))
            {
                _players.Add(new HumanPlayer(colour, gameTime));
            }
            _currentPlayerIndex = 0;
        }

        public IPlayer GetCurrentPlayer() => _players[_currentPlayerIndex];

        public void NextPlayerTurn() => _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
    }
}
