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
            popTextbox(introMessage);
            txtNotes.Text = MainGame.TempMiscData;
        }

        #region Menu Buttons

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            MainGame.SaveProgress(txtNotes.Text);
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
            MainGame.TurnCount++;
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            popTextbox(MainGame.EvaluateInput("south"));
            MainGame.TurnCount++;
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            popTextbox(MainGame.EvaluateInput("east"));
            MainGame.TurnCount++;
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            popTextbox(MainGame.EvaluateInput("west"));
            MainGame.TurnCount++;
        }

        private void popTextbox(string output)
        {
            if (output.Contains("|"))
            {
                //split output
                var outArray = output.Split('|');
                var type = outArray.Length > 0 ? outArray[0] : "";
                var outputMessage = outArray.Length > 1 ? outArray[1] : "";
                //set output to the message portion only 
                output = outputMessage;

                if (type == "NewWeapon")
                {
                    this.Hide();

                    using (frmSwapEquipment frmSwapEquipment = new frmSwapEquipment(MainGame, MainGame.GetPlayerWeapon(), MainGame.TempWeapon))
                    {
                        frmSwapEquipment.ShowDialog();
                    }
                    this.Show();

                    popTextbox(MainGame.TempMiscData);
                }
                else if (type == "NewArmor")
                {
                    this.Hide();

                    using (frmSwapEquipment frmSwapEquipment = new frmSwapEquipment(MainGame, MainGame.GetPlayerArmor(), MainGame.TempArmor))
                    {
                        frmSwapEquipment.ShowDialog();
                    }
                    this.Show();

                    popTextbox(MainGame.TempMiscData);
                }
                else
                {
                    //type unknown
                    MessageBox.Show("Error loading equipment found. Please restart the game WITHOUT SAVING. Check XML data integrity if modified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            txtMainOutput.SelectionAlignment = HorizontalAlignment.Center;
            txtMainOutput.SelectionColor = Color.Black;
            txtMainOutput.SelectedText = output;
            txtMainOutput.ScrollToCaret();

            //Check to see if there is an active enemy. If so, open the combat form.
            if (MainGame.ActiveEnemy)
            {
                System.Threading.Thread.Sleep(1000);

                //Open Combat Form
                this.Hide();

                using (frmCombat combatScreen = new frmCombat(MainGame))
                {
                    combatScreen.ShowDialog();
                }
                //DO NOT show and re-hide here, this bugs the "ScrollToCaret" function causing the final line to be partially obscured. I believe this is related to form and texbox focus at the time.
                //After removing the Show() from here and the Hide() from Line #160, the ScrollToCaret is working as intended.


                if (MainGame.PlayerDied)
                {
                    //Set active enemy to false to allow for reuse of the popTextbox function without entering the combat code block.
                    MainGame.ActiveEnemy = false;
                    this.Show();

                    //Clear textbox and output final statistics.
                    txtMainOutput.Clear();
                    if (MainGame.CurrentEnemy.Name.Substring(1, 1) == "a" || MainGame.CurrentEnemy.Name.Substring(1, 1) == "e" || MainGame.CurrentEnemy.Name.Substring(1, 1) == "i" || MainGame.CurrentEnemy.Name.Substring(1, 1) == "o" || MainGame.CurrentEnemy.Name.Substring(1, 1) == "u")
                    {
                        popTextbox("You have been defeated by an " + MainGame.CurrentEnemy.Name + " your run is now over." + Environment.NewLine + MainGame.GenerateStatistics() + Environment.NewLine + "You may quit the game using the Menu -> Exit option, or by clicking the 'X' in the top right hand corner." + Environment.NewLine);
                    }
                    else
                    {
                        popTextbox("You have been defeated by a " + MainGame.CurrentEnemy.Name + " your run is now over." + Environment.NewLine + MainGame.GenerateStatistics() + Environment.NewLine + "You may quit the game using the Menu -> Exit option, or by clicking the 'X' in the top right hand corner." + Environment.NewLine);
                    }

                    DisableFormControls();

                    return;
                }

                //Check if the user prematurely closed the combat form.
                if (MainGame.ActiveEnemy)
                {

                    //User closed the combat screen when they should not have inform them and re-open it.
                    MessageBox.Show("You are still in combat, you must finish combat before performing other actions. Additional attempts to close the combat window early will result in the game closing.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    using (frmCombat combatScreen = new frmCombat(MainGame))
                    {
                        combatScreen.ShowDialog();
                    }
                    this.Show();

                    //Rather than have a loop to allow the user to infinitely close the window for it to automatically re-open,
                    //I assume that if the user closes the window twice in a row without completing the combat that they are attempting to force quit the game. I ablige them.
                    MessageBox.Show("You have closed the combat window prematurely twice in a row. It is assumed you are trying to force quit the game. The game will now close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }
                else
                {
                    //the combat was successfully completed.
                    //Populate the textbox with the results from the combat form.
                    this.Show();
                    popTextbox(MainGame.TempMiscData);

                }
            }
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

        private void txtMainOutput_TextChanged(object sender, EventArgs e)
        {
            txtMainOutput.Focus();
            txtMainOutput.SelectionStart = txtMainOutput.TextLength;
            txtMainOutput.ScrollToCaret();
            txtMainOutput.Refresh();
        }

        private void DisableFormControls()
        {
            btnEast.Enabled = false;
            btnWest.Enabled = false;
            btnNorth.Enabled = false;
            btnSouth.Enabled = false;
        }
    }
}
