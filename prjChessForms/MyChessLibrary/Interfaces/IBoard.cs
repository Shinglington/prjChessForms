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
        List<Piece> GetPieces(PieceColour colour);
        Piece GetPieceAt(Coords coords);
        Coords GetCoordsOfPiece(Piece piece);
    }

    public interface ISetupBoard 
    { 
        void SetupBoard();
        void PlaceStartingPieces();
        void PlacePiece(Piece piece, Coords coords);
    }


}
