using prjChessForms.MyChessLibrary.DataClasses.ChessMoves;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class MoveHandler : IMoveHandler
    {
        private readonly IRulebook _rulebook;
        private readonly ICoordSelectionHandler _coordsSelectionHandler;

        private SemaphoreSlim _semaphoreReceiveClick;
        private bool _awaitingClick;
        private Coords _clickedCoords;
        public MoveHandler(IRulebook rulebook, ICoordSelectionHandler coordSelectionHandler)
        {
            _rulebook = rulebook;
            _coordsSelectionHandler = coordSelectionHandler;

            _semaphoreReceiveClick = new SemaphoreSlim(0, 1);
            _awaitingClick = false;
        }

        public void ReceiveMoveInput(Coords coords)
        {
            if (_awaitingClick)
            {
                Debug.WriteLine("Model received coords input of {0}", coords);
                _clickedCoords = coords;
                _semaphoreReceiveClick.Release();
            }
        }

        public async Task<IChessMove> GetChessMove(PieceColour colourOfMover, CancellationToken cToken)
        {
            Coords fromCoords = Coords.Null;

            IChessMove chessMove = null;
            bool completeInput = false;
            _awaitingClick = true;
            while (chessMove == null)
            {
                Debug.WriteLine("Waiting for click");
                await _semaphoreReceiveClick.WaitAsync(cToken);
                Debug.WriteLine("Received click at {0}", _clickedCoords);

                if (_rulebook.CheckFirstSelectedCoords(colourOfMover, _clickedCoords))
                {
                    _coordsSelectionHandler.ChangeCoordsSelection(_clickedCoords);
                    fromCoords = _clickedCoords;
                }
                else if (!fromCoords.Equals(Coords.Null))
                {
                    chessMove = _rulebook.ProcessChessMove(colourOfMover, fromCoords, _clickedCoords);

                    // if still null, invalid move, so assume player wants to deselect piece
                    if (chessMove == null)
                    {
                        fromCoords = null;
                        _coordsSelectionHandler.ChangeCoordsSelection(fromCoords);
                    }
                }
            }
            _coordsSelectionHandler.ChangeCoordsSelection(null);
            _awaitingClick = false;
            return chessMove;
        }

        public void AttemptMakeMove(IBoard board, IChessMove move)
        {
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
