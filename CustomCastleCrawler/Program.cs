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

            Application.Run(new frmSwapEquipment(new MainGame(), new Weapon("Claymore", 20, "", 10, 5, 0), new Weapon("Great Club", 10, "", 20, 5, -10)));

            //Application.Run(new frmSwapEquipment(new MainGame(), new Armor("Iron Chestpiece", 5, "", 5, 5), new Armor("Steel Plate Mail", 10, "", 10, -5)));
        }
    }
}
