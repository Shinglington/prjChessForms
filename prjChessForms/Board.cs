using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace prjChessForms
{

    public struct Coords 
    {
        private int _x;
        private int _y;

        public Coords(int x, int  y)
        {
            _x = x;
            _y = y;
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
    }
    class Board
    {
        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private TableLayoutPanel _layoutPanel;
        private Player[] _players;
        private Square[,] _squares;
        private Dictionary<PieceColour, List<Piece>> _pieces;
        public Board(TableLayoutPanel boardPanel, Player[] players)
        {
            _layoutPanel = boardPanel;
            _players = players;
            SetupBoard();
        }
        public void MakeMove((int, int) StartCoords, (int, int) EndCoords)
        {
            var (startX, startY) = StartCoords;
            var (endX, endY) = EndCoords;

            Piece p = _squares[startX, startY].PieceInSquare;
            if (p != null)
            {
                p.Square = _squares[endX, endY];
            }
        }

        public King GetKing(PieceColour colour)
        {
            King king = null;
            foreach(Piece p in _pieces[colour])
            {
                if (p.GetType() == typeof(King))
                {
                    king = (King) p;
                    break;
                }
            }
            return king;
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
            _pieces.Add(PieceColour.White, new List<Piece>());
            _pieces.Add(PieceColour.Black, new List<Piece>()); 

            // Pieces
            PieceColour colour;
            Piece piece;
            Square square;
            for (int i = 0; i < 2; i++)
            {
                colour = (PieceColour)i;
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < COL_COUNT; x++)
                    {
                        if (colour == PieceColour.White)
                        {
                            square = _squares[x, 1 - y];
                        }
                        else
                        {
                            square = _squares[x, ROW_COUNT - 2 + y];
                        }
                        piece = MakePiece(defaultPieces[y, x], colour, square);
                        _pieces[colour].Add(piece);
                    }
                }
            }
        }

        private Piece MakePiece(char pieceType, PieceColour colour, Square square)
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
            return p;
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
                if (value == null && _piece != null)
                {
                    _piece.Square = null;
                }
                _piece = value;
                UpdateSquare();
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
            if (_piece != null && _piece.Image != null)
            {
                _pieceImage.Image = _piece.Image;
                Padding p = new Padding();
                p.Left = (_pieceImage.Width - _pieceImage.Image.Width) / 2;
                p.Top = (_pieceImage.Height - _pieceImage.Image.Height) / 2;
                _pieceImage.Padding = p;
            }
            else
            {
                _pieceImage.Image = null;
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
             
        }
    }
}