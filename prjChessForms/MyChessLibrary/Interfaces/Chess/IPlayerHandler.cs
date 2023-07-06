namespace prjChessForms.MyChessLibrary
{
    public interface IPlayerHandler
    {
        void SetupPlayers();
        IPlayer GetPlayer(PieceColour color);
    }
}
