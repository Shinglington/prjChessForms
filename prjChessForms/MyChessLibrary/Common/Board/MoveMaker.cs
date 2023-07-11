using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace prjChessForms.MyChessLibrary
{
    class MoveMaker : IMoveMaker
    {
        private IBoard _board;
        private Stack<IChessMove> _moveStack;
        public void SetBoard(IBoard board)
        {
            _board = board;
            ResetMoveStack();
        }
        public void MakeMove(IChessMove move)
        {
            try
            {
                move.ExecuteMove(_board);
                _moveStack.Push(move);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UndoLastMove()
        {
            IChessMove lastMove = _moveStack.Pop();
            lastMove.ReverseMove(_board);
        }

        public IChessMove GetLastMove() => _moveStack.Peek();

        private void ResetMoveStack()
        {
            _moveStack = new Stack<IChessMove>();
        }
    }
}
