using System;
using System.Collections;

namespace prjChessForms.MyChessLibrary
{
    public interface IPlayerHandler
    {
        void SetupPlayers(TimeSpan gameTime);
        IPlayer GetCurrentPlayer();
        IPlayer GetPlayer(PieceColour colour);
        void NextPlayerTurn();
    }
}
