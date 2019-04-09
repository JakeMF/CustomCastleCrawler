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
        public MainGame MainGame;

        public frmStartup()
        {
            InitializeComponent();

            MainGame = new MainGame();

            //Set the form's text on the title bar to the game's name.
            this.Text = MainGame.GameConfigurations.GameName;
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
                //If the player cleared out the name field, give them an error message.
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
                var results = MainGame.LoadProgress(playerName, false);
                if (results == "success")
                {
                    //Save successfully loaded
                    introString = MainGame.StartGame(playerName, false);
                }
                else if (results == "tryagain")
                {
                    //They want to try again, exit this method to be called again.
                    return;
                }
                else
                {
                    //The player did not want to attempt to load the game again, so just start a new game.
                    introString = MainGame.StartGame(playerName, true);
                }

                //Open Class Choice Form
                this.Hide();

                using (frmMain frmMain = new frmMain(MainGame, introString))
                {
                    frmMain.ShowDialog();
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
