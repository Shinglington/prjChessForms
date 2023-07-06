using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class Chess : IChess
    {
        private readonly IPlayerHandler _playerHandler;
        public Chess(IPlayerHandler playerHandler)
        {
            _playerHandler = playerHandler;
        }

        public IPlayer GetPlayer(PieceColour colour) => _playerHandler.GetPlayer(colour);

}
