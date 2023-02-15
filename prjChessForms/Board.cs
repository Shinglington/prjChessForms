using System;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;

namespace prjChessForms
{
    class Board
    {
        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private TableLayoutPanel _layoutPanel;
        private Player[] _players;
        private Square[,] _squares;
        public Board(TableLayoutPanel boardPanel, Player[] players)
        {
            _layoutPanel = boardPanel;
            _players = players;
            SetupBoard();
        }
        private void SetupBoard()
        {
            // Format TableLayoutPanel
            _layoutPanel.Padding = new Padding(0);
            _layoutPanel.Margin = new Padding(0);

            _layoutPanel.ColumnCount = COL_COUNT;
            _layoutPanel.ColumnStyles.Clear();
            _layoutPanel.RowCount = ROW_COUNT;
            _layoutPanel.ColumnStyles.Clear();

            for (int c = 0; c < COL_COUNT; c++)
            {
                _layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / COL_COUNT));
            }

            for (int r = 0; r < ROW_COUNT; r++)
            {
                _layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / ROW_COUNT));
            }

            // Add squares
            _squares = new Square[COL_COUNT, ROW_COUNT];
            for (int y = 0; y < ROW_COUNT; y++)
            {
                for (int x = 0; x < COL_COUNT; x++)
                {

                    _squares[x, y] = new Square(_layoutPanel, x, y);

                }
            }
        }
    }


    class Square
    {
        private Color _panelColour;
        private TableLayoutPanel _layoutPanel;
        private Panel _panel;
        private Piece _piece;
        private int _x;
        private int _y;
        public Square(TableLayoutPanel table, int x, int y)
        {
            _layoutPanel = table;
            _x = x;
            _y = y;
            _panelColour = (x + y) % 2 == 0 ? Color.SandyBrown : Color.LightGray;
            _piece = null;
            SetupSquare();
        }

        public Piece PieceInSquare
        {
            get
            {
                return _piece;
            }
            set
            {
                _piece = value;
            }
        }

        private void SetupSquare()
        {
            _panel = new Panel()
            {
                Parent = _layoutPanel,
                BackColor = _panelColour,
                Dock = DockStyle.Fill
            };
            _layoutPanel.SetCellPosition(_panel, new TableLayoutPanelCellPosition(_x, _y));
        }


        private void UpdateSquare()
        {
            _panel.BackColor = _panelColour;
        }
    }



}
