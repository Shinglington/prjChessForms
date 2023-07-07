using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace prjChessForms.MyChessLibrary
{
    class MoveMaker : IMoveMaker
    {
        private IBoard _board;
        private Stack<Move> _moveStack;
        public void SetBoard(IBoard board)
        {
            _board = board;
            ResetMoveStack();
        }
        public void MakeMove(Move move)
        {
            try
            {
                MovePiece(move);
                _moveStack.Push(move);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UndoLastMove()
        {
            Move lastMove = _moveStack.Pop();
            MovePiece(new Move(lastMove.MovedPiece, lastMove.EndCoords, lastMove.StartCoords));
        }

        public Move GetLastMove(Move move) => _moveStack.Peek();

        private void ResetMoveStack()
        {
            _moveStack = new Stack<Move>();
        }

        private void MovePiece(Move move)
        {
            Coords StartCoords = move.StartCoords;
            Coords EndCoords = move.EndCoords;
            IPiece p = move.MovedPiece;
            if (p != null)
            {
                _board.GetSquareAt(EndCoords).Piece = p;
                _board.GetSquareAt(StartCoords).Piece = null;
                p.HasMoved = true;
                return;
            }
            throw new ArgumentException(string.Format("Invalid move {0}", move));
        }
    }
}
