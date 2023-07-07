using prjChessForms.MyChessLibrary.Interfaces;
using System;

namespace prjChessForms.MyChessLibrary.Pieces
{
    class Queen : Piece
    {
        private readonly Rook _rookMovement;
        private readonly Bishop _bishopMovement;
        public Queen(PieceColour colour) : base(colour) 
        {
            _rookMovement = new Rook(colour);
            _bishopMovement = new Bishop(colour);
        }

        public override bool CanMove(IBoard board, Move move)
        {
            return _rookMovement.CanMove(board, move) || _bishopMovement.CanMove(board, move);
        }
    }
}
