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
            txtCombatLog.SelectionAlignment = HorizontalAlignment.Center;
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

            popCombatResults(combatResults);
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            //call block function
            string combatResults = MainGame.BattleEnemy("block");
            
            popCombatResults(combatResults);
        }

        private void btnHeal_Click(object sender, EventArgs e)
        {
            string healResults = MainGame.healPlayer();

            popCombatResults(healResults);
        }

        private void btnEscape_Click(object sender, EventArgs e)
        {
            string escapeResults = MainGame.EscapeAttempt();

            popCombatResults(escapeResults);
        }

        private void popCombatResults(string results)
        {
            var resArray = results.Split('|');

            lblPlayerHealth.Text = resArray.Length > 0 ? resArray[0] : "Er/Er";
            lblPlayerStamina.Text = resArray.Length > 1 ? resArray[1] : "Er/Er";
            lblEnemyHealth.Text = resArray.Length > 2 ? resArray[2] : "Er/Er";
            txtCombatLog.SelectedText = resArray.Length > 3 ? resArray[3] + Environment.NewLine: "Error retrieving combat information. Please restart game WITHOUT SAVING.";

            txtCombatLog.SelectionAlignment = HorizontalAlignment.Center;
            txtCombatLog.SelectionColor = Color.Black;
            txtCombatLog.ScrollToCaret();
            

            //The enemy was defeated, close the combat window
            if (!MainGame.ActiveEnemy || MainGame.PlayerDied)
            {
                Close();
            }
        }
    }
}
