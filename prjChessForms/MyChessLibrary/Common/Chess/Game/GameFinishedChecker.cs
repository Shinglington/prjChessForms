using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjChessForms.MyChessLibrary
{
    class GameFinishedChecker : IGameFinishedChecker
    {
        public event EventHandler<GameOverEventArgs> GameOver;

        private readonly IBoard _board;
        private readonly IPlayerHandler _playerHandler;
        private readonly IRulebook _rulebook;
        public GameFinishedChecker(IBoard board, IPlayerHandler playerHandler, IRulebook rulebook)
        {
            _board = board;
            _playerHandler = playerHandler;
        }
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
            GameOverEventArgs gameOverEventArgs = new GameOverEventArgs() { Result = result, Winner = winner };
            if (result != GameResult.Unfinished && GameOver != null)
            {
                GameOver.Invoke(this, gameOverEventArgs);
            }
            return gameOverEventArgs;
        }

        private bool IsInCheckmate(IPlayer player)
        {
            PieceColour colour = player.Colour;
            if (!_rulebook.IsInCheck(player))
            {
                return false;
            }
            return !CheckIfThereAreRemainingLegalMoves(player);
        }

        private bool IsInStalemate(IPlayer player)
        {
            PieceColour colour = player.Colour;
            if (_rulebook.IsInCheck(player))
            {
                return false;
            }
            return !CheckIfThereAreRemainingLegalMoves(player);
        }

        private bool CheckIfThereAreRemainingLegalMoves(IPlayer player)
        {
            PieceColour colour = player.Colour;
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
