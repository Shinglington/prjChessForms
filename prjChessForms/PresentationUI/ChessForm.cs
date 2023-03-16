﻿using prjChessForms.MyChessLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjChessForms.PresentationUI
{
    class SquareClickedEventArgs : EventArgs
    {
        public SquareClickedEventArgs(Coords clickedCoords)
        {
            ClickedCoords = clickedCoords;
        }
        public Coords ClickedCoords { get; set; }
    }

    class ImageInSquareUpdateEventArgs : EventArgs
    {
        public ImageInSquareUpdateEventArgs(Coords coords, Image image)
        {
            SquareCoords = coords;
            Image = image;
        }
        public Coords SquareCoords { get; set; }
        public Image Image { get; set; }
    }

    class SquareHighlightsChangedEventArgs : EventArgs
    {
        public SquareHighlightsChangedEventArgs(Coords selectedCoords, List<Coords> validMoves)
        {
            SelectedCoords = selectedCoords;
            ValidMoves = validMoves;
        }
        public Coords SelectedCoords { get; set; }
        public List<Coords> ValidMoves { get; set; }
    }
    partial class ChessForm : Form
    {
        public EventHandler<SquareClickedEventArgs> SquareClicked;

        private BoardTableLayoutPanel _boardPanel;
        private TableLayoutPanel _layoutPanel;
        public ChessForm(Controller controller)
        {
            InitializeComponent();
            Controller = controller;
            SetupControls();
            SetupEvents();
        }
        public Controller Controller { get; }

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
            _boardPanel.SquareClicked += (sender, e) => SquareClicked.Invoke(this, e);
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

        private void SetupEvents()
        {
            Controller.ImageInSquareUpdate += (sender, e) => _boardPanel.UpdateImageInSquare(e.SquareCoords, e.Image);
            Controller.SquareHighlightsChanged += (sender, e) => _boardPanel.ChangePieceSelection(e.SelectedCoords, e.ValidMoves);
        }
    }

   
    
}
