using prjChessForms.Controller;
using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace prjChessForms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IPiecePlacer piecePlacer = new PiecePlacer();
            IStartingPositionSetup startingPositionSetup = new StartingPositionSetup(piecePlacer);
            IBoardCreator boardCreator = new BoardCreator(startingPositionSetup, piecePlacer);
            ISquareProvider squareProvider = new SquareProvider();
            IMoveMaker moveMaker = new MoveMaker();
            IPieceProvider pieceProvider = new PieceProvider();
            IBoard board = new Board(boardCreator, squareProvider, pieceProvider, moveMaker);

            List<IRulebook> subRulebooks = new List<IRulebook>()
            {
                new NormalMovesRulebook(board),
                new CastlingRulebook(board),
                new EnPassantRulebook(board)
            };
            IRulebook fullRulebook = new FullRulebook(board, subRulebooks);

            IPlayerHandler playerHandler = new PlayerHandler();
            IMoveHandler moveInputHandler = new MoveHandler(fullRulebook);

            IChess chess = new Chess(board, playerHandler, moveInputHandler);


            






            OldChess game = new OldChess(board);
            ChessForm form = new ChessForm();
            ChessInputController controller = new ChessInputController(game, form);
            Application.Run(form);
        }
    }
}
