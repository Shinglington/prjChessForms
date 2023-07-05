using prjChessForms.Controller;
using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;
using System;
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

            IBoard board;
            IPiecePlacer piecePlacer = new PiecePlacer(board);
            IStartingPositionSetup startingPositionSetup = new StartingPositionSetup(board, piecePlacer);
            IBoardCreator boardCreator = new BoardCreator(board, startingPositionSetup, piecePlacer);
            ISquareProvider squareProvider = new SquareProvider(board);
            IMoveMaker moveMaker = new MoveMaker(board);
            board = new Board(boardCreator, squareProvider);
                

            Chess game = new Chess(board);
            ChessForm form = new ChessForm();
            ChessController controller = new ChessController(game, form);
            Application.Run(form);
        }
    }
}
