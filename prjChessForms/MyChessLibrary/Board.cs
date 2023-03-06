using System;
using System.Collections.Generic;

namespace prjChessForms.MyChessLibrary
{
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
            _squares = new Square[COL_COUNT, ROW_COUNT];
            for (int y = 0; y < ROW_COUNT; y++)
            {
                for (int x = 0; x < COL_COUNT; x++)
                {
                    _squares[x, y] = new Square(x, y);
                }
            }
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
}