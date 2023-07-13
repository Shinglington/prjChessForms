using System;

namespace prjChessForms.MyChessLibrary
{
    public class GameOverEventArgs : EventArgs
    {
        public GameResult Result { get; set; }
        public IPlayer Winner { get; set; }
    }
}
