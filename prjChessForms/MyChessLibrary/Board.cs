using System;
using System.Collections.Generic;
using System.Drawing;

namespace prjChessForms.MyChessLibrary
{

    public struct Coords
    {
        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }

        public override string ToString() => $"{(char)('a' + X)}{Y + 1}";

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
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
    class Board
    {
        private const int ROW_COUNT = 8;
        private const int COL_COUNT = 8;
        private Player[] _players;
        private Square[,] _squares;
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
                GetSquareAt(EndCoords).Piece = p;
                GetSquareAt(StartCoords).Piece = null;
                p.HasMoved = true;
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
            return (GetSquareAt(coords).Piece);
        }

        public Coords GetCoordsOfPiece(Piece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException();
            }

            foreach (Square s in GetSquares())
            {
                if (s.Piece == piece)
                {
                    return s.Coords;
                }
            }

            throw new Exception("Piece could not be located");
        }

        public Square[,] GetSquares()
        {
            return _squares;
        }

        public Square GetSquareAt(Coords coords)
        {
            return _squares[coords.X, coords.Y];
        }

        public void ClearHighlights()
        {
            foreach (Square s in GetSquares())
            {
                s.ResetPanelColour();
            }
        }

        public void HighlightAt(Coords coords, Color highlightColour)
        {
            GetSquareAt(coords).BackColor = highlightColour;
        }

        public void RemoveGhostPawns()
        {
            foreach (Square s in GetSquares())
            {
                if (s.GetGhostPawn() != null)
                {
                    s.Piece = null;
                }
            }
        }

        public bool CheckMoveInCheck(Player player, ChessMove move)
        {

            Coords start = move.StartCoords;
            Coords end = move.EndCoords;
            bool startPieceHasMoved = GetPieceAt(start).HasMoved;
            Piece originalEndPiece = GetPieceAt(end);

            MakeMove(move);
            bool SelfCheck = Rulebook.IsInCheck(this, player);
            MakeMove(new ChessMove(end, start));

            GetSquareAt(start).Piece.HasMoved = startPieceHasMoved;
            GetSquareAt(end).Piece = originalEndPiece;

            return SelfCheck;
        }

        private void SetupBoard()
        {
            // Format
            Dock = DockStyle.Fill;
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
            _squares = new Square[ColumnCount, RowCount];
            for (int y = 0; y < ROW_COUNT; y++)
            {
                for (int x = 0; x < COL_COUNT; x++)
                {
                    _squares[x, y] = new Square(this, x, y);
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
            Player player;
            Square square;
            for (int i = 0; i < 2; i++)
            {
                player = _players[i];
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < COL_COUNT; x++)
                    {
                        if (player.Colour == PieceColour.White)
                        {
                            square = GetSquareAt(new Coords(x, 1 - y));
                        }
                        else
                        {
                            square = GetSquareAt(new Coords(x, ROW_COUNT - 2 + y));
                        }
                        AddPiece(defaultPieces[y, x], player, square);
                    }
                }
            }
        }

        private void AddPiece(char pieceType, Player player, Square square)
        {
            Piece p = null;
            switch (pieceType)
            {
                case 'P':
                    p = new Pawn(player);
                    break;
                case 'N':
                    p = new Knight(player);
                    break;
                case 'B':
                    p = new Bishop(player);
                    break;
                case 'R':
                    p = new Rook(player);
                    break;
                case 'Q':
                    p = new Queen(player);
                    break;
                case 'K':
                    p = new King(player);
                    break;
                default:
                    throw new ArgumentException("Unrecognised pieceType");
            }
            square.Piece = p;
        }


    }
    class Square
    {
        private Piece _piece;
        public Square(int x, int y)
        {
            Coords = new Coords(x, y);
            Piece = null;
        }

        public Piece Piece
        {
            get
            {
                if (_piece != null && _piece.GetType() == typeof(GhostPawn))
                {
                    return null;
                }
                return _piece;
            }
            set
            {
                _piece = value;
            }
        }
        public Coords Coords { get; }
        public GhostPawn GetGhostPawn()
        {
            return (_piece != null && _piece.GetType() == typeof(GhostPawn)) ? (GhostPawn)_piece : null;
        }

    }

}