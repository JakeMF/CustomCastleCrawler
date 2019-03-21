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
    public partial class frmHelp : Form
    {
        public frmHelp()
        {
            InitializeComponent();
        }
        
        //If the click the exit button, close the form, returing to the main form.
        private void mnuExit_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
