namespace G15Map
{
	partial class TilesetForm
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
			this.btnClose = new System.Windows.Forms.Button();
			this.nudTilesetNo = new System.Windows.Forms.NumericUpDown();
			this.lblTilesetNo = new System.Windows.Forms.Label();
			this.pbTiles = new System.Windows.Forms.PictureBox();
			this.lblPaletteNo = new System.Windows.Forms.Label();
			this.nudPaletteNo = new System.Windows.Forms.NumericUpDown();
			this.chkShowOverlays = new System.Windows.Forms.CheckBox();
			this.chkEarlyCollisionMapping = new System.Windows.Forms.CheckBox();
			this.spnlBlocks = new G15Map.SelectablePanel();
			this.pbBlocks = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.nudTilesetNo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbTiles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPaletteNo)).BeginInit();
			this.spnlBlocks.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbBlocks)).BeginInit();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(212, 464);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// nudTilesetNo
			// 
			this.nudTilesetNo.Location = new System.Drawing.Point(68, 12);
			this.nudTilesetNo.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
			this.nudTilesetNo.Name = "nudTilesetNo";
			this.nudTilesetNo.Size = new System.Drawing.Size(70, 20);
			this.nudTilesetNo.TabIndex = 1;
			// 
			// lblTilesetNo
			// 
			this.lblTilesetNo.AutoSize = true;
			this.lblTilesetNo.Location = new System.Drawing.Point(12, 14);
			this.lblTilesetNo.Margin = new System.Windows.Forms.Padding(3, 0, 12, 0);
			this.lblTilesetNo.Name = "lblTilesetNo";
			this.lblTilesetNo.Size = new System.Drawing.Size(41, 13);
			this.lblTilesetNo.TabIndex = 3;
			this.lblTilesetNo.Text = "Tileset:";
			// 
			// pbTiles
			// 
			this.pbTiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbTiles.Location = new System.Drawing.Point(12, 64);
			this.pbTiles.Name = "pbTiles";
			this.pbTiles.Size = new System.Drawing.Size(275, 113);
			this.pbTiles.TabIndex = 4;
			this.pbTiles.TabStop = false;
			this.pbTiles.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTiles_Paint);
			// 
			// lblPaletteNo
			// 
			this.lblPaletteNo.AutoSize = true;
			this.lblPaletteNo.Location = new System.Drawing.Point(12, 40);
			this.lblPaletteNo.Margin = new System.Windows.Forms.Padding(3, 0, 12, 0);
			this.lblPaletteNo.Name = "lblPaletteNo";
			this.lblPaletteNo.Size = new System.Drawing.Size(43, 13);
			this.lblPaletteNo.TabIndex = 6;
			this.lblPaletteNo.Text = "Palette:";
			// 
			// nudPaletteNo
			// 
			this.nudPaletteNo.Location = new System.Drawing.Point(68, 38);
			this.nudPaletteNo.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
			this.nudPaletteNo.Name = "nudPaletteNo";
			this.nudPaletteNo.Size = new System.Drawing.Size(70, 20);
			this.nudPaletteNo.TabIndex = 5;
			// 
			// chkShowOverlays
			// 
			this.chkShowOverlays.AutoSize = true;
			this.chkShowOverlays.Checked = true;
			this.chkShowOverlays.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowOverlays.Location = new System.Drawing.Point(157, 13);
			this.chkShowOverlays.Name = "chkShowOverlays";
			this.chkShowOverlays.Size = new System.Drawing.Size(97, 17);
			this.chkShowOverlays.TabIndex = 9;
			this.chkShowOverlays.Text = "Show Overlays";
			this.chkShowOverlays.UseVisualStyleBackColor = true;
			// 
			// chkEarlyCollisionMapping
			// 
			this.chkEarlyCollisionMapping.AutoSize = true;
			this.chkEarlyCollisionMapping.Location = new System.Drawing.Point(157, 39);
			this.chkEarlyCollisionMapping.Name = "chkEarlyCollisionMapping";
			this.chkEarlyCollisionMapping.Size = new System.Drawing.Size(130, 17);
			this.chkEarlyCollisionMapping.TabIndex = 10;
			this.chkEarlyCollisionMapping.Text = "Assume Early Collision";
			this.chkEarlyCollisionMapping.UseVisualStyleBackColor = true;
			// 
			// spnlBlocks
			// 
			this.spnlBlocks.AlwaysShowHorizontalScroll = true;
			this.spnlBlocks.AlwaysShowVerticalScroll = true;
			this.spnlBlocks.AutoScroll = true;
			this.spnlBlocks.BackgroundImage = global::G15Map.Properties.Resources.BgEmpty;
			this.spnlBlocks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.spnlBlocks.Controls.Add(this.pbBlocks);
			this.spnlBlocks.DisableMouseWheel = true;
			this.spnlBlocks.DisableSmoothScrolling = true;
			this.spnlBlocks.Location = new System.Drawing.Point(12, 183);
			this.spnlBlocks.Name = "spnlBlocks";
			this.spnlBlocks.Size = new System.Drawing.Size(275, 275);
			this.spnlBlocks.TabIndex = 8;
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
			// TilesetForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(299, 499);
			this.Controls.Add(this.chkEarlyCollisionMapping);
			this.Controls.Add(this.chkShowOverlays);
			this.Controls.Add(this.spnlBlocks);
			this.Controls.Add(this.lblPaletteNo);
			this.Controls.Add(this.nudPaletteNo);
			this.Controls.Add(this.pbTiles);
			this.Controls.Add(this.lblTilesetNo);
			this.Controls.Add(this.nudTilesetNo);
			this.Controls.Add(this.btnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TilesetForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Tileset Viewer";
			((System.ComponentModel.ISupportInitialize)(this.nudTilesetNo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbTiles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPaletteNo)).EndInit();
			this.spnlBlocks.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbBlocks)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.NumericUpDown nudTilesetNo;
		private System.Windows.Forms.Label lblTilesetNo;
		private System.Windows.Forms.PictureBox pbTiles;
		private System.Windows.Forms.Label lblPaletteNo;
		private System.Windows.Forms.NumericUpDown nudPaletteNo;
		private System.Windows.Forms.CheckBox chkShowOverlays;
		private System.Windows.Forms.PictureBox pbBlocks;
		private SelectablePanel spnlBlocks;
		private System.Windows.Forms.CheckBox chkEarlyCollisionMapping;
	}
}