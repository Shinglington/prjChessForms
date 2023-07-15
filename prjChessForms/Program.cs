using prjChessForms.Controller;
using prjChessForms.MyChessLibrary;
using prjChessForms.MyChessLibrary.UserInterface;
using System;
using System.Collections.Generic;
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


            ChessForm form = (ChessForm)CreateInterface();
            IBoard board = CreateBoard((IBoardObserver)form);
            IChess chess = CreateChessGame(board, (IChessObserver)form);

            Application.Run(form);
        }


        private static IChessInterface CreateInterface()
        {
            IChessInterface chessInterface = new ChessForm();

            return chessInterface;
        }

        private static IBoard CreateBoard(IBoardObserver boardObserver)
        {
            IPiecePlacer piecePlacer = new PiecePlacer();
            IStartingPositionSetup startingPositionSetup = new StartingPositionSetup(piecePlacer);
            IBoardCreator boardCreator = new BoardCreator(startingPositionSetup, boardObserver);
            ISquareProvider squareProvider = new SquareProvider();
            IMoveMaker moveMaker = new MoveMaker();
            IPieceProvider pieceProvider = new PieceProvider();
            IBoard board = new Board(boardCreator, squareProvider, pieceProvider, moveMaker, boardObserver);
            return board;

        }

        private static IChess CreateChessGame(IBoard board, IChessObserver chessObserver)
        {
            List<IRulebook> subRulebooks = new List<IRulebook>()
            {
                new NormalMovesRulebook(board),
                new CastlingRulebook(board),
                new EnPassantRulebook(board)
            };
            IRulebook fullRulebook = new FullRulebook(board, subRulebooks);

            IPlayerHandler playerHandler = new PlayerHandler();
            ITimeManager timeManager = new TimeManager(playerHandler, 1000);
            ICoordSelectionHandler coordSelectionHandler = new CoordsSelectionHandler();
            IMoveHandler moveInputHandler = new MoveHandler(fullRulebook, coordSelectionHandler);
            IChessEventManager chessEventManager = new ChessEventManager();
            IGameFinishedChecker gameFinishedChecker = new GameFinishedChecker(board, playerHandler, fullRulebook, timeManager);
            IChessInputController chessInputController = new ChessInputController();
            IPromotionHandler promotionHandler = new PromotionHandler();
            IMoveHandler moveHandler = new MoveHandler(fullRulebook, coordSelectionHandler);

            IGameHandler gameHandler = new GameHandler(
                board, chessEventManager, chessInputController,
                playerHandler, timeManager, gameFinishedChecker,
                coordSelectionHandler, promotionHandler, moveHandler);


            IChess chess = new Chess(gameHandler, chessObserver);
            return chess;
        }
    }
}
