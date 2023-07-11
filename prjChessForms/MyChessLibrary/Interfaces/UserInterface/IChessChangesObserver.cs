using prjChessForms.PresentationUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary.Interfaces.Chess
{
    public interface IChessChangesObserver
    {
        void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e);
        void OnPieceSelectionChanged(object sender, PieceSelectionChangedEventArgs e);
        void OnPlayerCapturedPiecesChanged(object sender, PlayerCapturedPiecesChangedEventArgs e);
        void OnPromotion(object sender, PromotionEventArgs e);
        void OnGameOver(object sender, GameOverEventArgs e);
        void OnPlayerTimerTick(object sender, PlayerTimerTickEventArgs e);
    }
}
