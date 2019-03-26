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
        //This variable holds the game object, which will be passed to other Forms as a parameter.
        public MainGame MainGame;
        
        public frmMain(MainGame mainGame, string introMessage)
        {
            //First initialize the game object, then the form.
            MainGame = mainGame;
            InitializeComponent();
            
            //Populate the textbox with the introduction message.
            popTextbox(introMessage);

            //This populates the notes with any notes from past playthroughs.
            //If starting a new game, mainGame.TempMiscData will be blank.
            txtNotes.Text = mainGame.TempMiscData;
        }
        
        #region Menu Buttons

        private void mnuExit_Click(object sender, EventArgs e)
        {
            //Close the application.
            Application.Exit();
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            //Execute the save function, pass the player's notes as a parameter to be saved.
            MainGame.SaveProgress(txtNotes.Text);
        }
        

        private void clearConsoleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            txtMainOutput.Clear();
        }


        private void mnuHelp_Click(object sender, EventArgs e)
        {
            //Hide the main form
            this.Hide();

            //When the close the help form, show the main form again.
            using (frmHelp frmHelp = new frmHelp())
            {
                frmHelp.ShowDialog();
            }
            this.Show();
        }

        #endregion
        
        #region Movement Buttons 
        private void btnNorth_Click(object sender, EventArgs e)
        {
            //Populate the textbox with the results from moving North.
            popTextbox(MainGame.EvaluateInput("north"));
            
            //Keep track of the number of turns the player has taken.
            MainGame.TurnCount++;
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            //Populate the textbox with the results from moving South.
            popTextbox(MainGame.EvaluateInput("south"));
            MainGame.TurnCount++;
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            //Populate the textbox with the results from moving East.
            popTextbox(MainGame.EvaluateInput("east"));

            //Keep track of the number of turns the player has taken.
            MainGame.TurnCount++;
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            //Populate the textbox with the results from moving West.
            popTextbox(MainGame.EvaluateInput("west"));

            //Keep track of the number of turns the player has taken.
            MainGame.TurnCount++;
        }

        #endregion

        #region Action Buttons

        private void btn_Equip_Click(object sender, EventArgs e)
        {
            //Generate the string player's equipment list to be displayed.
            string output = MainGame.GenerateEquipmentList();
            
            //Clear the textbox before populating with the equipment list for readability.
            txtMainOutput.Clear();

            //ToDo: Test that this functions the same using the poptextbox function
            popTextbox(output);

        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            //Generate the player's statistics to be displayed.
            string output = MainGame.GenerateStatistics();

            //Clear the textbox before populating with the player's statistics for readability.
            txtMainOutput.Clear();
            
            //Populate the textbox
            popTextbox(output);
        }

        #endregion

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

            //Ensure the text is centered.
            txtMainOutput.SelectionAlignment = HorizontalAlignment.Center;

            //Change the text color to black because the default color on a disabled textbox is a gray which is much harder to read.
            txtMainOutput.SelectionColor = Color.Black;

            //Populate the textbox.
            txtMainOutput.SelectedText = output;

            //Ensure the textbox is scrolled down to the most recent message.
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

        private void txtMainOutput_TextChanged(object sender, EventArgs e)
        {
            //When text is added to the textbox, ensure it takes focus.
            txtMainOutput.Focus();
            txtMainOutput.SelectionStart = txtMainOutput.TextLength;
            
            //Ensure that the textbox is scrolled down to the most recent message.
            txtMainOutput.ScrollToCaret();

            //Force textbox to redraw, this fixed some minor display formatting issues.
            txtMainOutput.Refresh();
        }

        private void DisableFormControls()
        {
            //Disable all movement controls so that the player can no longer 'play' the game. 
            //This function will be called when the player dies.
            btnEast.Enabled = false;
            btnWest.Enabled = false;
            btnNorth.Enabled = false;
            btnSouth.Enabled = false;
        }

    }
}
