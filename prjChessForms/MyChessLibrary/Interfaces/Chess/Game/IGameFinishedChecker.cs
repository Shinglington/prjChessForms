using System;
using System.Threading;

namespace prjChessForms.MyChessLibrary
{
    public interface IGameFinishedChecker
    {
        event EventHandler<GameOverEventArgs> GameOver;
        CancellationToken cToken { get; set; }
        GameOverEventArgs GetGameResult();
    }
}