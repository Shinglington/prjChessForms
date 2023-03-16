using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjChessForms
{
    class CoordsClickedEventArgs : EventArgs
    {
        public CoordsClickedEventArgs(Coords coords)
        {
            ClickedCoords = coords;
        }
        public Coords ClickedCoords { get; set; }
    }  
    class Controller
    {
        public EventHandler<CoordsClickedEventArgs> CoordsClicked;
        public EventHandler<ImageInSquareUpdateEventArgs> ImageInSquareUpdate;
        public EventHandler<SquareHighlightsChangedEventArgs> SquareHighlightsChanged;

        private CancellationToken cts = new CancellationToken();
        public Controller()
        {
            ChessGame = new Chess(this);
            ChessForm = new ChessForm(this);

            SetupEvents();
            Start();
        }

        public Chess ChessGame { get; }
        public ChessForm ChessForm { get; }

        public void Start()
        {
            ChessGame.SyncGameAndBoard();
            ChessGame.StartGame();
        }

        private void SetupEvents()
        {
            ChessForm.SquareClicked += (sender, e) => CoordsClicked.Invoke(this, new CoordsClickedEventArgs(e.ClickedCoords));
            ChessGame.PieceInSquareChanged += (sender, e) => ImageInSquareUpdate.Invoke(this, new ImageInSquareUpdateEventArgs(e.SquareCoords, e.NewPiece != null ? e.NewPiece.Image : null));
            ChessGame.PieceSelectionChanged += OnPieceSelectionChanged;
        }

        private void OnPieceSelectionChanged(object sender, PieceSelectionChangedEventArgs e)
        {
            List<Coords> highlightCoords = new List<Coords>();
            foreach(ChessMove move in e.ValidMoves)
            {
                highlightCoords.Add(move.EndCoords);
            }
            SquareHighlightsChangedEventArgs eventArgs = new SquareHighlightsChangedEventArgs(ChessGame.GetCoordsOf(e.SelectedPiece), highlightCoords);
            SquareHighlightsChanged.Invoke(this, eventArgs);
        }

        private void OnGameOver(object sender, GameOverEventArgs e)
        {
            MessageBox.Show(e.Result.ToString() + " ," + (e.Winner != null ? e.Winner.Colour.ToString() : "Nobody") + " Wins");
        }
    }
}

