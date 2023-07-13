using System;

namespace prjChessForms.MyChessLibrary
{
    public class GameOverEventArgs : EventArgs
    {
        public GameResult Result { get; }
        public IPlayer Winner { get; }
    }
}
