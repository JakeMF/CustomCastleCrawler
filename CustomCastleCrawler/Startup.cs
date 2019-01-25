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
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var playerName = txtName.Text;

            if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrWhiteSpace(playerName))
            {
                //Run Load Game method. This will in turn build the game.
                if(MainGame.LoadProgress(playerName, false))
                {
                    //Save successfully loaded
                    MainGame.StartGame(playerName, false);
                }
                else
                {
                    //Save was not loaded
                    MainGame.StartGame(playerName, true);
                }
            }
            else
            {
                MessageBox.Show("Please Enter a Valid Name");
            }
        }
    }
}
