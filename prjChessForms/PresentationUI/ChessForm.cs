using prjChessForms.Controller;
using prjChessForms.MyChessLibrary;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace prjChessForms.PresentationUI
{
    partial class ChessForm : Form, IModelObserver
    {
        public EventHandler<SquareClickedEventArgs> SquareClicked;

        private BoardTableLayoutPanel _boardPanel;
        private TableLayoutPanel _layoutPanel;
        public ChessForm()
        {
            InitializeComponent();
            SetupControls();
        }
        public ChessController Controller { get; set; }

        public void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e)
        {
            _boardPanel.UpdateSquare(e.Square.Coords, e.NewPiece != null ? e.NewPiece.Image : null);
        }

        public void OnPieceSelectionChanged(object sender, PieceSelectionChangedEventArgs e)
        {
            _boardPanel.ChangePieceSelection(e.SelectedPieceCoords, e.PossibleEndCoords);
        }

        private void SetupControls()
        {
            // layout
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


            // Board panel
            _boardPanel = new BoardTableLayoutPanel(this)
            {
                Parent = _layoutPanel,
                Dock = DockStyle.Fill,
            };
            _boardPanel.SquareClicked += OnBoardClicked;
            _layoutPanel.SetCellPosition(_boardPanel, new TableLayoutPanelCellPosition(0, 1));

            // Timer
            //_timer = new System.Timers.Timer(1000);
            //_timerLabels = new Label[2];


            //// White player label
            //TableLayoutPanel whiteTable = new TableLayoutPanel()
            //{
            //    Parent = _layoutPanel,
            //    Dock = DockStyle.Fill,
            //    ColumnStyles = { new ColumnStyle(SizeType.Percent, 50), new ColumnStyle(SizeType.Percent, 50) },
            //    RowStyles = { new RowStyle(SizeType.Percent, 100) },
            //};
            //_layoutPanel.SetCellPosition(whiteTable, new TableLayoutPanelCellPosition(0, 2));
            //Label whiteLabel = new Label()
            //{
            //    Parent = whiteTable,
            //    Dock = DockStyle.Fill,
            //    Text = _players[0].Colour.ToString(),
            //};
            //whiteTable.SetCellPosition(whiteLabel, new TableLayoutPanelCellPosition(0, 0));
            //_timerLabels[0] = new Label()
            //{
            //    Parent = whiteTable,
            //    Dock = DockStyle.Fill,
            //    Text = _players[0].RemainingTime.ToString(),
            //};
            //whiteTable.SetCellPosition(_timerLabels[0], new TableLayoutPanelCellPosition(1, 0));





            // Black player label
            //TableLayoutPanel blackTable = new TableLayoutPanel()
            //{
            //    Parent = _layoutPanel,
            //    Dock = DockStyle.Fill,
            //    ColumnStyles = { new ColumnStyle(SizeType.Percent, 50), new ColumnStyle(SizeType.Percent, 50) },
            //    RowStyles = { new RowStyle(SizeType.Percent, 100) },
            //};
            //_layoutPanel.SetCellPosition(blackTable, new TableLayoutPanelCellPosition(0, 0));
            //Label blackLabel = new Label()
            //{
            //    Parent = blackTable,
            //    Dock = DockStyle.Fill,
            //    Text = _players[1].Colour.ToString(),
            //};
            //blackTable.SetCellPosition(blackLabel, new TableLayoutPanelCellPosition(0, 0));

            //_timerLabels[1] = new Label()
            //{
            //    Parent = blackTable,
            //    Dock = DockStyle.Fill,
            //    Text = _players[1].RemainingTime.ToString(),
            //};
            //whiteTable.SetCellPosition(_timerLabels[1], new TableLayoutPanelCellPosition(1, 0));
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
