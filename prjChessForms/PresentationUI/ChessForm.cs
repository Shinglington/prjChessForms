using prjChessForms.MyChessLibrary;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjChessForms.PresentationUI
{
    public class SquareClickedEventArgs : EventArgs
    {
        public SquareClickedEventArgs(Coords clickedCoords)
        {
            ClickedCoords = clickedCoords;
        }

        public Coords ClickedCoords { get; set; }
    }
    public partial class ChessForm : Form
    {
        public event EventHandler<SquareClickedEventArgs> SquareClicked;

        private BoardTableLayoutPanel _boardPanel;
        private TableLayoutPanel _layoutPanel;

        private SemaphoreSlim _semaphoreClick = new SemaphoreSlim(0, 1);
        private CancellationToken cts = new CancellationToken();

        public ChessForm()
        {
            InitializeComponent();
            SetupControls();
        }
        public Controller Controller { get; set; }
        private async Task<ChessMove> GetPlayerMove()
        {
            _fromCoords = new Coords();
            _toCoords = new Coords();
            ChessMove move = new ChessMove();
            bool completeInput = false;
            while (!completeInput)
            {
                await _semaphoreClick.WaitAsync(cToken);
                if (_game.GetPieceAt(_clickedCoords) != null && _game.GetPieceAt(_clickedCoords).Owner.Equals(_game.CurrentPlayer))
                {
                    _fromCoords = _clickedCoords;
                    _toCoords = new Coords();
                }
                else if (!_fromCoords.Equals(new Coords()))
                {
                    _toCoords = _clickedCoords;
                }
                // Check if move is valid now

                if (!_toCoords.Equals(new Coords()) && !_fromCoords.Equals(new Coords()))
                {
                    move = new ChessMove(_fromCoords, _toCoords);
                    completeInput = true;
                }
            }
            return move;
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
            _boardPanel = new BoardTableLayoutPanel()
            {
                Parent = _layoutPanel,
                Dock = DockStyle.Fill,
            };
            _boardPanel.SquareClicked += OnSquareClicked;
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

        private void OnSquareClicked(object sender, SquareClickedEventArgs e)
        {
            _clickedCoords = e.ClickedCoords;
            _semaphoreClick.Release();
        }
    }
}
