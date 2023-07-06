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
            Coords StartCoords = move.StartCoords;
            Coords EndCoords = move.EndCoords;
            IPiece p = _board.GetPieceAt(StartCoords);
            if (p != null)
            {
                _board.GetSquareAt(EndCoords).Piece = p;
                _board.GetSquareAt(StartCoords).Piece = null;
                p.HasMoved = true;
            }
        }

        public void UndoLastMove()
        {
            throw new System.NotImplementedException();
        }

        public ChessMove GetLastMove(ChessMove move) => _moveStack.Peek();

        private void ResetMoveStack()
        {
            _moveStack = new Stack<ChessMove>();
        }
    }
}
