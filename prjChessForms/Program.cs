using prjChessForms.Controller;
using prjChessForms.MyChessLibrary;
using prjChessForms.PresentationUI;
using System;
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

            Chess game = new Chess();
            ChessForm form = new ChessForm();
            ChessController controller = new ChessController(game, form);
            Application.Run(form);
        }
    }
}
