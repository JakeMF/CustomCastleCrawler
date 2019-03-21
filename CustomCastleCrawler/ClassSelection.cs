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
    public partial class frmClassSelection : Form
    {
        public frmStartup startupForm { get; set; }
        public MainGame MainGame;
        public string IntroMessage;
        public frmClassSelection(MainGame MainGame, string introMessage)
        {
            this.MainGame = MainGame;

            InitializeComponent();
            
            //store the intro message to be passed to the main form later.
            IntroMessage = introMessage;

            //Load class names and descriptions from XML.
            var gameData = new GameData();

            //Set class name and descriptions to those pulled from XML.
            //Note: The names here are based on the startup classes I chose, which may or may not have any bearing on the real classes.
            lblKnight.Text = gameData.Classes[0].ClassName;
            txtKnight.Text = gameData.Classes[0].ClassDescription;

            lblSoldier.Text = gameData.Classes[1].ClassName;
            txtSoldier.Text = gameData.Classes[1].ClassDescription;

            lblArcher.Text = gameData.Classes[2].ClassName;
            txtArcher.Text = gameData.Classes[2].ClassDescription;

            lblGiant.Text = gameData.Classes[3].ClassName;
            txtGiant.Text = gameData.Classes[3].ClassDescription;
        }

        //Class Selection Buttons
        private void btnKnight_Click(object sender, EventArgs e)
        {
            string classDescription = MainGame.SelectClass(MainGame.PlayerName, lblKnight.Text);
            
            //Open Main Form
            this.Hide();

            using (frmMain mainScren = new frmMain(MainGame, IntroMessage))
            {
                mainScren.ShowDialog();
            }
            this.Close();
        }

        private void btnSoldier_Click(object sender, EventArgs e)
        {
            //Change player class and get class specific intro part to append to beginning of intro text.
            string classDescription = MainGame.SelectClass(MainGame.PlayerName, lblSoldier.Text);

            //Open Main Form
            this.Hide();

            using (frmMain mainScren = new frmMain(MainGame, IntroMessage))
            {
                mainScren.ShowDialog();
            }
            this.Close();

        }

        private void btnArcher_Click(object sender, EventArgs e)
        {
            string classDescription = MainGame.SelectClass(MainGame.PlayerName, lblArcher.Text);

            //Open Main Form
            this.Hide();

            using (frmMain mainScren = new frmMain(MainGame, IntroMessage))
            {
                mainScren.ShowDialog();
            }
            this.Close();
        }

        private void btnGiant_Click(object sender, EventArgs e)
        {
            string classDescription = MainGame.SelectClass(MainGame.PlayerName, lblGiant.Text);

            //Open Main Form
            this.Hide();

            using (frmMain mainScren = new frmMain(MainGame, IntroMessage))
            {
                mainScren.ShowDialog();
            }
            this.Close();
        }
    }
}
