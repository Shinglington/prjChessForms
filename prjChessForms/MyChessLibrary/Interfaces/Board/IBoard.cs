﻿using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
    public interface IBoard
    {
        int ColumnCount { get; }
        int RowCount { get; }
        ISquare[,] GetSquares();
        void SetSquares(ISquare[,] squares);
        ISquare GetSquareAt(Coords coords);
        IPiece GetPieceAt(Coords coords);
        ICollection<IPiece> GetPieces(PieceColour colour);
        void MakeMove(ChessMove move);
    }








}