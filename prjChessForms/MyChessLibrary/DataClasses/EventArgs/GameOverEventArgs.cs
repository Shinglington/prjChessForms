using System;

namespace prjChessForms.MyChessLibrary
{
    public class GameOverEventArgs : EventArgs
    {
        public GameOverEventArgs(GameResult result, IPlayer winner)
        {
            Result = result;
            Winner = winner;
        }

        public GameResult Result { get; set; }
        public IPlayer Winner { get; set; }
    }
}
