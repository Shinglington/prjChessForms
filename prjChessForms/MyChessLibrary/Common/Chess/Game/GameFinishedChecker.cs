using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class GameFinishedChecker : IGameFinishedChecker
    {
        public event EventHandler<GameOverEventArgs> GameOver;

        private readonly IBoard _board;
        private readonly IPlayerHandler _playerHandler;
        private readonly IRulebook _rulebook;
        private readonly ICheckHandler _checkHandler;


        private CancellationTokenSource cts = new CancellationTokenSource();
        public GameFinishedChecker(IBoard board, IPlayerHandler playerHandler, ITimeManager timeManager, IRulebook rulebook, ICheckHandler checkHandler)
        {
            _board = board;
            _playerHandler = playerHandler;
            _rulebook = rulebook;
            _checkHandler = checkHandler;

            GameOver += (sender, e) => cts.Cancel();
            timeManager.TimeExpired += HandleTimeExpiration;
        }
        public CancellationToken cToken { get; set; }
        public GameOverEventArgs GetGameResult()
        {
            IPlayer currentPlayer = _playerHandler.GetCurrentPlayer();
            GameResult result = GameResult.Unfinished;
            IPlayer winner = null;
            if (IsInStalemate(currentPlayer))
            {
                result = GameResult.Stalemate;
            }
            else if (IsInCheckmate(currentPlayer))
            {
                result = GameResult.Checkmate;
                winner = currentPlayer;
            }
            GameOverEventArgs gameOverEventArgs = new GameOverEventArgs(result, winner);
            if (result != GameResult.Unfinished)
            {
                InvokeGameOver(this, gameOverEventArgs);
            }
            return gameOverEventArgs;
        }

        private void InvokeGameOver(object sender, GameOverEventArgs e)
        {
            if (GameOver != null)
            {
                GameOver.Invoke(this, e);
            }
        }

        private void HandleTimeExpiration(object sender, TimeExpiredEventArgs e)
        {
            PieceColour winnerColour = (e.PlayerWhoseTimeExpired.Colour == PieceColour.White ? PieceColour.Black : PieceColour.White);
            IPlayer winner = _playerHandler.GetPlayer(winnerColour);
            GameOverEventArgs gameOverEventArgs = new GameOverEventArgs(GameResult.Time, winner);
            InvokeGameOver(this, gameOverEventArgs);
        }

        private bool IsInCheckmate(IPlayer player)
        {
            PieceColour colour = player.Colour;
            return !CheckIfThereAreRemainingLegalMoves(colour);
        }

        private bool IsInStalemate(IPlayer player)
        {
            PieceColour colour = player.Colour;
            return !_checkHandler.IsInCheck(colour) && !CheckIfThereAreRemainingLegalMoves(colour);
        }

        private bool CheckIfThereAreRemainingLegalMoves(PieceColour colour)
        {
            bool anyLegalMoves = false;
            foreach (Piece p in _board.GetPieces(colour))
            {
                ICollection<IChessMove> moves = _rulebook.GetPossibleMovesForPiece(p);
                if (moves.Count > 0)
                {
                    anyLegalMoves = true;
                    break;
                }
            }
            return anyLegalMoves;
        }
    }
}
