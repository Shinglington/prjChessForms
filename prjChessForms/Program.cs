using System;
using System.Windows.Forms;

using prjChessForms.PresentationUI;
using prjChessForms.MyChessLibrary;
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


            Controller controller = new Controller();
            Application.Run(controller.ChessForm);
        }
    }
}
