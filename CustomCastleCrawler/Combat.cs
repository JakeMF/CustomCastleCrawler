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
    public partial class frmCombat : Form
    {
        public MainGame MainGame;
        public frmCombat(MainGame MainGame)
        {
            this.MainGame = MainGame;

            InitializeComponent();

            //Ensure the textbox is scrolled down to the most recent message.
            txtCombatLog.SelectionAlignment = HorizontalAlignment.Center;

            //Change the text color to black because the default color on a disabled textbox is a gray which is much harder to read.
            txtCombatLog.SelectionColor = Color.Black;

            //Populate default player health and stamina values
            string results = MainGame.GetPlayerHealthAndStamina();
            var resArray = results.Split('|');

            lblPlayerHealth.Text = resArray.Length > 0 ? resArray[0] : "Er/Er";
            lblPlayerStamina.Text = resArray.Length > 1 ? resArray[1] : "Er/Er";
            
            //Populate default enemy health value.
            lblEnemyHealth.Text = MainGame.CurrentEnemy.GetHealth();

            //Ensure that the name is not too long to fit the control
            grpPlayer.Text = this.MainGame.PlayerName.Length <= 21 ? this.MainGame.PlayerName : this.MainGame.PlayerName.Substring(0, 18) + "...";

            if (this.MainGame.ActiveEnemy)
            {
                Enemy currentEnemy = this.MainGame.CurrentEnemy;

                //Ensure that the name is not too long to fit the control
                grpEnemy.Text = currentEnemy.Name.Length <= 21 ? currentEnemy.Name : currentEnemy.Name.Substring(0, 18) + "...";
                lblEnemyHealth.Text = MainGame.CurrentEnemy.GetHealth();
            }
            
        }

        private void btnAttack_Click(object sender, EventArgs e)
        {
            //Call attack function
            string combatResults = MainGame.BattleEnemy("attack");

            //populate the textbox with the results.
            popCombatResults(combatResults);
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            //call block function
            string combatResults = MainGame.BattleEnemy("block");

            //populate the textbox with the results.
            popCombatResults(combatResults);
        }

        private void btnHeal_Click(object sender, EventArgs e)
        {
            //Get the results of the player's attempt to heal.
            string healResults = MainGame.healPlayer();

            //populate the textbox with the results.
            popCombatResults(healResults);
        }

        private void btnEscape_Click(object sender, EventArgs e)
        {
            //Get the results of the player's attempt to escape.
            string escapeResults = MainGame.EscapeAttempt();

            //populate the textbox with the results.
            popCombatResults(escapeResults);
        }

        private void popCombatResults(string results)
        {
            //Split the string to separate player/enemy health and stamina values from the text message of the combat.
            var resArray = results.Split('|');

            //Fill in player and enemy stat lables.
            lblPlayerHealth.Text = resArray.Length > 0 ? resArray[0] : "Er/Er";
            lblPlayerStamina.Text = resArray.Length > 1 ? resArray[1] : "Er/Er";
            lblEnemyHealth.Text = resArray.Length > 2 ? resArray[2] : "Er/Er";

            //Fill in combat log.
            txtCombatLog.SelectedText = resArray.Length > 3 ? resArray[3] + Environment.NewLine: "Error retrieving combat information. Please restart game WITHOUT SAVING.";

            //Ensure that the text is centered
            txtCombatLog.SelectionAlignment = HorizontalAlignment.Center;

            //Change the text color to black because the default color on a disabled textbox is a gray which is much harder to read.
            txtCombatLog.SelectionColor = Color.Black;

            //Ensure the textbox is scrolled down to the most recent message.
            txtCombatLog.ScrollToCaret();
            

            //The enemy was defeated or the player was killed, close the combat window
            if (!MainGame.ActiveEnemy || MainGame.PlayerDied)
            {
                Close();
            }
        }
    }
}
