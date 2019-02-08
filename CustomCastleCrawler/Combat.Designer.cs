namespace CustomCastleCrawler
{
    partial class frmCombat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCombat));
            this.grpEnemy = new System.Windows.Forms.GroupBox();
            this.lblEHLabel = new System.Windows.Forms.Label();
            this.lblEnemyHealth = new System.Windows.Forms.Label();
            this.grpPlayer = new System.Windows.Forms.GroupBox();
            this.lblPHLabel = new System.Windows.Forms.Label();
            this.lblPlayerHealth = new System.Windows.Forms.Label();
            this.lblPHStam = new System.Windows.Forms.Label();
            this.lblPlayerStamina = new System.Windows.Forms.Label();
            this.lblEscape = new System.Windows.Forms.Label();
            this.lblHeal = new System.Windows.Forms.Label();
            this.lblBlock = new System.Windows.Forms.Label();
            this.btnEscape = new System.Windows.Forms.Button();
            this.btnBlock = new System.Windows.Forms.Button();
            this.btnHeal = new System.Windows.Forms.Button();
            this.lblAttack = new System.Windows.Forms.Label();
            this.btnAttack = new System.Windows.Forms.Button();
            this.txtCombatLog = new System.Windows.Forms.RichTextBox();
            this.grpEnemy.SuspendLayout();
            this.grpPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpEnemy
            // 
            this.grpEnemy.Controls.Add(this.lblEHLabel);
            this.grpEnemy.Controls.Add(this.lblEnemyHealth);
            this.grpEnemy.Font = new System.Drawing.Font("Britannic Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpEnemy.Location = new System.Drawing.Point(386, 24);
            this.grpEnemy.Name = "grpEnemy";
            this.grpEnemy.Size = new System.Drawing.Size(211, 123);
            this.grpEnemy.TabIndex = 39;
            this.grpEnemy.TabStop = false;
            this.grpEnemy.Text = "Seath the Scaleless";
            // 
            // lblEHLabel
            // 
            this.lblEHLabel.AutoSize = true;
            this.lblEHLabel.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEHLabel.Location = new System.Drawing.Point(13, 30);
            this.lblEHLabel.Name = "lblEHLabel";
            this.lblEHLabel.Size = new System.Drawing.Size(58, 17);
            this.lblEHLabel.TabIndex = 19;
            this.lblEHLabel.Text = "Health:";
            // 
            // lblEnemyHealth
            // 
            this.lblEnemyHealth.AutoSize = true;
            this.lblEnemyHealth.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnemyHealth.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblEnemyHealth.Location = new System.Drawing.Point(92, 30);
            this.lblEnemyHealth.Name = "lblEnemyHealth";
            this.lblEnemyHealth.Size = new System.Drawing.Size(85, 17);
            this.lblEnemyHealth.TabIndex = 22;
            this.lblEnemyHealth.Text = "900/1000";
            // 
            // grpPlayer
            // 
            this.grpPlayer.Controls.Add(this.lblPHLabel);
            this.grpPlayer.Controls.Add(this.lblPlayerHealth);
            this.grpPlayer.Controls.Add(this.lblPHStam);
            this.grpPlayer.Controls.Add(this.lblPlayerStamina);
            this.grpPlayer.Font = new System.Drawing.Font("Britannic Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPlayer.Location = new System.Drawing.Point(15, 24);
            this.grpPlayer.Name = "grpPlayer";
            this.grpPlayer.Size = new System.Drawing.Size(211, 123);
            this.grpPlayer.TabIndex = 38;
            this.grpPlayer.TabStop = false;
            this.grpPlayer.Text = "Player";
            // 
            // lblPHLabel
            // 
            this.lblPHLabel.AutoSize = true;
            this.lblPHLabel.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPHLabel.Location = new System.Drawing.Point(6, 30);
            this.lblPHLabel.Name = "lblPHLabel";
            this.lblPHLabel.Size = new System.Drawing.Size(58, 17);
            this.lblPHLabel.TabIndex = 19;
            this.lblPHLabel.Text = "Health:";
            // 
            // lblPlayerHealth
            // 
            this.lblPlayerHealth.AutoSize = true;
            this.lblPlayerHealth.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerHealth.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblPlayerHealth.Location = new System.Drawing.Point(95, 30);
            this.lblPlayerHealth.Name = "lblPlayerHealth";
            this.lblPlayerHealth.Size = new System.Drawing.Size(85, 17);
            this.lblPlayerHealth.TabIndex = 22;
            this.lblPlayerHealth.Text = "900/1000";
            // 
            // lblPHStam
            // 
            this.lblPHStam.AutoSize = true;
            this.lblPHStam.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPHStam.Location = new System.Drawing.Point(6, 65);
            this.lblPHStam.Name = "lblPHStam";
            this.lblPHStam.Size = new System.Drawing.Size(69, 17);
            this.lblPHStam.TabIndex = 23;
            this.lblPHStam.Text = "Stamina:";
            // 
            // lblPlayerStamina
            // 
            this.lblPlayerStamina.AutoSize = true;
            this.lblPlayerStamina.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerStamina.ForeColor = System.Drawing.Color.Orange;
            this.lblPlayerStamina.Location = new System.Drawing.Point(105, 65);
            this.lblPlayerStamina.Name = "lblPlayerStamina";
            this.lblPlayerStamina.Size = new System.Drawing.Size(55, 17);
            this.lblPlayerStamina.TabIndex = 24;
            this.lblPlayerStamina.Text = "40/80";
            // 
            // lblEscape
            // 
            this.lblEscape.AutoSize = true;
            this.lblEscape.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEscape.Location = new System.Drawing.Point(515, 458);
            this.lblEscape.Name = "lblEscape";
            this.lblEscape.Size = new System.Drawing.Size(58, 17);
            this.lblEscape.TabIndex = 37;
            this.lblEscape.Text = "Escape";
            // 
            // lblHeal
            // 
            this.lblHeal.AutoSize = true;
            this.lblHeal.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeal.Location = new System.Drawing.Point(359, 458);
            this.lblHeal.Name = "lblHeal";
            this.lblHeal.Size = new System.Drawing.Size(39, 17);
            this.lblHeal.TabIndex = 36;
            this.lblHeal.Text = "Heal";
            // 
            // lblBlock
            // 
            this.lblBlock.AutoSize = true;
            this.lblBlock.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlock.Location = new System.Drawing.Point(200, 458);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.Size = new System.Drawing.Size(47, 17);
            this.lblBlock.TabIndex = 35;
            this.lblBlock.Text = "Block";
            // 
            // btnEscape
            // 
            this.btnEscape.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEscape.BackgroundImage")));
            this.btnEscape.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEscape.FlatAppearance.BorderSize = 0;
            this.btnEscape.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEscape.Location = new System.Drawing.Point(492, 498);
            this.btnEscape.Name = "btnEscape";
            this.btnEscape.Size = new System.Drawing.Size(100, 100);
            this.btnEscape.TabIndex = 34;
            this.btnEscape.UseVisualStyleBackColor = true;
            this.btnEscape.Click += new System.EventHandler(this.btnEscape_Click);
            // 
            // btnBlock
            // 
            this.btnBlock.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBlock.BackgroundImage")));
            this.btnBlock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBlock.FlatAppearance.BorderSize = 0;
            this.btnBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlock.Location = new System.Drawing.Point(174, 498);
            this.btnBlock.Name = "btnBlock";
            this.btnBlock.Size = new System.Drawing.Size(100, 100);
            this.btnBlock.TabIndex = 33;
            this.btnBlock.UseVisualStyleBackColor = true;
            this.btnBlock.Click += new System.EventHandler(this.btnBlock_Click);
            // 
            // btnHeal
            // 
            this.btnHeal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHeal.BackgroundImage")));
            this.btnHeal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHeal.FlatAppearance.BorderSize = 0;
            this.btnHeal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeal.Location = new System.Drawing.Point(333, 498);
            this.btnHeal.Name = "btnHeal";
            this.btnHeal.Size = new System.Drawing.Size(100, 100);
            this.btnHeal.TabIndex = 32;
            this.btnHeal.UseVisualStyleBackColor = true;
            this.btnHeal.Click += new System.EventHandler(this.btnHeal_Click);
            // 
            // lblAttack
            // 
            this.lblAttack.AutoSize = true;
            this.lblAttack.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttack.Location = new System.Drawing.Point(40, 458);
            this.lblAttack.Name = "lblAttack";
            this.lblAttack.Size = new System.Drawing.Size(51, 17);
            this.lblAttack.TabIndex = 31;
            this.lblAttack.Text = "Attack";
            // 
            // btnAttack
            // 
            this.btnAttack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAttack.BackgroundImage")));
            this.btnAttack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAttack.FlatAppearance.BorderSize = 0;
            this.btnAttack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAttack.Location = new System.Drawing.Point(15, 498);
            this.btnAttack.Name = "btnAttack";
            this.btnAttack.Size = new System.Drawing.Size(100, 100);
            this.btnAttack.TabIndex = 30;
            this.btnAttack.UseVisualStyleBackColor = true;
            this.btnAttack.Click += new System.EventHandler(this.btnAttack_Click);
            // 
            // txtCombatLog
            // 
            this.txtCombatLog.Enabled = false;
            this.txtCombatLog.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCombatLog.Location = new System.Drawing.Point(15, 153);
            this.txtCombatLog.Name = "txtCombatLog";
            this.txtCombatLog.Size = new System.Drawing.Size(578, 284);
            this.txtCombatLog.TabIndex = 29;
            this.txtCombatLog.Text = "";
            // 
            // frmCombat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 622);
            this.Controls.Add(this.grpEnemy);
            this.Controls.Add(this.grpPlayer);
            this.Controls.Add(this.lblEscape);
            this.Controls.Add(this.lblHeal);
            this.Controls.Add(this.lblBlock);
            this.Controls.Add(this.btnEscape);
            this.Controls.Add(this.btnBlock);
            this.Controls.Add(this.btnHeal);
            this.Controls.Add(this.lblAttack);
            this.Controls.Add(this.btnAttack);
            this.Controls.Add(this.txtCombatLog);
            this.Font = new System.Drawing.Font("Britannic Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmCombat";
            this.Text = "Combat";
            this.grpEnemy.ResumeLayout(false);
            this.grpEnemy.PerformLayout();
            this.grpPlayer.ResumeLayout(false);
            this.grpPlayer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpEnemy;
        private System.Windows.Forms.Label lblEHLabel;
        private System.Windows.Forms.Label lblEnemyHealth;
        private System.Windows.Forms.GroupBox grpPlayer;
        private System.Windows.Forms.Label lblPHLabel;
        private System.Windows.Forms.Label lblPlayerHealth;
        private System.Windows.Forms.Label lblPHStam;
        private System.Windows.Forms.Label lblPlayerStamina;
        private System.Windows.Forms.Label lblEscape;
        private System.Windows.Forms.Label lblHeal;
        private System.Windows.Forms.Label lblBlock;
        private System.Windows.Forms.Button btnEscape;
        private System.Windows.Forms.Button btnBlock;
        private System.Windows.Forms.Button btnHeal;
        private System.Windows.Forms.Label lblAttack;
        private System.Windows.Forms.Button btnAttack;
        private System.Windows.Forms.RichTextBox txtCombatLog;
    }
}