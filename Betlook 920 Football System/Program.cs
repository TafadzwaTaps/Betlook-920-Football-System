using Betlook_920_Football_System;
using System;
using System.Windows.Forms;

namespace BetLook_920_Football
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
            //Application.Run(new Welcome_Page());
            Application.Run(new Welcome_Page());

        }
    }
}
