using System;
using System.Drawing;
using System.Net.NetworkInformation;
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

        private King[] _kings;
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

            // Add pieces
            AddDefaultPieces();
        }

        private void AddDefaultPieces()
        {
            char[,] defaultPieces =
           {
                { 'P','P','P','P','P','P','P','P'},
                { 'R','N','B','Q','K','B','N','R'}
            };

            // Pieces
            _kings = new King[2];
            PieceColour colour;
            for (int i = 0; i < 2; i++)
            {
                colour = (PieceColour)i;
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < COL_COUNT; x++)
                    {
                        if (colour == PieceColour.White)
                        {
                            AddPiece(defaultPieces[y, x], colour, _squares[x, 1 - y]);
                        }
                        else
                        {
                            AddPiece(defaultPieces[y, x], colour, _squares[x, ROW_COUNT - 2 + y]);
                        }
                    }
                }
            }
        }

        private void AddPiece(char pieceType, PieceColour colour, Square square)
        {
            Piece p = null;
            switch (pieceType) 
            {
                case 'P':
                    p = new Pawn(colour, square);
                    break;
                case 'N':
                    p = new Knight(colour, square);
                    break;
                case 'B':
                    p = new Bishop(colour, square);
                    break;
                case 'R':
                    p = new Rook(colour, square);   
                    break;
                case 'Q':
                    p = new Queen(colour, square);
                    break;
                case 'K':
                    p = new King(colour, square);
                    break;
                default:
                    throw new ArgumentException("Unrecognised pieceType");
            }
            p.Square = square;
        }
    }


    class Square
    {
        private Color _panelColour;
        private Piece _piece;
        private int _x;
        private int _y;

        private TableLayoutPanel _layoutPanel;
        private Panel _panel;
        private PictureBox _pieceImage;
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

        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
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
            _pieceImage = new PictureBox()
            {
                Parent = _panel,
                Dock = DockStyle.Fill,
                Image = null
            };
            _layoutPanel.SetCellPosition(_panel, new TableLayoutPanelCellPosition(_x, _y));
            UpdateSquare();
        }


        private void UpdateSquare()
        {
            _panel.BackColor = _panelColour;
            if (_piece == null)
            {
                _pieceImage.Image = null;
            }
            else
            {
                _pieceImage.Image = _piece.Image;
                Padding p = new Padding();
                p.Left = (_pieceImage.Width - _pieceImage.Image.Width) / 2;
                p.Top = (_pieceImage.Height - _pieceImage.Image.Height) / 2;
                _pieceImage.Padding = p;
            }
        }
    }
}