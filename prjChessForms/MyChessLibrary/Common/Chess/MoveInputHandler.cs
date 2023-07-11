using System;

namespace prjChessForms.MyChessLibrary
{
    class MoveInputHandler : IMoveInputHandler
    {
        private readonly IRulebook _rulebook;
        public MoveInputHandler(IRulebook rulebook)
        {
            _rulebook = rulebook;
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
