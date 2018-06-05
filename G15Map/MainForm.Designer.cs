namespace G15Map
{
	partial class MainForm
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openROMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.saveMapImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.objectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showObjectOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.useNighttimePalettesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.enableZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.ofdOpenROM = new System.Windows.Forms.OpenFileDialog();
			this.sfdSaveMapImage = new System.Windows.Forms.SaveFileDialog();
			this.tvMaps = new G15Map.TreeViewEx();
			this.spnlMap = new G15Map.SelectablePanel();
			this.pbMap = new System.Windows.Forms.PictureBox();
			this.spnlBlocks = new G15Map.SelectablePanel();
			this.pbBlocks = new System.Windows.Forms.PictureBox();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.spnlMap.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbMap)).BeginInit();
			this.spnlBlocks.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbBlocks)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.objectsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(1134, 24);
			this.menuStrip.TabIndex = 6;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openROMToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveMapImageToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openROMToolStripMenuItem
			// 
			this.openROMToolStripMenuItem.Name = "openROMToolStripMenuItem";
			this.openROMToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.openROMToolStripMenuItem.Text = "&Open ROM...";
			this.openROMToolStripMenuItem.Click += new System.EventHandler(this.openROMToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(167, 6);
			// 
			// saveMapImageToolStripMenuItem
			// 
			this.saveMapImageToolStripMenuItem.Enabled = false;
			this.saveMapImageToolStripMenuItem.Name = "saveMapImageToolStripMenuItem";
			this.saveMapImageToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.saveMapImageToolStripMenuItem.Text = "&Save Map Image...";
			this.saveMapImageToolStripMenuItem.Click += new System.EventHandler(this.saveMapImageToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(167, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// objectsToolStripMenuItem
			// 
			this.objectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showInformationToolStripMenuItem});
			this.objectsToolStripMenuItem.Name = "objectsToolStripMenuItem";
			this.objectsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.objectsToolStripMenuItem.Text = "&Objects";
			// 
			// showInformationToolStripMenuItem
			// 
			this.showInformationToolStripMenuItem.Enabled = false;
			this.showInformationToolStripMenuItem.Name = "showInformationToolStripMenuItem";
			this.showInformationToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.showInformationToolStripMenuItem.Text = "Show &Information...";
			this.showInformationToolStripMenuItem.Click += new System.EventHandler(this.showInformationToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showObjectOverlayToolStripMenuItem,
            this.useNighttimePalettesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.enableZoomToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "&Options";
			// 
			// showObjectOverlayToolStripMenuItem
			// 
			this.showObjectOverlayToolStripMenuItem.Checked = true;
			this.showObjectOverlayToolStripMenuItem.CheckOnClick = true;
			this.showObjectOverlayToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showObjectOverlayToolStripMenuItem.Name = "showObjectOverlayToolStripMenuItem";
			this.showObjectOverlayToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.showObjectOverlayToolStripMenuItem.Text = "Show Object Overlay";
			// 
			// useNighttimePalettesToolStripMenuItem
			// 
			this.useNighttimePalettesToolStripMenuItem.CheckOnClick = true;
			this.useNighttimePalettesToolStripMenuItem.Name = "useNighttimePalettesToolStripMenuItem";
			this.useNighttimePalettesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.useNighttimePalettesToolStripMenuItem.Text = "Use &Nighttime Palettes";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(191, 6);
			// 
			// enableZoomToolStripMenuItem
			// 
			this.enableZoomToolStripMenuItem.Checked = true;
			this.enableZoomToolStripMenuItem.CheckOnClick = true;
			this.enableZoomToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.enableZoomToolStripMenuItem.Name = "enableZoomToolStripMenuItem";
			this.enableZoomToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.enableZoomToolStripMenuItem.Text = "Enable &Zoom";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus});
			this.statusStrip.Location = new System.Drawing.Point(0, 609);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1134, 22);
			this.statusStrip.TabIndex = 7;
			this.statusStrip.Text = "statusStrip1";
			// 
			// tsslStatus
			// 
			this.tsslStatus.Name = "tsslStatus";
			this.tsslStatus.Size = new System.Drawing.Size(1055, 17);
			this.tsslStatus.Spring = true;
			this.tsslStatus.Text = "---";
			this.tsslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ofdOpenROM
			// 
			this.ofdOpenROM.Filter = "Super Game Boy ROMs (*.sgb)|*.sgb|All Files (*.*)|*.*";
			// 
			// sfdSaveMapImage
			// 
			this.sfdSaveMapImage.Filter = "Image Files (*.png; *.bmp)|*.png;*.bmp";
			// 
			// tvMaps
			// 
			this.tvMaps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tvMaps.HideSelection = false;
			this.tvMaps.Location = new System.Drawing.Point(12, 27);
			this.tvMaps.Name = "tvMaps";
			this.tvMaps.Size = new System.Drawing.Size(164, 579);
			this.tvMaps.TabIndex = 1;
			this.tvMaps.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMaps_AfterSelect);
			// 
			// spnlMap
			// 
			this.spnlMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.spnlMap.AutoScroll = true;
			this.spnlMap.BackgroundImage = global::G15Map.Properties.Resources.BgEmpty;
			this.spnlMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.spnlMap.Controls.Add(this.pbMap);
			this.spnlMap.DisableMouseWheel = true;
			this.spnlMap.Location = new System.Drawing.Point(182, 28);
			this.spnlMap.Name = "spnlMap";
			this.spnlMap.Size = new System.Drawing.Size(659, 578);
			this.spnlMap.TabIndex = 5;
			this.spnlMap.TabStop = true;
			// 
			// pbMap
			// 
			this.pbMap.BackColor = System.Drawing.Color.Transparent;
			this.pbMap.Location = new System.Drawing.Point(0, 0);
			this.pbMap.Margin = new System.Windows.Forms.Padding(0);
			this.pbMap.Name = "pbMap";
			this.pbMap.Size = new System.Drawing.Size(32, 32);
			this.pbMap.TabIndex = 0;
			this.pbMap.TabStop = false;
			this.pbMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMap_Paint);
			// 
			// spnlBlocks
			// 
			this.spnlBlocks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.spnlBlocks.AutoScroll = true;
			this.spnlBlocks.BackgroundImage = global::G15Map.Properties.Resources.BgEmpty;
			this.spnlBlocks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.spnlBlocks.Controls.Add(this.pbBlocks);
			this.spnlBlocks.DisableMouseWheel = true;
			this.spnlBlocks.Location = new System.Drawing.Point(847, 29);
			this.spnlBlocks.Name = "spnlBlocks";
			this.spnlBlocks.Size = new System.Drawing.Size(275, 577);
			this.spnlBlocks.TabIndex = 2;
			this.spnlBlocks.TabStop = true;
			// 
			// pbBlocks
			// 
			this.pbBlocks.BackColor = System.Drawing.Color.Transparent;
			this.pbBlocks.Location = new System.Drawing.Point(0, 0);
			this.pbBlocks.Margin = new System.Windows.Forms.Padding(0);
			this.pbBlocks.Name = "pbBlocks";
			this.pbBlocks.Size = new System.Drawing.Size(32, 32);
			this.pbBlocks.TabIndex = 0;
			this.pbBlocks.TabStop = false;
			this.pbBlocks.Paint += new System.Windows.Forms.PaintEventHandler(this.pbBlocks_Paint);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1134, 631);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.tvMaps);
			this.Controls.Add(this.spnlMap);
			this.Controls.Add(this.spnlBlocks);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.spnlMap.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbMap)).EndInit();
			this.spnlBlocks.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbBlocks)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbMap;
		private SelectablePanel spnlBlocks;
		private System.Windows.Forms.PictureBox pbBlocks;
		private SelectablePanel spnlMap;
		private TreeViewEx tvMaps;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openROMToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
		private System.Windows.Forms.ToolStripMenuItem objectsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showInformationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showObjectOverlayToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem useNighttimePalettesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem enableZoomToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem saveMapImageToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.OpenFileDialog ofdOpenROM;
		private System.Windows.Forms.SaveFileDialog sfdSaveMapImage;
	}
}

