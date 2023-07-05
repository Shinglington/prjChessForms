using prjChessForms.MyChessLibrary;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace prjChessForms.PresentationUI
{
    internal class CapturedPiecesPanel : TableLayoutPanel
    {
        private Dictionary<string, int> _capturedPieceCounts;
        public CapturedPiecesPanel(PieceColour colour)
        {
            CapturedPieceColour = colour == PieceColour.White ? PieceColour.Black : PieceColour.White;
            _capturedPieceCounts = new Dictionary<string, int>()
            {
                {"Pawn", 0},
                {"Bishop", 0},
                {"Knight", 0 },
                {"Rook", 0},
                {"Queen", 0}
            };

            RowStyles.Clear();
            ColumnStyles.Clear();
            RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            for (int i = 0; i < 5; i++)
            {
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            }
        }

        public PieceColour CapturedPieceColour { get; }

        public void AddCapturedPiece(Piece piece)
        {
            _capturedPieceCounts[piece.Name]++;
            UpdateDisplayPanel();
        }

        private void UpdateDisplayPanel()
        {
            Controls.Clear();
            int tableIndex = 0;
            for (int i = 0; i < 5; i++)
            {
                KeyValuePair<string, int> keyValuePair = _capturedPieceCounts.ElementAt(i);
                if (keyValuePair.Value > 0)
                {
                    Control p = CreatePieceCaptureCounter(keyValuePair.Key);
                    SetCellPosition(p, new TableLayoutPanelCellPosition(tableIndex, 0));
                    tableIndex++;
                }
            }
        }

        private Control CreatePieceCaptureCounter(string pieceName)
        {
            int count = _capturedPieceCounts[pieceName];
            Label p = new Label()
            {
                Parent = this,
                Dock = DockStyle.Fill,
                Image = (Image)Properties.Resources.ResourceManager.GetObject(CapturedPieceColour.ToString() + "_" + pieceName),
                Text = "x" + count.ToString()
            };
            return p;
        }
    }
}
