using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G15Map
{
	public partial class TilesetForm : Form
	{
		GameHandler gameHandler;
		GameDrawing gameDrawing;

		Bitmap tilesetBitmap, blockBitmap;
		int tilesetZoom, blockZoom;
		int tilesetX, tilesetY;

		Pen gridPen;

		public TilesetForm()
		{
			InitializeComponent();

			tilesetZoom = blockZoom = 2;
			tilesetX = tilesetY = 0;

			gridPen = new Pen(Color.FromArgb(128, Color.Gray));

			spnlBlocks.VerticalScroll.SmallChange = 64;
			spnlBlocks.VerticalScroll.LargeChange = 256;

			nudTilesetNo.ValueChanged += (s, e) => { GetAndDrawTileset(); GetAndDrawBlocks(); };
			nudPaletteNo.ValueChanged += (s, e) => { GetAndDrawTileset(); GetAndDrawBlocks(); };

			chkShowOverlays.CheckedChanged += (s, e) => { pbTiles.Invalidate(); pbBlocks.Invalidate(); };
			chkEarlyCollisionMapping.CheckedChanged += (s, e) => { pbTiles.Invalidate(); pbBlocks.Invalidate(); };
			chkShowGrids.CheckedChanged += (s, e) => { pbTiles.Invalidate(); pbBlocks.Invalidate(); };

			pbBlocks.MouseDown += (s, e) => { pbBlocks.Parent?.Focus(); };
		}

		public void Initialize(GameHandler gameHandler, GameDrawing gameDrawing)
		{
			this.gameHandler = gameHandler;
			this.gameDrawing = gameDrawing;

			nudTilesetNo.Maximum = (GameHandler.NumTilesets - 1);
			nudPaletteNo.Maximum = (GameHandler.NumPalettes - 1);

			GetAndDrawTileset();
			GetAndDrawBlocks();
		}

		public void SetTilesetAndPalette(byte tilesetIdx, byte paletteIdx)
		{
			nudTilesetNo.Value = tilesetIdx;
			nudPaletteNo.Value = paletteIdx;
		}

		private void GetAndDrawTileset()
		{
			tilesetBitmap = gameDrawing.GetTilesetBitmap((byte)nudTilesetNo.Value, (byte)nudPaletteNo.Value);

			tilesetX = (pbTiles.ClientSize.Width - (tilesetBitmap.Width * tilesetZoom)) / 2;
			tilesetY = (pbTiles.ClientSize.Height - (tilesetBitmap.Height * tilesetZoom)) / 2;

			pbTiles.Invalidate();

			spnlBlocks.Refresh();
		}

		private void GetAndDrawBlocks()
		{
			blockBitmap = gameDrawing.GetTilesetBlocksBitmap((byte)nudTilesetNo.Value, (byte)nudPaletteNo.Value, 4);
			pbBlocks.ClientSize = new Size(blockBitmap.Width * blockZoom, blockBitmap.Height * blockZoom);
			pbBlocks.Invalidate();

			spnlBlocks.Refresh();
		}

		private void TilesetForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (gridPen != null)
			{
				gridPen.Dispose();
				gridPen = null;
			}
		}

		private void pbTiles_Paint(object sender, PaintEventArgs e)
		{
			if (tilesetBitmap == null) return;

			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

			e.Graphics.DrawImage(
				tilesetBitmap,
				new Rectangle(tilesetX, tilesetY, tilesetBitmap.Width * tilesetZoom, tilesetBitmap.Height * tilesetZoom),
				new Rectangle(0, 0, tilesetBitmap.Width, tilesetBitmap.Height),
				GraphicsUnit.Pixel);

			if (chkShowOverlays.Checked)
				gameDrawing.DrawTilesetOverlay(e.Graphics, tilesetX, tilesetY, tilesetZoom);

			if (chkShowGrids.Checked)
				gameDrawing.DrawGridOverlay(
					e.Graphics,
					new Point(tilesetX, tilesetY),
					new Size((tilesetBitmap.Width * tilesetZoom), (tilesetBitmap.Height * tilesetZoom)),
					(Parsers.Tileset.TileDimensions * tilesetZoom),
					gridPen);
		}

		private void pbBlocks_Paint(object sender, PaintEventArgs e)
		{
			if (blockBitmap == null) return;

			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			e.Graphics.DrawImage(blockBitmap, new Rectangle(0, 0, blockBitmap.Width * blockZoom, blockBitmap.Height * blockZoom), new Rectangle(0, 0, blockBitmap.Width, blockBitmap.Height), GraphicsUnit.Pixel);

			if (chkShowOverlays.Checked)
				gameDrawing.DrawCollisionOverlay(e.Graphics, 0, 0, (byte)nudTilesetNo.Value, (blockBitmap.Width / 32), blockZoom, chkEarlyCollisionMapping.Checked);

			if (chkShowGrids.Checked)
				gameDrawing.DrawGridOverlay(e.Graphics,
					new Point(0, 0),
					new Size((sender as PictureBox).DisplayRectangle.Width, (sender as PictureBox).DisplayRectangle.Height),
					(Parsers.Tileset.BlockDimensions * blockZoom),
					gridPen);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
