namespace prjChessForms.MyChessLibrary
{
    public interface IMoveChecker
    {
        bool CheckCanMove(IPlayer player, Coords coords);

    }
}
