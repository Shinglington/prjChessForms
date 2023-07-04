using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.Interfaces
{
    public interface IBoardCreator
    {
        void SetupBoard();
    }

    public interface IStartingPositionSetup
    {
        void PlaceStartingPieces();
    }

    public interface IPiecePlacer 
    {
        void PlacePiece(IPiece piece, Coords coords);
    }

}
