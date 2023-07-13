using System;
using System.Threading;

namespace prjChessForms.MyChessLibrary
{
    public interface IGameFinishedChecker
    {
        event EventHandler<GameOverEventArgs> GameOver;
        CancellationTokenSource cts { get; set; }
        GameOverEventArgs GetGameResult();
    }
}