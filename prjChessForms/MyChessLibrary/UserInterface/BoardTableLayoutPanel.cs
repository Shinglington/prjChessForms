using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace prjChessForms.MyChessLibrary.UserInterface
{
    class BoardTableLayoutPanel : TableLayoutPanel
    {
        public EventHandler<SquareClickedEventArgs> SquareClicked;

        private const int ROWCOUNT = 8;
        private const int COLUMNCOUNT = 8;
        private Button[,] _buttons;
        public BoardTableLayoutPanel(ChessForm parentForm)
        {
            ParentForm = parentForm;
            ColumnCount = COLUMNCOUNT;
            RowCount = ROWCOUNT;
            SetupRowsAndColumns();
            Display();
        }
        public ChessForm ParentForm { get; }

        public void Display(bool flippedPerspective = false)
        {
            for (int x = 0; x < ColumnCount; x++)
            {
                for (int y = 0; y < RowCount; y++)
                {
                    if (flippedPerspective)
                    {
                        SetCellPosition(_buttons[x, y], new TableLayoutPanelCellPosition(x, y));
                    }
                    else
                    {
                        SetCellPosition(_buttons[x, y], new TableLayoutPanelCellPosition(x, RowCount - 1 - y));
                    }
                }
            }
        }

        public Coords GetCoordsOf(Button button)
        {
            for (int y = 0; y < _buttons.GetLength(1); y++)
            {
                for (int x = 0; x < _buttons.GetLength(0); x++)
                {
                    if (_buttons[x, y] == button)
                    {
                        return new Coords(x, y);
                    }
                }
            }
            throw new ArgumentException("Button could not be found");
        }

        public void UpdateSquares(ISquare[,] squares, Piece selectedPiece, List<Coords> possibleMoves)
        {
            for (int y = 0; y < squares.GetLength(1); y++)
            {
                for (int x = 0; x < squares.GetLength(0); x++)
                {
                    IPiece piece = squares[x, y].Piece;
                    Button button = _buttons[x, y];
                    if (piece != null)
                    {
                        button.Image = piece.Image;
                        if (piece.Equals(selectedPiece))
                        {
                            button.BackColor = Color.Blue;
                        }
                        else if (possibleMoves.Contains(new Coords(x, y)))
                        {
                            button.BackColor = Color.Green;
                        }
                        else
                        {
                            button.BackColor = DefaultBackColor;
                        }
                    }
                    else
                    {
                        button.Image = null;
                    }
                }
            }
        }

        public void UpdateHighlights(Coords selectedCoords)
        {
            for (int y = 0; y < _buttons.GetLength(1); y++)
            {
                for (int x = 0; x < _buttons.GetLength(0); x++)
                {
                    Button button = _buttons[x, y];
                    if (selectedCoords.Equals(new Coords(x, y)))
                    {
                        button.BackColor = Color.Blue;
                    }
                }
            }
        }

        public void UpdateSquare(Coords coords, IPiece piece)
        {
            _buttons[coords.X, coords.Y].Image = piece != null ? piece.Image : null;
        }

        private void SetupRowsAndColumns()
        {
            RowStyles.Clear();
            ColumnStyles.Clear();
            _buttons = new Button[ColumnCount, RowCount];
            for (int x = 0; x < ColumnCount; x++)
            {
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / ColumnCount));
                for (int y = 0; y < RowCount; y++)
                {
                    RowStyles.Add(new RowStyle(SizeType.Percent, 100 / RowCount));

                    Button button = new Button()
                    {
                        Parent = this,
                        Dock = DockStyle.Fill,
                    };
                    button.Click += OnSquareClicked;
                    _buttons[x, y] = button;
                }
            }
        }

        private void OnSquareClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            SquareClicked.Invoke(this, new SquareClickedEventArgs(GetCoordsOf(button)));
        }
    }
}
