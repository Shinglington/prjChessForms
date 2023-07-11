using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.DataClasses.ChessMoves
{
    public class ChessMove : IChessMove
    {
        private List<IChessMove> _moveSequence;
        public ChessMove(List<IChessMove> moveSequence) 
        {
            _moveSequence = moveSequence;
        }

        public void ExecuteMove(IBoard board)
        {
            try
            {
                for (int i = 0; i < _moveSequence.Count; i++)
                    _moveSequence[i].ExecuteMove(board);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReverseMove(IBoard board)
        {
            try
            {
                for (int i = _moveSequence.Count - 1; i >= 0; i--)
                    _moveSequence[i].ReverseMove(board);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
