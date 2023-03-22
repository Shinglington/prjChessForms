using prjChessForms.Controller;
using prjChessForms.MyChessLibrary;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace prjChessForms.PresentationUI
{
    partial class ChessForm : Form, IModelObserver
    {
        public EventHandler<SquareClickedEventArgs> SquareClicked;

        private BoardTableLayoutPanel _boardPanel;
        private TableLayoutPanel _layoutPanel;

        private PlayerInformationPanel _whiteInfo;
        private PlayerInformationPanel _blackInfo;

        public ChessForm()
        {
            InitializeComponent();
            SetupControls();
        }
        public ChessController Controller { get; set; }

        public void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e)
        {
            _boardPanel.UpdateSquare(e.Square.Coords, e.NewPiece);
        }

        public void OnPieceSelectionChanged(object sender, PieceSelectionChangedEventArgs e)
        {
            _boardPanel.UpdateHighlights(e.SelectedPiece, e.SelectedPieceCoords, e.PossibleEndCoords);
        }

        public void OnPlayerCapturedPiecesChanged(object sender, PlayerCapturedPiecesChangedEventArgs e)
        {
            
        }

        public void OnGameOver(object sender, GameOverEventArgs e)
        {
            if (e.Winner == null)
            {
                MessageBox.Show("Draw", e.Result.ToString());
            }
            else
            {
                MessageBox.Show(e.Winner.Colour.ToString() + " Wins!", e.Result.ToString());
            }
        }

        public void OnPlayerTimerTick(object sender, PlayerTimerTickEventArgs e)
        {
            switch (e.CurrentPlayer.Colour) 
            {
                case PieceColour.White:
                    _whiteInfo.UpdateTime(e.PlayerRemainingTime);
                    break;
                case PieceColour.Black:
                    _blackInfo.UpdateTime(e.PlayerRemainingTime);
                    break;
            }

        }

        private void SetupControls()
        {
            SetupLayoutPanel();
            SetupBoard();
            SetupPlayerInfo();
        }

        private void SetupLayoutPanel()
        {
            _layoutPanel = new TableLayoutPanel()
            {
                Parent = this,
                Dock = DockStyle.Fill,
            };
            _layoutPanel.ColumnStyles.Clear();
            _layoutPanel.RowStyles.Clear();

            _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90));
            _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 5));
        }

        private void SetupBoard()
        {
            _boardPanel = new BoardTableLayoutPanel(this)
            {
                Parent = _layoutPanel,
                Dock = DockStyle.Fill,
            };
            _boardPanel.SquareClicked += OnBoardClicked;
            _layoutPanel.SetCellPosition(_boardPanel, new TableLayoutPanelCellPosition(0, 1));
        }

        private void SetupPlayerInfo()
        {
            _whiteInfo = new PlayerInformationPanel(PieceColour.White)
            {
                Parent = _layoutPanel,
                Dock = DockStyle.Fill,
            };
            _layoutPanel.SetCellPosition(_whiteInfo, new TableLayoutPanelCellPosition(0, 2));
            _blackInfo = new PlayerInformationPanel(PieceColour.Black)
            {
                Parent = _layoutPanel,
                Dock = DockStyle.Fill,
            };
            _layoutPanel.SetCellPosition(_blackInfo, new TableLayoutPanelCellPosition(0, 0));
        }

        private void OnBoardClicked(object sender, SquareClickedEventArgs e)
        {
            if (SquareClicked != null)
            {
                SquareClicked.Invoke(this, e);
            }
        }

    }



}
