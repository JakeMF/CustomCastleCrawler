using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CustomCastleCrawler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new frmStartup());

            //Application.Run(new frmSwapEquipment(new MainGame(), new Armor("Iron Chestpiece", 5, "", 5, 5), new Armor("Steel Plate Mail", 10, "", 10, -5)));
        }
    }
}
