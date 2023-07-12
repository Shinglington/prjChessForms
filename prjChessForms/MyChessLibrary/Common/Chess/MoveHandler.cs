using prjChessForms.MyChessLibrary.DataClasses.ChessMoves;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class MoveHandler : IMoveHandler
    {
        private readonly IRulebook _rulebook;

        private bool _awaitingClick;
        public MoveHandler(IRulebook rulebook)
        {
            _rulebook = rulebook;
        }

        public Task<IChessMove> GetChessMove(CancellationToken cToken)
        {
            Coords fromCoords = Coords.Null;
            Coords toCoords = Coords.Null;
            bool completeInput = false;
            _awaitingClick = true;
            _timer.Start();
            while (!completeInput)
            {
                Debug.WriteLine("Waiting for click");
                await _semaphoreReceiveClick.WaitAsync(cToken);
                Debug.WriteLine("Received click at {0}", _clickedCoords);

                if (GetPieceAt(_clickedCoords) != null && GetPieceAt(_clickedCoords).Colour.Equals(CurrentPlayer.Colour))
                {
                    ChangeSelection(GetPieceAt(_clickedCoords));
                    fromCoords = _clickedCoords;
                    toCoords = Coords.Null;
                }
                else if (!fromCoords.Equals(Coords.Null))
                {
                    toCoords = _clickedCoords;
                }
                // Check if move is valid now
                if (!toCoords.IsNull && !fromCoords.IsNull && FullRulebook.CheckLegalMove(_board, CurrentPlayer.Colour, new ChessMove(fromCoords, toCoords)))
                {
                    move = new ChessMove(fromCoords, toCoords);
                    completeInput = true;
                }
                else
                {
                    toCoords = Coords.Null;
                }
            }
            _awaitingClick = false;
            _timer.Stop();
            return move;

        }

        public void AttemptMakeMove(IBoard board, Coords StartCoords, Coords EndCoords)
        {
            IChessMove move = _rulebook.ProcessChessMove(StartCoords, EndCoords);
            try
            {
                move.ExecuteMove(board);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
