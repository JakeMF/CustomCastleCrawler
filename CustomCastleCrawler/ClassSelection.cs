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
        public string PlayerName;
        public string IntroMessage;
        public frmClassSelection(string playerName, string introMessage)
        {
            PlayerName = playerName;
            IntroMessage = introMessage;

            //Load class names and descriptions from XML.
            var gameData = new GameData();

            lblKnight.Text = gameData.Classes[0].ClassName;
            txtKnight.Text = gameData.Classes[0].ClassDescription;

            lblSoldier.Text = gameData.Classes[1].ClassName;
            txtSoldier.Text = gameData.Classes[1].ClassDescription;

            lblArcher.Text = gameData.Classes[2].ClassName;
            txtArcher.Text = gameData.Classes[2].ClassDescription;

            lblGiant.Text = gameData.Classes[3].ClassName;
            txtGiant.Text = gameData.Classes[3].ClassDescription;

            InitializeComponent();
        }

        //Class Selection Buttons
        private void btnKnight_Click(object sender, EventArgs e)
        {
            var classDescription = MainGame.SelectClass(PlayerName, lblKnight.Text);
            
            //Open Class Choice Form
            Form currentForm = Form.ActiveForm;
            currentForm.Hide();

            using (frmMain mainScren = new frmMain(classDescription + Environment.NewLine + IntroMessage))
                mainScren.ShowDialog();
            currentForm.Show();
        }

        private void btnSoldier_Click(object sender, EventArgs e)
        {
            //Change player class and get class specific intro part to append to beginning of intro text.
            var classDescription = MainGame.SelectClass(PlayerName, lblSoldier.Text);

            //Open Class Choice Form
            Form currentForm = Form.ActiveForm;
            currentForm.Hide();

            using (frmMain mainScren = new frmMain(classDescription + Environment.NewLine + IntroMessage))
                mainScren.ShowDialog();
            currentForm.Show();
        }

        private void btnArcher_Click(object sender, EventArgs e)
        {
            var classDescription = MainGame.SelectClass(PlayerName, lblArcher.Text);

            //Open Class Choice Form
            Form currentForm = Form.ActiveForm;
            currentForm.Hide();

            using (frmMain mainScren = new frmMain(classDescription + Environment.NewLine + IntroMessage))
                mainScren.ShowDialog();
            currentForm.Show();
        }

        private void btnGiant_Click(object sender, EventArgs e)
        {
            var classDescription = MainGame.SelectClass(PlayerName, lblGiant.Text);

            //Open Class Choice Form
            Form currentForm = Form.ActiveForm;
            currentForm.Hide();

            using (frmMain mainScren = new frmMain(classDescription + Environment.NewLine + IntroMessage))
                mainScren.ShowDialog();
            currentForm.Show();
        }
    }
}
