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
        List<IPiece> GetPieces(PieceColour colour);
        IPiece GetPieceAt(Coords coords);
        Coords GetCoordsOfPiece(IPiece piece);
        ISquare[,] GetSquares();
        ISquare GetSquareAt(Coords coords);

    }

    public interface ISetupBoard 
    { 
        void SetupBoard();
        void PlaceStartingPieces();
        void PlacePiece(IPiece piece, Coords coords);
    }


}
