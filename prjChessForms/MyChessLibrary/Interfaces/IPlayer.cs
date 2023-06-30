using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.Interfaces
{
    public interface IPlayer : IPlayerProperties, IPlayerMethods
    {


    }

    public interface IPlayerProperties
    {
        TimeSpan RemainingTime { get; }
        PieceColour Colour { get; }
        List<IPiece> CapturedPieces { get; }
    }

    public interface IPlayerMethods 
    {
        void TickTime(TimeSpan time);
        void AddCapturedPiece(IPiece piece);
    }


}
