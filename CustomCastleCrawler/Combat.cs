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
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;

            grpPlayer.Text = this.MainGame.PlayerName;

            if (this.MainGame.ActiveEnemy)
            {
                Enemy currentEnemy = this.MainGame.CurrentEnemy;

                grpEnemy.Text = currentEnemy.Name;
                lblEnemyHealth.Text = currentEnemy.CurrentHealth + "/" + currentEnemy.MaxHealth;
            }
            
        }
    }
}
