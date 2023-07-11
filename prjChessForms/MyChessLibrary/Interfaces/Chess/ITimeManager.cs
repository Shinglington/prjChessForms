using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    public interface ITimeManager
    {
        void SetupWithPlayers(IPlayerManager playerManager);
        void StartTimer();
        void StopTimer();
    }
}
