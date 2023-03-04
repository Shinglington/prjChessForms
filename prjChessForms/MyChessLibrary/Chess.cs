using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace prjChessForms.MyChessLibrary
{
    class GameOverEventArgs : EventArgs
    {
        public GameOverEventArgs(Player winner, GameResult result) 
        {
            Result = result;
            Winner = winner;
        }
        public Player Winner { get; set; }
        public GameResult Result { get; set; }

    }
    class Chess
    {
        public event EventHandler<GameOverEventArgs> GameOver;

        private Board _board;
        private System.Timers.Timer _timer;
        private CancellationTokenSource cts = new CancellationTokenSource();

        private GameResult _result;
        private Player[] _players;
        private int _turnCount;
        public Chess()
        {
            CreatePlayers();
        }

        public Player CurrentPlayer { get { return _players[_turnCount % 2]; } }

        public Square[,] BoardSquares { get { return _board.GetSquares(); } }


        public async Task StartGame()
        {
            await Play(cts.Token);
            OnGameOver();
        }

        public async Task Play(CancellationToken cToken)
        {
            _turnCount = 0;
            _result = GameResult.Unfinished;
            _timer.Elapsed += OnPlayerTimerTick;
            while (_result == GameResult.Unfinished)
            {
                try
                {
                    _timer.Start();
                    ChessMove move = await CurrentPlayer.GetMove(cToken);
                    _timer.Stop();

                    if (Rulebook.CheckLegalMove(_board, CurrentPlayer, move))
                    {
                        Rulebook.MakeMove(_board, CurrentPlayer, move);
                        _result = Rulebook.GetGameResult(_board, CurrentPlayer);
                        _turnCount++;
                    }
                }
                catch when (cToken.IsCancellationRequested)
                {
                    _result = GameResult.Time;
                }
            }
            cts.Cancel();
            _timer.Elapsed -= OnPlayerTimerTick;
        }
        private void CreatePlayers()
        {
            _players = new Player[2];
            _players[0] = new HumanPlayer(PieceColour.White, new TimeSpan(0, 3, 0));
            _players[1] = new HumanPlayer(PieceColour.Black, new TimeSpan(0, 3, 0));
        }

        private void OnPlayerTimerTick(object sender, ElapsedEventArgs e)
        {
            CurrentPlayer.TickTime(new TimeSpan(0, 0, 1));
            if (CurrentPlayer.RemainingTime > TimeSpan.Zero)
            {
                _timer.Elapsed -= OnPlayerTimerTick;
                cts.Cancel();
            }
        }
        private void OnGameOver()
        {
            cts.Cancel();
            Player winner = null;
            if (_result == GameResult.Checkmate || _result == GameResult.Time)
            {
                winner = CurrentPlayer == _players[0] ? _players[1] : _players[0];
            }
            GameOver.Invoke(this, new GameOverEventArgs(winner, _result));
        }
    }
}
