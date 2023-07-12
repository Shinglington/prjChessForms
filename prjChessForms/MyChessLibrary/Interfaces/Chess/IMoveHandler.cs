using System.Threading;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface IMoveHandler
    {
        void ReceiveMoveInput(Coords clickedCoords);
        Task<IChessMove> GetChessMove(CancellationToken cToken);
        void AttemptMakeMove(IBoard board, Coords StartCoords, Coords EndCoords);

    }
}
