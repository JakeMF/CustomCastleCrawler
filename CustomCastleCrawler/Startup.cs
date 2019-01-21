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
    public partial class frmStartup : Form
    {
        public frmStartup()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            var playerName = txtName.Text;

            if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrWhiteSpace(playerName))
            {
                MainGame.StartGame(playerName, true);
            }
            else
            {
                MessageBox.Show("Please Enter a Valid Name");
                return;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var playerName = txtName.Text;

            if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrWhiteSpace(playerName))
            {
                //Run Load Game method. This will in turn build the game.
                MainGame.LoadProgress(playerName, false);
            }
            else
            {
                MessageBox.Show("Please Enter a Valid Name");
                return;
            }
        }
    }
}
