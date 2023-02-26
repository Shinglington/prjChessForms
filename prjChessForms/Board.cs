﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace prjChessForms
{

    public struct Coords
    {
        private int _x;
        private int _y;

        public Coords(int x, int y)
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

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Coords))
            {
                return false;
            }
            else
            {
                Coords other = (Coords)obj;
                return other.X == X && other.Y == Y;
            }
        }

        public override int GetHashCode()
        {
            int hashCode = 367829482;
            hashCode = hashCode * -1521134295 + _x.GetHashCode();
            hashCode = hashCode * -1521134295 + _y.GetHashCode();
            return hashCode;
        }
    }
    class Board : TableLayoutPanel
    {
        public event EventHandler<EventArgs> RaiseSquareClicked;

        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private Player[] _players;
        public Board(Player[] players)
        {
            _players = players;
            SetupBoard();
        }

        public void MakeMove(ChessMove Move)
        {
            Coords StartCoords = Move.StartCoords;
            Coords EndCoords = Move.EndCoords;
            Piece p = GetPieceAt(StartCoords);
            if (p != null)
            {
                p.Square = GetSquareAt(EndCoords);
            }
        }

        public King GetKing(PieceColour colour)
        {
            King king = null;
            foreach (Piece p in GetPieces(colour))
            {
                if (p.GetType() == typeof(King))
                {
                    king = (King)p;
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
                    p = GetPieceAt(new Coords(x, y));
                    if (p != null && p.Colour == colour)
                    {
                        pieces.Add(p);
                    }
                }
            }
            return pieces;
        }


        public Piece GetPieceAt(Coords coords)
        {
            return (GetSquareAt(coords).PieceInSquare);
        }

        public Square[,] GetSquares()
        {
            Square[,] squares = new Square[ColumnCount, RowCount];
            for (int col = 0; col < ColumnCount; col++)
            {
                for (int row = 0; row < RowCount; row++)
                {
                    squares[col, row] = (Square)GetControlFromPosition(col, row);
                }
            }


            return squares;
        }

        public Square GetSquareAt(Coords coords)
        {
            return (Square) GetControlFromPosition(coords.X, coords.Y);
        }

        public void TriggerSquareClicked(Square square)
        {
            OnSquareClickedEvent(square, new EventArgs());
        }

        private void OnSquareClickedEvent(object sender, EventArgs e)
        {
            EventHandler<EventArgs> raiseEvent = RaiseSquareClicked;
            if (raiseEvent != null && sender is Square)
            {
                raiseEvent(sender, e);
            }
        }
        private void SetupBoard()
        {
            // Format
            Padding = new Padding(0);
            Margin = new Padding(0);

            ColumnCount = COL_COUNT;
            ColumnStyles.Clear();
            RowCount = ROW_COUNT;
            ColumnStyles.Clear();

            for (int c = 0; c < COL_COUNT; c++)
            {
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / COL_COUNT));
            }

            for (int r = 0; r < ROW_COUNT; r++)
            {
                RowStyles.Add(new RowStyle(SizeType.Percent, 100 / ROW_COUNT));
            }

            // Add squares
            Square s;
            for (int y = 0; y < ROW_COUNT; y++)
            {
                for (int x = 0; x < COL_COUNT; x++)
                {
                    s = new Square(this, x, y);
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
                            square = GetSquareAt(new Coords(x, y));
                        }
                        else
                        {
                            square = GetSquareAt(new Coords(x, ROW_COUNT - 2 + y));
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
    class Square : Button
    {
        private Board _board;
        private Piece _piece;
        private Coords _coords;

        private Color _panelColour;
        private TableLayoutPanel _layoutPanel;

        public Square(Board board, int x, int y)
        {
            _board = board;
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
        private void SetupSquare()
        {
            BackColor = _panelColour;
            Dock = DockStyle.Fill;
            Click += OnPanelClick;
            UpdateSquare();
        }

        private void UpdateSquare()
        {
            BackColor = _panelColour;
            Image = _piece != null ? _piece.Image : null;
        }

        private void OnPanelClick(object sender, EventArgs e)
        {
            _board.TriggerSquareClicked(this);
        }
    }

}