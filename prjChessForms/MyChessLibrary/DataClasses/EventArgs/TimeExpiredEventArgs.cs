namespace prjChessForms.MyChessLibrary
{
    public class TimeExpiredEventArgs
    {
        public TimeExpiredEventArgs(IPlayer player)
        {
            PlayerWhoseTimeExpired = player;

        }
        public IPlayer PlayerWhoseTimeExpired { get; }
    }
}