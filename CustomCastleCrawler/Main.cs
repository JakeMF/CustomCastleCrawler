using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomCastleCrawler
{
    public partial class frmMain : Form
    {
        public MainGame MainGame;

        public frmStartup startupForm { get; set; }
        public frmMain(MainGame MainGame, string introMessage)
        {
            this.MainGame = MainGame;
            InitializeComponent();
            txtMainOutput.SelectionAlignment = HorizontalAlignment.Center;
            txtMainOutput.SelectedText = introMessage;

            if(this.MainGame.ActiveEnemy)
            {
                System.Threading.Thread.Sleep(750);

                //Open Combat Form
                this.Hide();

                using (frmCombat combatScreen = new frmCombat(MainGame))
                {
                    combatScreen.ShowDialog();
                }
                this.Show();
            }
        }

        #region Menu Buttons

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            MainGame.SaveProgress();
        }
        

        private void clearConsoleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            txtMainOutput.Clear();
        }

        private void mnuExploration_Click(object sender, EventArgs e)
        {

            //Open Help Form
            this.Hide();

            using (frmHelp helpScreen = new frmHelp())
            {
                helpScreen.ShowDialog();
            }
            this.Show();
        }

        private void combatToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Open Help Form
            this.Hide();

            using (frmHelp helpScreen = new frmHelp())
            {
                helpScreen.ShowDialog();
            }
            this.Show();
        }

        #endregion

        #region Movement Buttons 
        private void btnNorth_Click(object sender, EventArgs e)
        {
            popTextbox(MainGame.EvaluateInput("north"));
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            popTextbox(MainGame.EvaluateInput("south"));
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            popTextbox(MainGame.EvaluateInput("east"));
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            popTextbox(MainGame.EvaluateInput("west"));
        }

        private void popTextbox(string output)
        {
            if (output.Contains("|"))
            {
                //split output
                var outArray = output.Split('|');
                var type = outArray.Length > 0 ? outArray[0] : "";
                var outputMessage = outArray.Length > 1 ? outArray[1] : "";

                if(type == "NewWeapon")
                {
                    this.Hide();

                    using (frmSwapEquipment frmSwapEquipment = new frmSwapEquipment(MainGame, MainGame.GetPlayerWeapon(), MainGame.TempWeapon))
                    {
                        frmSwapEquipment.ShowDialog();
                    }
                    this.Show();
                }
                else if (type == "NewArmor")
                {
                    this.Hide();

                    using (frmSwapEquipment frmSwapEquipment = new frmSwapEquipment(MainGame, MainGame.GetPlayerArmor(), MainGame.TempArmor))
                    {
                        frmSwapEquipment.ShowDialog();
                    }
                    this.Show();
                }
                else
                {
                    //type unknown
                    MessageBox.Show("Error loading equipment found. Please restart the game WITHOUT SAVING");
                }
            }

            txtMainOutput.SelectionAlignment = HorizontalAlignment.Center;
            txtMainOutput.SelectedText = output;
        }
        #endregion

        #region Action Buttons

        private void btn_Equip_Click(object sender, EventArgs e)
        {
            string output = MainGame.GenerateEquipmentList();

            txtMainOutput.Clear();
            txtMainOutput.SelectionAlignment = HorizontalAlignment.Center;
            txtMainOutput.SelectedText = output;

        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            string output = MainGame.GenerateStatistics();

            txtMainOutput.Clear();
            txtMainOutput.SelectionAlignment = HorizontalAlignment.Center;
            txtMainOutput.SelectedText = output;
        }

        #endregion

    }
}
