using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Channels;
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

        public override string ToString()
        {
            return Convert.ToString(Convert.ToChar(Convert.ToInt32('a') + _x)) + Convert.ToString(_y + 1);
        }

    }
    class Board
    {
        public event EventHandler<SquareClickedEventArgs> RaiseSquareClicked;

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
            foreach(Piece p in GetPieces(colour))
            {
                if (p.GetType() == typeof(King))
                {
                    king = (King) p;
                    break;
                }
            }
            return king;
        }

        public List<Piece> GetPieces(PieceColour colour)
        {
            List<Piece> pieces = new List<Piece>();
            Piece p;
            for (int y = 0; y < ROW_COUNT; y++)
            {
                for (int x = 0; x < COL_COUNT; x++)
                {
                    p = _squares[x, y].PieceInSquare;
                    if (p != null && p.Colour == colour)
                    {
                        pieces.Add(p);
                    }
                }
            }
            return pieces;
        }

        public void TriggerSquareClicked(Square square)
        {
            OnSquareClickedEvent(new SquareClickedEventArgs(square));
        }
 
        private void OnSquareClickedEvent(SquareClickedEventArgs e)
        {
            EventHandler<SquareClickedEventArgs> raiseEvent = RaiseSquareClicked;
            if (raiseEvent != null)
            {
                raiseEvent(this, e);
            }
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
                    _squares[x, y] = new Square(this, _layoutPanel, x, y);
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
            PieceColour colour;
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
                        AddPiece(defaultPieces[y, x], colour, square);
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

    class SquareClickedEventArgs : EventArgs
    {
        private Square _square;
        public SquareClickedEventArgs(Square square)
        {
            _square = square;
        }

        public Square Square 
        { 
            get 
            { 
                return _square; 
            } 
        }
    }

    class Square
    {
        private Board _board;
        private Piece _piece;
        private Coords _coords;

        private Color _panelColour;
        private TableLayoutPanel _layoutPanel;
        private Button _panel;
        private PictureBox _pieceImage;

        public Square(Board board, TableLayoutPanel table, int x, int y)
        {
            _board = board;
            _layoutPanel = table;
            _coords = new Coords(x, y);
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
                if (value != null && _piece != null)
                {
                    // update original piece in current square to null
                    _piece.Square = null;
                }
                _piece = value;
                UpdateSquare();
            }
        }

        public Coords Coords 
        {
            get
            {
                return _coords;
            }
        }
        public Button Panel
        {
            get
            {
                return _panel;
            }
        }

        private void SetupSquare()
        {
            _panel = new Button()
            {
                Parent = _layoutPanel,
                BackColor = _panelColour,
                Dock = DockStyle.Fill
            };
            /*
            _pieceImage = new PictureBox()
            {
                Parent = _panel,
                Dock = DockStyle.Fill,
                Image = null
            };
            */
            _layoutPanel.SetCellPosition(_panel, new TableLayoutPanelCellPosition(_coords.X, _coords.Y));
            _panel.Click += OnPanelClick;
            UpdateSquare();
        }

        private void UpdateSquare()
        {
            _panel.BackColor = _panelColour;
            _panel.Image = _piece != null ? _piece.Image : null;
            /*
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
            */
        }

        private void OnPanelClick(object sender, EventArgs e)
        {
            _board.TriggerSquareClicked(this);
            Console.WriteLine(Coords.ToString());
        }
    }

}