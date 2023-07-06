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
            IPiecePlacer piecePlacer = new PiecePlacer();
            IStartingPositionSetup startingPositionSetup = new StartingPositionSetup(piecePlacer);
            IBoardCreator boardCreator = new BoardCreator(startingPositionSetup, piecePlacer);
            ISquareProvider squareProvider = new SquareProvider();
            IMoveMaker moveMaker = new MoveMaker();
            IPieceProvider pieceProvider = new PieceProvider();
            board = new Board(boardCreator, squareProvider, pieceProvider, moveMaker);
                

            Chess game = new Chess(board);
            ChessForm form = new ChessForm();
            ChessController controller = new ChessController(game, form);
            Application.Run(form);
        }
    }
}
