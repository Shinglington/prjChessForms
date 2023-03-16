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
    class RequestMoveEventArgs : EventArgs
    {
        public RequestMoveEventArgs(Player player, CancellationToken cToken)
        {
            CToken = cToken;
            PlayerToMove = player;
        }
        public CancellationToken CToken { get; set; }
        public Player PlayerToMove { get; set; }
    }
    class Chess
    {
        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<RequestMoveEventArgs> RequestMove;
        public event EventHandler<EventArgs> MoveReceived;

        private Board _board;
        private System.Timers.Timer _timer;
        private CancellationTokenSource cts = new CancellationTokenSource();

        private GameResult _result;
        private Player[] _players;
        private int _turnCount;
        public Chess(Controller controller)
        {
            Controller = controller;
            CreatePlayers();
            _board = new Board(_players);
            _timer = new System.Timers.Timer(1000);
        }
        public Controller Controller { get; }
        public Player CurrentPlayer { get { return _players[_turnCount % 2]; } }
        public Player WhitePlayer { get { return _players[0]; } }
        public Player BlackPlayer { get { return _players[1]; } }
        public Square[,] BoardSquares { get { return _board.GetSquares(); } }
        public Piece GetPieceAt(Coords coords) => _board.GetPieceAt(coords);

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
                    RequestMove.Invoke(this, new RequestMoveEventArgs(CurrentPlayer, cToken));
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
        private void SetupControllerEvents()
        {

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
            if (GameOver != null)
            {
                GameOver.Invoke(this, new GameOverEventArgs(winner, _result));
            }
        }
    }
}
