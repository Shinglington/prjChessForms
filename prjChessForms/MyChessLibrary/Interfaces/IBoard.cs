using System.Collections.Generic;
using System.Windows.Forms;

namespace prjChessForms.MyChessLibrary.Interfaces
{
    public interface IBoard : IPlayableBoard, IViewableBoard
    { 

    }


    public interface IPlayableBoard
    {
        void MakeMove(ChessMove Move);
    }

    public interface IViewableBoard
    {
        ISquare GetSquareAt(Coords coords);
    }

    public interface IBoardCreator 
    {
        void SetupBoard();
        void PlaceStartingPieces();
    }

    public interface IPiecePlacer
    {
        void PlacePiece(IPiece piece, Coords coords);
    }


}
