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
        string ChangeType = "";
        MainGame MainGame;
        Armor NewArmor;
        Weapon NewWeapon;
        public frmSwapEquipment(MainGame MainGame, Weapon OldWeapon, Weapon NewWeapon)
        {
            InitializeComponent();
            ChangeType = "weapon";
            this.MainGame = MainGame;
            this.NewWeapon = NewWeapon;
            //This is a Weapon swap, show weapon controls.
            pnlArmor.Visible = false;
            pnlWeapon.Visible = true;

            lblOldWeaponName.Text = OldWeapon.Name.Length <= 21 ? OldWeapon.Name : OldWeapon.Name.Substring(0, 18) + "...";
            lblOldWeaponRarity.Text = OldWeapon.Rarity.ToString();
            lblOldBDamage.Text = OldWeapon.BDamage.ToString();
            lblOldAPDamage.Text = OldWeapon.APDamage.ToString();
            lblOldWeaponEvasion.Text = OldWeapon.Evasion.ToString();

            lblNewWeaponName.Text = NewWeapon.Name.Length <= 21 ? NewWeapon.Name : NewWeapon.Name.Substring(0, 18) + "...";
            lblNewWeaponRarity.Text = NewWeapon.Rarity.ToString();
            lblNewBDamage.Text = NewWeapon.BDamage.ToString();
            lblNewAPDamage.Text = NewWeapon.APDamage.ToString();
            lblNewWeaponEvasion.Text = NewWeapon.Evasion.ToString();
        }
        public frmSwapEquipment(MainGame MainGame, Armor OldArmor, Armor NewArmor)
        {
            InitializeComponent();
            ChangeType = "armor";
            this.MainGame = MainGame;
            this.NewArmor = NewArmor;

            //This is an armor swap, show armor controls.
            pnlWeapon.Visible = false;
            pnlArmor.Visible = true;

            //Ensure that the name is not too long to fit the control
            lblOldArmorName.Text = OldArmor.Name.Length <= 21 ? OldArmor.Name : OldArmor.Name.Substring(0, 18) + "...";
            lblOldArmorRarity.Text = OldArmor.Rarity.ToString();
            lblOldArmorVal.Text = OldArmor.ArmorVal.ToString();
            lblOldArmorEvasion.Text = OldArmor.Evasion.ToString();

            //Ensure that the name is not too long to fit the control
            lblNewArmorName.Text = NewArmor.Name.Length <= 21 ? NewArmor.Name : NewArmor.Name.Substring(0, 18) + "...";
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
                    break;
                case "weapon":
                    MainGame.SwapWeapon(NewWeapon);
                    break;
                default:
                    MessageBox.Show("There was an error changing your weapon. Please restart the game WIHTOUT SAVING.");
                    break;
            }

            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            //they want to keep their old equipment
            Close();
        }
    }
}
