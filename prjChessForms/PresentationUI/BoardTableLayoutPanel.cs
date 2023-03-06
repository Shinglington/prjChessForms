﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using prjChessForms.MyChessLibrary;

namespace prjChessForms.PresentationUI
{
    class SquareClickedEventArgs : EventArgs 
    { 
        public SquareClickedEventArgs(Coords clickedCoords) 
        {
            ClickedCoords = clickedCoords;
        }

        public Coords ClickedCoords { get; set; }
    }

    class BoardTableLayoutPanel : TableLayoutPanel
    {
        public EventHandler<SquareClickedEventArgs> SquareClicked;

        private Square[,] _boardSquares;
        private Button[,] _buttons;

        private PieceColour _perspective;
        public BoardTableLayoutPanel(Square[,] boardSquares)
        {
            _perspective = PieceColour.White;
            _boardSquares = boardSquares;
            ColumnCount = _boardSquares.GetLength(0);
            RowCount = _boardSquares.GetLength(1);

            SetupRowsAndColumns();
            Display();
        }

        public void Display(PieceColour perspective = PieceColour.White)
        {
            perspective = PieceColour.White;
            for(int x = 0; x < ColumnCount; x++)
            {
                for(int y = 0; y < RowCount; y++)
                {
                    SetCellPosition(_buttons[x, y], new TableLayoutPanelCellPosition(x, RowCount - 1 - y));
                }
            }
        }

        private void SetupRowsAndColumns()
        {
            RowStyles.Clear();
            ColumnStyles.Clear();

            for(int x = 0; x < ColumnCount; x++)
            {
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / ColumnCount));
                for (int y = 0; y < RowCount; y++)
                {
                    RowStyles.Add(new RowStyle(SizeType.Percent, 100 / RowCount));

                    Button button = new Button()
                    {
                        Parent = this,
                        Dock = DockStyle.Fill

                    };
                    button.Click += OnSquareClicked;
                    _buttons[x, y] = button;
                    _boardSquares[x, y].PieceChanged += OnPieceInSquareChanged;
                    
                }
            }
        }

        private void OnSquareClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            TableLayoutPanelCellPosition position = GetPositionFromControl(button);
            Coords clickedCoords = new Coords();
            switch (_perspective) 
            {
                case PieceColour.White:
                    clickedCoords = new Coords(position.Column, RowCount - 1 - position.Row);
                    break;
                case PieceColour.Black:
                    clickedCoords = new Coords(position.Column, position.Row);
                    break;
            }
            SquareClicked.Invoke(this, new SquareClickedEventArgs(clickedCoords));
        }
        private void OnPieceInSquareChanged(object sender, EventArgs e)
        {
            Square square = (Square) sender;
            Button button = _buttons[square.Coords.X, square.Coords.Y];
            button.Image = square.Piece.Image;
        }



    }
}