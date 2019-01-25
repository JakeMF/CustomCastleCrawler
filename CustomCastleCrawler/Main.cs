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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectedText = "Welcome to the land of Iscandal Artorias the Abysswalker. \n\nYou can use the four arrows to move North, South, East, and West. \n\n" +
                "You may encounter enemies, find treasure, or discover secrets. \n\nIf you encounter an enemy, you will be taken into another menu for the combat phase. \n\n" +
                "Explore, fight, and gain power; it is all that will keep you alive.";
        }
        
    }
}
