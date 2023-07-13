using System;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface IGameHandler
    {
        Task<GameOverEventArgs> PlayGame(TimeSpan playerTime);
    }
}
