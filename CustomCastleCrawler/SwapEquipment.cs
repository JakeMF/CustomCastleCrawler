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
    public partial class frmSwapEquipment : Form
    {
        //This variable is used to store the type of equipment swap we are performing, this is used to determine which output and methods to use.
        string ChangeType = "";
        MainGame MainGame;
        Armor NewArmor;
        Weapon NewWeapon;

        public frmSwapEquipment(MainGame MainGame, Weapon OldWeapon, Weapon NewWeapon)
        {
            InitializeComponent();
            //Form was constructed using the weapon constructor, so we know they found a weapon.
            ChangeType = "weapon";
            this.MainGame = MainGame;
            this.NewWeapon = NewWeapon;

            //This is a Weapon swap, show weapon controls.
            pnlArmor.Visible = false;
            pnlWeapon.Visible = true;

            //Ensure that the name is not too long to fit the control
            lblOldWeaponName.Text = OldWeapon.Name.Length <= 21 ? OldWeapon.Name : OldWeapon.Name.Substring(0, 18) + "...";

            //Populate the stat labels.
            lblOldWeaponRarity.Text = OldWeapon.Rarity.ToString();
            lblOldBDamage.Text = OldWeapon.BDamage.ToString();
            lblOldAPDamage.Text = OldWeapon.APDamage.ToString();
            lblOldWeaponEvasion.Text = OldWeapon.Evasion.ToString();

            //Ensure that the name is not too long to fit the control
            lblNewWeaponName.Text = NewWeapon.Name.Length <= 21 ? NewWeapon.Name : NewWeapon.Name.Substring(0, 18) + "...";

            //Populate the stat labels.
            lblNewWeaponRarity.Text = NewWeapon.Rarity.ToString();
            lblNewBDamage.Text = NewWeapon.BDamage.ToString();
            lblNewAPDamage.Text = NewWeapon.APDamage.ToString();
            lblNewWeaponEvasion.Text = NewWeapon.Evasion.ToString();
        }
        public frmSwapEquipment(MainGame MainGame, Armor OldArmor, Armor NewArmor)
        {
            InitializeComponent();
            
            //Form was constructed using the armor constructor, so we know they found armor.
            ChangeType = "armor";


            this.MainGame = MainGame;
            this.NewArmor = NewArmor;

            //This is an armor swap, show armor controls.
            pnlWeapon.Visible = false;
            pnlArmor.Visible = true;

            //Ensure that the name is not too long to fit the control
            lblOldArmorName.Text = OldArmor.Name.Length <= 21 ? OldArmor.Name : OldArmor.Name.Substring(0, 18) + "...";
            
            //Populate the stat labels.
            lblOldArmorRarity.Text = OldArmor.Rarity.ToString();
            lblOldArmorVal.Text = OldArmor.ArmorVal.ToString();
            lblOldArmorEvasion.Text = OldArmor.Evasion.ToString();

            //Ensure that the name is not too long to fit the control
            lblNewArmorName.Text = NewArmor.Name.Length <= 21 ? NewArmor.Name : NewArmor.Name.Substring(0, 18) + "...";

            //Populate the stat labels.
            lblNewArmorRarity.Text = NewArmor.Rarity.ToString();
            lblNewArmorVal.Text = NewArmor.ArmorVal.ToString();
            lblNewArmorEvasion.Text = NewArmor.Evasion.ToString();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            //The want the new equipment
            switch (ChangeType)
            {
                case "armor":
                    MainGame.SwapArmor(NewArmor);

                    //Check if the equipment starts with a vowel so that we can use 'an' instead of 'a'.
                    if (NewArmor.Name.Substring(1, 1) == "a" || NewArmor.Name.Substring(1, 1) == "e" || NewArmor.Name.Substring(1, 1) == "i" || NewArmor.Name.Substring(1, 1) == "o" || NewArmor.Name.Substring(1, 1) == "u")
                    {
                        MainGame.TempMiscData = "You have found an " + NewArmor.Name + " you have droppped your " + MainGame.GetPlayerArmor().Name + " for the new armor." + Environment.NewLine;
                    }
                    else
                    {
                        MainGame.TempMiscData = "You have found a " + NewArmor.Name + " you have droppped your " + MainGame.GetPlayerArmor().Name + " for the new armor." + Environment.NewLine;
                    }
                    break;
                case "weapon":
                    MainGame.SwapWeapon(NewWeapon);

                    //Check if the equipment starts with a vowel so that we can use 'an' instead of 'a'.
                    if (NewWeapon.Name.Substring(1, 1) == "a" || NewWeapon.Name.Substring(1, 1) == "e" || NewWeapon.Name.Substring(1, 1) == "i" || NewWeapon.Name.Substring(1, 1) == "o" || NewWeapon.Name.Substring(1, 1) == "u")
                    {
                        MainGame.TempMiscData = "You have found an " + NewWeapon.Name + " you have droppped your " + MainGame.GetPlayerWeapon().Name + " for the new weapon." + Environment.NewLine;
                    }
                    else
                    {
                        MainGame.TempMiscData = "You have found a " + NewWeapon.Name + " you have droppped your " + MainGame.GetPlayerWeapon().Name + " for the new weapon." + Environment.NewLine;
                    }
                    break;
                default:
                    MessageBox.Show("There was an error changing your weapon. Please restart the game WIHTOUT SAVING.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            //they want to keep their old equipment
            MainGame.TempMiscData = "You have found a new piece of equipment, but you decided to keep your old gear." + Environment.NewLine;
            Close();
        }
    }
}
