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
        //Initialize MainGame class 
        public MainGame MainGame = new MainGame();

        public frmStartup()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            var playerName = txtName.Text;

            if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrWhiteSpace(playerName))
            {
                var introString = MainGame.StartGame(playerName, true);

                //Open Class Choice Form
                this.Hide();

                using (frmClassSelection classSelection = new frmClassSelection(MainGame, introString))
                {
                    classSelection.ShowDialog();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Enter a Valid Name");
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string playerName = txtName.Text;
            string introString;
            if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrWhiteSpace(playerName))
            {
                //Run Load Game method. This will in turn build the game.
                if(MainGame.LoadProgress(playerName, false))
                {
                    //Save successfully loaded
                    introString = MainGame.StartGame(playerName, false);
                }
                else
                {
                    //Save was not loaded
                    introString = MainGame.StartGame(playerName, true);
                }

                //Open Class Choice Form
                this.Hide();

                using (frmClassSelection classSelection = new frmClassSelection(MainGame, introString))
                {
                    classSelection.ShowDialog();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Enter a Valid Name");
            }
        }
    }
}
