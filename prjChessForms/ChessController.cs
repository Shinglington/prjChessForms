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
    class ChessController
    {
        public EventHandler<CoordsClickedEventArgs> CoordsClicked;
        public EventHandler<ImageInSquareUpdateEventArgs> ImageInSquareUpdate;
        public EventHandler<SquareHighlightsChangedEventArgs> SquareHighlightsChanged;

        private CancellationToken cts = new CancellationToken();

        private Chess _chessModel;
        private ChessForm _chessView;

        public ChessController(Chess chessModel, ChessForm chessView)
        {
            _chessModel = chessModel;
            _chessView = chessView;
            _chessView.Controller = this;

            SetupEvents();
            Start();
        }


        public void Start()
        {
            _chessModel.SyncGameAndBoard();
            _chessModel.StartGame();
        }

        private void SetupEvents()
        {
            _chessView.SquareClicked += (sender, e) => CoordsClicked.Invoke(this, new CoordsClickedEventArgs(e.ClickedCoords));
            _chessModel.PieceChanged += (sender, e) => ImageInSquareUpdate.Invoke(this, new ImageInSquareUpdateEventArgs(e.SquareCoords, e.NewPiece != null ? e.NewPiece.Image : null));
            _chessModel.PieceSelectionChanged += OnPieceSelectionChanged;
        }

        private void OnPieceSelectionChanged(object sender, PieceSelectionChangedEventArgs e)
        {
            List<Coords> highlightCoords = new List<Coords>();
            foreach(ChessMove move in e.ValidMoves)
            {
                highlightCoords.Add(move.EndCoords);
            }
            SquareHighlightsChangedEventArgs eventArgs = new SquareHighlightsChangedEventArgs(_chessModel.GetCoordsOf(e.SelectedPiece), highlightCoords);
            SquareHighlightsChanged.Invoke(this, eventArgs);
        }

        private void OnGameOver(object sender, GameOverEventArgs e)
        {
            MessageBox.Show(e.Result.ToString() + " ," + (e.Winner != null ? e.Winner.Colour.ToString() : "Nobody") + " Wins");
        }
    }
}

