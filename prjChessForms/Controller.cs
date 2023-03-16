using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjChessForms
{
    class Controller
    {
        private CancellationToken cts = new CancellationToken();
        public Controller()
        {
            ChessGame = new Chess(this);
            ChessForm = new ChessForm(this);
        }

        public Chess ChessGame { get; }
        public ChessForm ChessForm { get; }


        private void SetupEvents()
        {
           
        }


        private async Task<ChessMove> GetPlayerMove()
        {
            _fromCoords = new Coords();
            _toCoords = new Coords();
            ChessMove move = new ChessMove();
            bool completeInput = false;
            while (!completeInput)
            {
                await _semaphoreClick.WaitAsync(cToken);
                if (_game.GetPieceAt(_clickedCoords) != null && _game.GetPieceAt(_clickedCoords).Owner.Equals(_game.CurrentPlayer))
                {
                    _fromCoords = _clickedCoords;
                    _toCoords = new Coords();
                }
                else if (!_fromCoords.Equals(new Coords()))
                {
                    _toCoords = _clickedCoords;
                }
                // Check if move is valid now

                if (!_toCoords.Equals(new Coords()) && !_fromCoords.Equals(new Coords()))
                {
                    move = new ChessMove(_fromCoords, _toCoords);
                    completeInput = true;
                }
            }
            return move;
        }
        private async void SendMove(object sender, RequestMoveEventArgs e)
        {
            Player player = (Player)sender;
            ChessMove move = await GetPlayerMove(e.CToken);
            player.OnMoveSent(move);
        }

        private void OnGameOver(object sender, GameOverEventArgs e)
        {
            MessageBox.Show(e.Result.ToString() + " ," + (e.Winner != null ? e.Winner.Colour.ToString() : "Nobody") + " Wins");
        }
    }
}

}
