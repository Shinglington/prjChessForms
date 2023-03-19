using prjChessForms.MyChessLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace prjChessForms.PresentationUI
{
    class PlayerInformationPanel : TableLayoutPanel
    {
        private Label _colourLabel;
        private Label _timeLabel;
        private FlowLayoutPanel _capturedPieces;
        public PlayerInformationPanel(PieceColour colour)
        {
            PieceColour = colour;
            SetupPanel();
        }

        public PieceColour PieceColour { get; }

        public void UpdateInfo(TimeSpan remainingTime, List<Piece> capturedPieces)
        {
            _timeLabel.Text = remainingTime.ToString();
            UpdateCapturedPieces(capturedPieces);
        }



        private void SetupPanel()
        {
            RowStyles.Clear();
            ColumnStyles.Clear();
            
            RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            SetupLabel();
            SetupCapturedPieces();
            SetupTimer();

        }

        private void SetupLabel()
        {
            _colourLabel = new Label 
            {
                Text = PieceColour.ToString(),
                Parent = this,
                Dock = DockStyle.Fill,
            };
            SetCellPosition(_colourLabel, new TableLayoutPanelCellPosition(0, 0));
        }

        private void SetupCapturedPieces()
        {
            _capturedPieces = new FlowLayoutPanel() 
            {
                FlowDirection = FlowDirection.LeftToRight,
                Parent = this,
                Dock = DockStyle.Fill,
            };
            SetCellPosition(_capturedPieces, new TableLayoutPanelCellPosition(1, 0));

        }

        private void SetupTimer()
        {
            _timeLabel = new Label()
            {
                Text = TimeSpan.Zero.ToString(),
                Parent = this,
                Dock = DockStyle.Fill,
            };
            SetCellPosition(_timeLabel, new TableLayoutPanelCellPosition(2, 0));
        }


        private void UpdateCapturedPieces(List<Piece> capturedPiece)
        {

        }




    }
}
