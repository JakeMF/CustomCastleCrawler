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
        public frmCombat()
        {
            InitializeComponent();
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectedText = "You have encountered a Skeleton! \n\nThe Skeleton attacked you, dealing 10 damage. \n\nYou tried to escape, you failed... \n\nThe skeleton attacked you, dealing 10 damage. \n\nYou attacked the skeleton, dealing 5 damage. \n\nThe skeleton attacked you, but you dodged the attack!";
        }
    }
}
