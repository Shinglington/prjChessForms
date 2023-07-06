using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace prjChessForms.MyChessLibrary
{
    class MoveMaker : IMoveMaker
    {
        private IBoard _board;
        private Stack<ChessMove> _moveStack;
        public void SetBoard(IBoard board)
        {
            _board = board;
            ResetMoveStack();
        }
        public void MakeMove(ChessMove move)
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
            ChessMove lastMove = _moveStack.Pop();
            MovePiece(new ChessMove(lastMove.MovedPiece, lastMove.EndCoords, lastMove.StartCoords));
        }

        public ChessMove GetLastMove(ChessMove move) => _moveStack.Peek();

        private void ResetMoveStack()
        {
            _moveStack = new Stack<ChessMove>();
        }

        private void MovePiece(ChessMove move)
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
