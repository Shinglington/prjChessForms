﻿using System.Collections.Generic;
namespace prjChessForms.MyChessLibrary
{
    class PieceProvider : IPieceProvider
    {
        private IBoard _board;
        public void SetBoard(IBoard board) => _board = board;
        public IPiece GetPieceAt(Coords coords) => _board.GetSquareAt(coords).Piece;
        public ICollection<IPiece> GetPieces(PieceColour colour)
        {
            ICollection<IPiece> pieces = new List<IPiece>();
            IPiece p;
            for (int y = 0; y < _board.RowCount; y++)
            {
                for (int x = 0; x < _board.ColumnCount; x++)
                {
                    p = GetPieceAt(new Coords(x, y));
                    if (p != null && p.Colour == colour)
                    {
                        pieces.Add(p);
                    }
                }
            }
            return pieces;
        }
    }
}