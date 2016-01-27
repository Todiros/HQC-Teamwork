namespace Poker
{
    using System;
    using System.Windows.Forms;

    static class Engine
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PokerTable());
        }
    }
}
