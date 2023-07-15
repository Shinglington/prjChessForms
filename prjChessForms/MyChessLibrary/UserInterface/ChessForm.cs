using System;
using System.Windows.Forms;

namespace prjChessForms.MyChessLibrary.UserInterface
{
    partial class ChessForm : Form, IChessInterface, IChessObserver, IBoardObserver
    {

        public event EventHandler<SquareClickedEventArgs> SquareClicked;
        public event EventHandler<PromotionSelectedEventArgs> PromotionSelected;

        private TableLayoutPanel _layoutPanel;
        private BoardTableLayoutPanel _boardPanel;
        private PromotionSelectionPanel _promotionPanel;

        private PlayerInformationPanel _whiteInfo;
        private PlayerInformationPanel _blackInfo;

        public ChessForm()
        {
            InitializeComponent();
            SetupControls();
        }



        public void OnPieceInSquareChanged(object sender, PieceChangedEventArgs e)
        {
            _boardPanel.UpdateSquare(e.Square.Coords, e.NewPiece);
        }

        public void OnCoordsSelectionChanged(object sender, CoordsSelectionChangedEventArgs e)
        {
            _boardPanel.UpdateHighlights(e.SelectedCoords);
        }

        public void OnPlayerCapturedPiecesChanged(object sender, PlayerCapturedPiecesChangedEventArgs e)
        {
            switch (e.Player.Colour)
            {
                case PieceColour.White:
                    _whiteInfo.UpdateCapturedPieces(e.CapturedPiece);
                    break;
                case PieceColour.Black:
                    _blackInfo.UpdateCapturedPieces(e.CapturedPiece);
                    break;
            }
        }

        public void OnPromotion(object sender, PromotionEventArgs e)
        {
            _promotionPanel = new PromotionSelectionPanel(e.PromotingColour);
            _promotionPanel.PromotionSelected += OnPromotionSelected;
            _promotionPanel.Show();
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
                Padding = new Padding(0)
            };
            _layoutPanel.ColumnStyles.Clear();
            _layoutPanel.RowStyles.Clear();

            _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90));
            _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));

            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
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

        private void OnPromotionSelected(object sender, PromotionSelectedEventArgs e)
        {
            if (PromotionSelected != null)
            {
                _promotionPanel.Hide();
                _promotionPanel = null;
                PromotionSelected.Invoke(this, e);
            }
        }

        public void OnCoordSelectionChanged(object sender, CoordsSelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
