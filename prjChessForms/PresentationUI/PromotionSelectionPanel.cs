using prjChessForms.MyChessLibrary;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace prjChessForms.PresentationUI
{
    class PromotionSelectionPanel : Form
    {
        public EventHandler<PromotionSelectedEventArgs> PromotionSelected;

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
                    PromotionSelected.Invoke(this, new PromotionSelectedEventArgs(_selectedPromotion));
                }
            }
        }

        private void OnSelectionButtonClicked(PromotionOption option) => SelectedPromotionOption = option;

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
            Button button;
            int i = 0;
            foreach(PromotionOption option in Enum.GetValues(typeof(PromotionOption)))
            {
                button = new Button()
                {
                    Parent = _panel,
                    Image = (Image)Properties.Resources.ResourceManager.GetObject(PieceColour.ToString() + "_" + option.ToString()),
                    Dock = DockStyle.Fill
                };
                _panel.SetCellPosition(button, new TableLayoutPanelCellPosition(i/2, i%2));
                button.Click += (sender, e) => OnSelectionButtonClicked(option);
                i++;
            }
        }
    }
}
