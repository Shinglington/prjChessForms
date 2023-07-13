using System;

namespace prjChessForms.MyChessLibrary
{
    public interface IGameFinishedChecker
    {
        event EventHandler<GameOverEventArgs> GameOver;
        GameOverEventArgs GetGameResult();
    }
}