using prjChessForms.MyChessLibrary;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace prjChessForms.PresentationUI
{
    enum PromotionOption
    {
        Knight,
        Bishop,
        Rook,
        Queen
    }
    class PromotionSelectionPanel : Form
    {
        public EventHandler<PromotionEventArgs> PromotionSelected;

        private TableLayoutPanel _panel;
        private PromotionOption _selectedPromotion;
        public PromotionSelectionPanel(PieceColour colour)
        {
            PieceColour = colour;
            SetupPanel();
            SetupButtons();
        }
        public PieceColour PieceColour { get; }

        public PromotionOption SelectedPromotionOption
        {
            get { return _selectedPromotion; }
            set 
            {
                _selectedPromotion = value;
                if (PromotionSelected != null)
                {
                    PromotionSelected.Invoke(this, new PromotionEventArgs())
                }
            }
        }

        private void OnSelectionButtonClicked(PromotionOption)

        private void OnQueenSelectionClicked(object sender, EventArgs e) => SelectedPromotionOption = PromotionOption.Queen;

        private void OnBishopSelectionClicked(object sender, EventArgs e) => SelectedPromotionOption = PromotionOption.Bishop;

        private void OnKnightSelectionClicked(object sender, EventArgs e) => SelectedPromotionOption = PromotionOption.Knight;

        private void OnRookSelectionClicked(object sender, EventArgs e) => SelectedPromotionOption = PromotionOption.Rook;

        private void SetupPanel()
        {
            _panel = new TableLayoutPanel()
            {
                Parent = this,
                Dock = DockStyle.Fill,
            };
            _panel.RowStyles.Clear();
            _panel.ColumnStyles.Clear();

            _panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            _panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            _panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        }

        private void SetupButtons()
        {
            Button button = new Button() 
            {
                Parent = _panel,
                Image = (Image)Properties.Resources.ResourceManager.GetObject(PieceColour.ToString() + "_Queen"),
                Dock = DockStyle.Fill
            };
            _panel.SetCellPosition(button, new TableLayoutPanelCellPosition(0, 0));
            button.Click += OnQueenSelectionClicked;

            button = new Button()
            {
                Parent = _panel,
                Image = (Image)Properties.Resources.ResourceManager.GetObject(PieceColour.ToString() + "_Rook"),
                Dock = DockStyle.Fill
            };
            _panel.SetCellPosition(button, new TableLayoutPanelCellPosition(0, 1));
            button.Click += OnRookSelectionClicked;

            button = new Button()
            {
                Parent = _panel,
                Image = (Image)Properties.Resources.ResourceManager.GetObject(PieceColour.ToString() + "_Knight"),
                Dock = DockStyle.Fill
            };
            _panel.SetCellPosition(button, new TableLayoutPanelCellPosition(1, 0));
            button.Click += OnKnightSelectionClicked;

            button = new Button()
            {
                Parent = _panel,
                Image = (Image)Properties.Resources.ResourceManager.GetObject(PieceColour.ToString() + "_Bishop"),
                Dock = DockStyle.Fill
            };
            _panel.SetCellPosition(button, new TableLayoutPanelCellPosition(1, 1));
            button.Click += OnBishopSelectionClicked;
        }

    }
}
