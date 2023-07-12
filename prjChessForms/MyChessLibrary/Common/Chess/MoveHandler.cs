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
        private readonly IMoveChecker _moveChecker;

        private SemaphoreSlim _semaphoreReceiveClick;
        private bool _awaitingClick;
        private Coords _clickedCoords;
        public MoveHandler(IRulebook rulebook, IMoveChecker moveChecker)
        {
            _rulebook = rulebook;
            _moveChecker = moveChecker;

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

        public async Task<IChessMove> GetChessMove(CancellationToken cToken)
        {
            Coords fromCoords = Coords.Null;
            Coords toCoords = Coords.Null;
            bool completeInput = false;
            _awaitingClick = true;
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

        private void ChangeSelection(Piece selectedPiece)
        {
            if (PieceSelectionChanged != null)
            {
                List<Coords> endCoords = new List<Coords>();
                Coords selectedCoords = new Coords();
                if (selectedPiece != null)
                {
                    selectedCoords = GetCoordsOf(selectedPiece);
                    foreach (PieceMovement m in FullRulebook.GetPossibleMoves(_board, selectedPiece))
                    {
                        endCoords.Add(m.EndCoords);
                    }
                }
                PieceSelectionChanged.Invoke(this, new PieceSelectionChangedEventArgs(selectedPiece, selectedCoords, endCoords));
            }
        }
    }
}
