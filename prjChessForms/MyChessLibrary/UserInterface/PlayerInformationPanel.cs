using prjChessForms.MyChessLibrary;
using System;
using System.Windows.Forms;

namespace prjChessForms.MyChessLibrary.UserInterface
{
    class PlayerInformationPanel : TableLayoutPanel
    {
        private Label _colourLabel;
        private Label _timeLabel;
        private CapturedPiecesPanel _capturedPieces;
        public PlayerInformationPanel(PieceColour colour)
        {
            PieceColour = colour;
            SetupPanel();
        }

        public PieceColour PieceColour { get; }

        public void UpdateTime(TimeSpan remainingTime)
        {
            string text = remainingTime.ToString();
            if (_timeLabel.InvokeRequired)
            {
                Action safeWrite = delegate { UpdateTime(remainingTime); };
                _timeLabel.Invoke(safeWrite);
            }
            else
            {
                _timeLabel.Text = text;
            }
        }

        public void UpdateCapturedPieces(IPiece capturedPiece)
        {
            _capturedPieces.AddCapturedPiece(capturedPiece);
        }

        private void SetupPanel()
        {
            RowStyles.Clear();
            ColumnStyles.Clear();

            RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            SetupCapturedPieces();
            SetupLabel();
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
            _capturedPieces = new CapturedPiecesPanel(PieceColour)
            {
                Parent = this,
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
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
    }
}
