using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace prjChessForms.MyChessLibrary
{
    public class RequestMoveEventArgs : EventArgs 
    {
        public RequestMoveEventArgs(CancellationToken cToken) 
        {
            CToken = cToken;
        }

        public CancellationToken CToken { get; set; }
    }

    public class SendMoveEventArgs : EventArgs
    {
        public SendMoveEventArgs(ChessMove move)
        {
            Move = move;
        }

        public ChessMove Move { get; set; }
    }
    abstract class Player
    {
        public event EventHandler<RequestMoveEventArgs> RequestMove;
        public event EventHandler TimeExpiredEvent;

        private SemaphoreSlim _semaphoreMoveSend = new SemaphoreSlim(0, 1);
        private ChessMove _selectedMove;
        public Player(PieceColour colour, TimeSpan initialTime)
        {
            Colour = colour;
            RemainingTime = initialTime;
        }
        public TimeSpan RemainingTime { get; private set; }
        public PieceColour Colour { get; }

        public void TickTime(TimeSpan time)
        {
            RemainingTime = RemainingTime.Subtract(time);
            if (RemainingTime < TimeSpan.Zero)
            {
                TimeExpiredEvent.Invoke(this, new EventArgs());
            }
        }
        public async Task<ChessMove> GetMove(CancellationToken cToken)
        {
            RequestMove.Invoke(this, new RequestMoveEventArgs(cToken));
            await _semaphoreMoveSend.WaitAsync(cToken);
            return _selectedMove;
        }

        public void OnMoveSent(ChessMove move)
        {
            _semaphoreMoveSend.Release();
            _selectedMove = move;
        }

    }

    class HumanPlayer : Player
    {
        public HumanPlayer(PieceColour colour, TimeSpan initialTime) : base(colour, initialTime) { }
    }

    class ComputerPlayer : Player
    {
        public ComputerPlayer(PieceColour colour, TimeSpan initialTime) : base(colour, initialTime) { }

    }

}
