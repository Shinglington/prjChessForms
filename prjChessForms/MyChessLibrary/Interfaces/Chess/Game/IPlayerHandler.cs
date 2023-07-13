using System;

namespace prjChessForms.MyChessLibrary
{
    public interface IPlayerHandler
    {
        void SetupPlayers(TimeSpan gameTime);
        IPlayer GetCurrentPlayer();
        void NextPlayerTurn();
    }
}
