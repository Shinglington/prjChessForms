using System;

namespace prjChessForms.MyChessLibrary
{
    public interface IPlayerHandler
    {
        void SetupPlayers(TimeSpan gameTime);
        IPlayer GetPlayer(PieceColour color);
    }
}
