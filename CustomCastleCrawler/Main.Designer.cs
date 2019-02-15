namespace CustomCastleCrawler
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblNotes = new System.Windows.Forms.Label();
            this.lblStats = new System.Windows.Forms.Label();
            this.lblEquipment = new System.Windows.Forms.Label();
            this.txtMainOutput = new System.Windows.Forms.RichTextBox();
            this.txtNotes = new System.Windows.Forms.RichTextBox();
            this.btn_Equip = new System.Windows.Forms.Button();
            this.btnStats = new System.Windows.Forms.Button();
            this.btnWest = new System.Windows.Forms.Button();
            this.btnSouth = new System.Windows.Forms.Button();
            this.btnEast = new System.Windows.Forms.Button();
            this.btnNorth = new System.Windows.Forms.Button();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.mnuMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClearConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExploration = new System.Windows.Forms.ToolStripMenuItem();
            this.combatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotes.Location = new System.Drawing.Point(339, 479);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(48, 17);
            this.lblNotes.TabIndex = 25;
            this.lblNotes.Text = "Notes";
            // 
            // lblStats
            // 
            this.lblStats.AutoSize = true;
            this.lblStats.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStats.Location = new System.Drawing.Point(490, 362);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(43, 17);
            this.lblStats.TabIndex = 24;
            this.lblStats.Text = "Stats";
            // 
            // lblEquipment
            // 
            this.lblEquipment.AutoSize = true;
            this.lblEquipment.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipment.Location = new System.Drawing.Point(366, 362);
            this.lblEquipment.Name = "lblEquipment";
            this.lblEquipment.Size = new System.Drawing.Size(83, 17);
            this.lblEquipment.TabIndex = 23;
            this.lblEquipment.Text = "Equipment";
            // 
            // txtMainOutput
            // 
            this.txtMainOutput.Enabled = false;
            this.txtMainOutput.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMainOutput.Location = new System.Drawing.Point(13, 59);
            this.txtMainOutput.Name = "txtMainOutput";
            this.txtMainOutput.Size = new System.Drawing.Size(578, 284);
            this.txtMainOutput.TabIndex = 21;
            this.txtMainOutput.Text = "";
            this.txtMainOutput.TextChanged += new System.EventHandler(this.txtMainOutput_TextChanged);
            // 
            // txtNotes
            // 
            this.txtNotes.Font = new System.Drawing.Font("Britannic Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes.Location = new System.Drawing.Point(342, 500);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(249, 138);
            this.txtNotes.TabIndex = 20;
            this.txtNotes.Text = "";
            // 
            // btn_Equip
            // 
            this.btn_Equip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Equip.BackgroundImage")));
            this.btn_Equip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_Equip.FlatAppearance.BorderSize = 0;
            this.btn_Equip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Equip.Location = new System.Drawing.Point(370, 392);
            this.btn_Equip.Name = "btn_Equip";
            this.btn_Equip.Size = new System.Drawing.Size(75, 75);
            this.btn_Equip.TabIndex = 19;
            this.btn_Equip.UseVisualStyleBackColor = true;
            this.btn_Equip.Click += new System.EventHandler(this.btn_Equip_Click);
            // 
            // btnStats
            // 
            this.btnStats.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStats.BackgroundImage")));
            this.btnStats.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStats.FlatAppearance.BorderSize = 0;
            this.btnStats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStats.Location = new System.Drawing.Point(478, 392);
            this.btnStats.Name = "btnStats";
            this.btnStats.Size = new System.Drawing.Size(75, 75);
            this.btnStats.TabIndex = 18;
            this.btnStats.UseVisualStyleBackColor = true;
            this.btnStats.Click += new System.EventHandler(this.btnStats_Click);
            // 
            // btnWest
            // 
            this.btnWest.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnWest.BackgroundImage")));
            this.btnWest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnWest.Location = new System.Drawing.Point(13, 473);
            this.btnWest.Name = "btnWest";
            this.btnWest.Size = new System.Drawing.Size(75, 75);
            this.btnWest.TabIndex = 17;
            this.btnWest.UseVisualStyleBackColor = true;
            this.btnWest.Click += new System.EventHandler(this.btnWest_Click);
            // 
            // btnSouth
            // 
            this.btnSouth.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSouth.BackgroundImage")));
            this.btnSouth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSouth.Location = new System.Drawing.Point(94, 554);
            this.btnSouth.Name = "btnSouth";
            this.btnSouth.Size = new System.Drawing.Size(75, 75);
            this.btnSouth.TabIndex = 16;
            this.btnSouth.UseVisualStyleBackColor = true;
            this.btnSouth.Click += new System.EventHandler(this.btnSouth_Click);
            // 
            // btnEast
            // 
            this.btnEast.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEast.BackgroundImage")));
            this.btnEast.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEast.Location = new System.Drawing.Point(175, 473);
            this.btnEast.Name = "btnEast";
            this.btnEast.Size = new System.Drawing.Size(75, 75);
            this.btnEast.TabIndex = 15;
            this.btnEast.UseVisualStyleBackColor = true;
            this.btnEast.Click += new System.EventHandler(this.btnEast_Click);
            // 
            // btnNorth
            // 
            this.btnNorth.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNorth.BackgroundImage")));
            this.btnNorth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNorth.Location = new System.Drawing.Point(94, 392);
            this.btnNorth.Name = "btnNorth";
            this.btnNorth.Size = new System.Drawing.Size(75, 75);
            this.btnNorth.TabIndex = 14;
            this.btnNorth.UseVisualStyleBackColor = true;
            this.btnNorth.Click += new System.EventHandler(this.btnNorth_Click);
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMenu,
            this.mnuClearConsole,
            this.mnuHelp});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(609, 24);
            this.menuBar.TabIndex = 22;
            this.menuBar.Text = "menuStrip1";
            // 
            // mnuMenu
            // 
            this.mnuMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSave,
            this.mnuExit});
            this.mnuMenu.Font = new System.Drawing.Font("Britannic Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnuMenu.Name = "mnuMenu";
            this.mnuMenu.Size = new System.Drawing.Size(49, 20);
            this.mnuMenu.Text = "&Menu";
            // 
            // mnuSave
            // 
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(100, 22);
            this.mnuSave.Text = "&Save";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(100, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuClearConsole
            // 
            this.mnuClearConsole.Font = new System.Drawing.Font("Britannic Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnuClearConsole.Name = "mnuClearConsole";
            this.mnuClearConsole.Size = new System.Drawing.Size(99, 20);
            this.mnuClearConsole.Text = "&Clear Console";
            this.mnuClearConsole.Click += new System.EventHandler(this.clearConsoleToolStripMenuItem_Click_1);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExploration,
            this.combatToolStripMenuItem});
            this.mnuHelp.Font = new System.Drawing.Font("Britannic Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(45, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuExploration
            // 
            this.mnuExploration.Name = "mnuExploration";
            this.mnuExploration.Size = new System.Drawing.Size(140, 22);
            this.mnuExploration.Text = "&Exploration";
            this.mnuExploration.Click += new System.EventHandler(this.mnuExploration_Click);
            // 
            // combatToolStripMenuItem
            // 
            this.combatToolStripMenuItem.Name = "combatToolStripMenuItem";
            this.combatToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.combatToolStripMenuItem.Text = "&Combat";
            this.combatToolStripMenuItem.Click += new System.EventHandler(this.combatToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 661);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.lblEquipment);
            this.Controls.Add(this.txtMainOutput);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.btn_Equip);
            this.Controls.Add(this.btnStats);
            this.Controls.Add(this.btnWest);
            this.Controls.Add(this.btnSouth);
            this.Controls.Add(this.btnEast);
            this.Controls.Add(this.btnNorth);
            this.Controls.Add(this.menuBar);
            this.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Text = "Main";
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Label lblEquipment;
        private System.Windows.Forms.RichTextBox txtMainOutput;
        private System.Windows.Forms.RichTextBox txtNotes;
        private System.Windows.Forms.Button btn_Equip;
        private System.Windows.Forms.Button btnStats;
        private System.Windows.Forms.Button btnWest;
        private System.Windows.Forms.Button btnSouth;
        private System.Windows.Forms.Button btnEast;
        private System.Windows.Forms.Button btnNorth;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem mnuMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuClearConsole;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuExploration;
        private System.Windows.Forms.ToolStripMenuItem combatToolStripMenuItem;
    }
}