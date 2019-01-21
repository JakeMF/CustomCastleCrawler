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
        public frmClassSelection(string PlayerName)
        {
            this.PlayerName = PlayerName;
            InitializeComponent();
        }

        //Class Selection Buttons
        private void btnKnight_Click(object sender, EventArgs e)
        {

        }

        private void btnSoldier_Click(object sender, EventArgs e)
        {

        }

        private void btnArcher_Click(object sender, EventArgs e)
        {

        }

        private void btnGiant_Click(object sender, EventArgs e)
        {

        }
    }
}
