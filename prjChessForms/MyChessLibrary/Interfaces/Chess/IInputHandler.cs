using System.Threading;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface IInputHandler
    {
        Task<IChessMove> GetChessMove(CancellationToken cToken);
    }
}
