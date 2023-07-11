namespace prjChessForms.MyChessLibrary
{
    public interface IPlayerManager
    {
        void SetupPlayers();
        IPlayer GetPlayer(PieceColour color);
    }
}
