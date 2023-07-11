using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface IChess
    {
        IPlayer GetPlayer(PieceColour colour);
        Task PlayGame();
        void SendCoords(Coords coords);
        void SendPromotion(PromotionOption option);
    }
}
