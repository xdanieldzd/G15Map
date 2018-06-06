using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;

using G15Map.Parsers;
using G15Map.Parsers.Events;

namespace G15Map
{
	public partial class MainForm : Form
	{
		public static PrivateFontCollection PrivateFontCollection { get; private set; }

		GameHandler gameHandler;
		GameDrawing gameDrawing;

		Map selectedMap;
		IEventObject selectedEvent;

		int blocksWidth;

		Pen gridPen;

		public MainForm()
		{
			InitializeComponent();

			foreach (var control in Controls)
				(control as Control).Font = SystemFonts.MessageBoxFont;

			Text = Application.ProductName;
			tsslStatus.Text = "Ready";

			gameHandler = null;
			gameDrawing = null;

			selectedMap = null;
			selectedEvent = null;

			blocksWidth = 4;

			gridPen = new Pen(Color.FromArgb(128, Color.Gray));

			PrivateFontCollection = new PrivateFontCollection();
			AddFontFromResource(PrivateFontCollection, "G15Map.Data.PixelFont.smallest_pixel-7.ttf");

			spnlMap.Resize += (s, e) => { spnlMap.Refresh(); };
			spnlBlocks.Resize += (s, e) => { spnlBlocks.Refresh(); };

			pbBlocks.MouseDown += (s, e) => { pbBlocks.Parent?.Focus(); };

			showEventOverlayToolStripMenuItem.CheckedChanged += (s, e) => { pbMap.Invalidate(); pbBlocks.Invalidate(); };
			showGridOverlayToolStripMenuItem.CheckedChanged += (s, e) => { pbMap.Invalidate(); pbBlocks.Invalidate(); };
			useNighttimePalettesToolStripMenuItem.CheckedChanged += (s, e) => { pbMap.Invalidate(); pbBlocks.Invalidate(); };
			enableZoomToolStripMenuItem.CheckedChanged += (s, e) => { LoadMap(selectedMap); pbMap.Invalidate(); pbBlocks.Invalidate(); };
#if DEBUG
			if (Environment.MachineName == "RIN-CORE")
			{
				LoadROM(@"D:\Games\Game Boy & Advance\Pokemon GS Spaceworld 1997 Demos\Pokémon Gold - Spaceworld 1997 Demo (Debug) (Header Fixed).sgb");
				//tilesetViewerToolStripMenuItem_Click(tilesetViewerToolStripMenuItem, EventArgs.Empty);
				//tvMaps.SelectedNode = tvMaps.Nodes[0].Nodes[3];
			}
#endif
		}

		private void LoadROM(string path)
		{
			using (System.IO.BinaryReader reader = new System.IO.BinaryReader(System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite)))
			{
				gameHandler = new GameHandler(reader);
			}

			gameDrawing = new GameDrawing(gameHandler);

			CreateMapTree();

			pbMap.Invalidate();
			pbBlocks.Invalidate();

			tilesetViewerToolStripMenuItem.Enabled = true;

			Text = $"{Application.ProductName} - [{path}]";
			tsslStatus.Text = "ROM loaded";
		}

		private void LoadMap(Map map)
		{
			if (map != null)
			{
				var zoom = (enableZoomToolStripMenuItem.Checked ? 2 : 1);

				selectedMap = map;

				var mapBitmap = gameDrawing.GetMapBitmap(selectedMap, useNighttimePalettesToolStripMenuItem.Checked);
				pbMap.ClientSize = new Size(mapBitmap.Width * zoom, mapBitmap.Height * zoom);

				var blockBitmap = gameDrawing.GetTilesetBlocksBitmap(selectedMap.Tileset, selectedMap.Location, blocksWidth);
				pbBlocks.ClientSize = new Size(blockBitmap.Width * 2, blockBitmap.Height * 2);

				spnlBlocks.VerticalScroll.SmallChange = 64;
				spnlBlocks.VerticalScroll.LargeChange = 512;

				spnlMap.Refresh();
				spnlBlocks.Refresh();
			}
		}

		private void SelectEvent(IEventObject eventObject)
		{
			selectedEvent = eventObject;

			if (selectedEvent != null)
			{
				var treeNode = FindEventTreeNode(tvMaps.Nodes, eventObject);
				if (treeNode != null)
				{
					foreach (TreeNode node in treeNode.Parent.Parent.Nodes)
					{
						if (!node.Nodes.Contains(treeNode))
							node.Collapse();
					}
					tvMaps.SelectedNode = treeNode;
				}

				eventInformationToolStripMenuItem.Enabled = true;
				tsslStatus.Text = $"Selected {eventObject.GetType().Name} event at (X:{eventObject.X - ((eventObject is NPC) ? 4 : 0)}, Y:{eventObject.Y - ((eventObject is NPC) ? 4 : 0)})";
			}
			else
			{
				eventInformationToolStripMenuItem.Enabled = false;
				tsslStatus.Text = "No event selected";
			}

			pbMap.Invalidate();
			pbBlocks.Invalidate();
		}

		private TreeNode FindEventTreeNode(TreeNodeCollection nodes, IEventObject eventObject)
		{
			TreeNode foundNode = null;
			foreach (TreeNode node in nodes)
			{
				if ((node.Tag is System.Runtime.CompilerServices.ITuple tuple) && (tuple.Length == 2) && (tuple[0] is Map) && (tuple[1] == eventObject))
					return node;

				foundNode = FindEventTreeNode(node.Nodes, eventObject);
				if (foundNode != null) break;
			}
			return foundNode;
		}

		/* https://stackoverflow.com/a/23658552 */
		private void AddFontFromResource(PrivateFontCollection privateFontCollection, string fontResourceName)
		{
			var fontBytes = GetFontResourceBytes(typeof(Program).Assembly, fontResourceName);
			var fontData = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontBytes.Length);
			System.Runtime.InteropServices.Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
			privateFontCollection.AddMemoryFont(fontData, fontBytes.Length);
			System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontData);
		}

		private static byte[] GetFontResourceBytes(System.Reflection.Assembly assembly, string fontResourceName)
		{
			var resourceStream = assembly.GetManifestResourceStream(fontResourceName);
			if (resourceStream == null)
				throw new Exception(string.Format("Unable to find font '{0}' in embedded resources.", fontResourceName));
			var fontBytes = new byte[resourceStream.Length];
			resourceStream.Read(fontBytes, 0, (int)resourceStream.Length);
			resourceStream.Close();
			return fontBytes;
		}

		private void CreateMapTree()
		{
			tvMaps.BeginUpdate();

			tvMaps.Nodes.Clear();
			for (int i = 0; i < gameHandler.Maps.Length; i++)
			{
				var mapGroupNode = new TreeNode($"Group {i + 1:D2}") { Tag = gameHandler.Maps[i] };
				mapGroupNode.Expand();
				for (int j = 0; j < gameHandler.Maps[i].Length; j++)
				{
					var map = gameHandler.Maps[i][j];
					var debug = gameHandler.DebugWarps.FirstOrDefault(x => x.Target.MapGroup == (byte)(i + 1) && x.Target.MapID == (byte)(j + 1));
					var mapNode = new TreeNode(debug.Name ?? $"Map {j + 1:D2}") { Tag = map };

					var warpsNode = new TreeNode("Warps");
					for (int k = 0; k < map.SecondaryHeader.Warps.Length; k++)
						warpsNode.Nodes.Add(new TreeNode($"{k + 1:D2}") { Tag = (Map: map, Warp: map.SecondaryHeader.Warps[k]) });
					mapNode.Nodes.Add(warpsNode);

					var signsNode = new TreeNode("Signs");
					for (int k = 0; k < map.SecondaryHeader.Signs.Length; k++)
						signsNode.Nodes.Add(new TreeNode($"{k + 1:D2}") { Tag = (Map: map, Sign: map.SecondaryHeader.Signs[k]) });
					mapNode.Nodes.Add(signsNode);

					var npcsNode = new TreeNode("NPCs");
					for (int k = 0; k < map.SecondaryHeader.NPCs.Length; k++)
						npcsNode.Nodes.Add(new TreeNode($"{k + 1:D2}") { Tag = (Map: map, NPC: map.SecondaryHeader.NPCs[k]) });
					mapNode.Nodes.Add(npcsNode);

					mapGroupNode.Nodes.Add(mapNode);
				}
				tvMaps.Nodes.Add(mapGroupNode);
			}

			tvMaps.EndUpdate();
		}

		private void pbMap_Paint(object sender, PaintEventArgs e)
		{
			if (selectedMap == null) return;

			var zoom = (enableZoomToolStripMenuItem.Checked ? 2 : 1);

			var mapBitmap = gameDrawing.GetMapBitmap(selectedMap, useNighttimePalettesToolStripMenuItem.Checked);
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			e.Graphics.DrawImage(mapBitmap, 0, 0, mapBitmap.Width * zoom, mapBitmap.Height * zoom);

			if (showEventOverlayToolStripMenuItem.Checked)
				gameDrawing.DrawEventOverlay(e.Graphics, selectedMap, selectedEvent, zoom);

			if (showGridOverlayToolStripMenuItem.Checked)
				gameDrawing.DrawGridOverlay(
					e.Graphics,
					new Point(0, 0),
					new Size(mapBitmap.Width * zoom, mapBitmap.Height * zoom),
					Tileset.BlockDimensions * zoom,
					gridPen);
		}

		private void tvMaps_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag is Map map && map != null)
			{
				selectedEvent = null;
				eventInformationToolStripMenuItem.Enabled = false;
				saveMapImageToolStripMenuItem.Enabled = true;

				LoadMap(map);
				tsslStatus.Text = $"Loaded map {map.ParentGroup + 1:D2}:{Array.IndexOf(gameHandler.Maps[map.ParentGroup], map) + 1:D2} - Tileset {map.Tileset:D2}, {map.SecondaryHeader.NumWarps} warp(s), {map.SecondaryHeader.NumSigns} sign(s), {map.SecondaryHeader.NumNPCs} NPC(s)";
			}
			else if ((e.Node.Tag is System.Runtime.CompilerServices.ITuple tuple) && (tuple.Length == 2) && (tuple[0] is Map mapFromEvent) && (tuple[1] is IEventObject eventObject))
			{
				if (selectedMap != mapFromEvent)
					LoadMap(mapFromEvent);

				saveMapImageToolStripMenuItem.Enabled = true;

				SelectEvent(eventObject);
			}
			else
			{
				selectedEvent = null;
				eventInformationToolStripMenuItem.Enabled = false;
				saveMapImageToolStripMenuItem.Enabled = false;

				pbMap.Invalidate();
				pbBlocks.Invalidate();
			}
		}

		private void pbBlocks_Paint(object sender, PaintEventArgs e)
		{
			if (selectedMap == null) return;

			var blocksBitmap = gameDrawing.GetTilesetBlocksBitmap(selectedMap, useNighttimePalettesToolStripMenuItem.Checked, blocksWidth);
			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
			e.Graphics.DrawImage(blocksBitmap, 0, 0, blocksBitmap.Width * 2, blocksBitmap.Height * 2);

			if (showGridOverlayToolStripMenuItem.Checked)
				gameDrawing.DrawGridOverlay(
					e.Graphics,
					new Point(0, 0),
					new Size(blocksBitmap.Width * 2, blocksBitmap.Height * 2),
					Tileset.BlockDimensions * 2,
					gridPen);
		}

		private void eventInformationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowEventInformation(selectedEvent);
		}

		private void ShowEventInformation(IEventObject eventObject)
		{
			if (eventObject is Warp warp)
			{
				MessageBox.Show(
					$"Warp {Array.IndexOf(selectedMap.SecondaryHeader.Warps, warp) + 1:D2}:\n\n" +
					$"Target Warp Index: {warp.TargetWarpIndex:D2}\n" +
					$"Target Map Group: {warp.TargetMapGroup:D2}\n" +
					$"Target Map ID: {warp.TargetMapID:D2}\n" +
					$"(Unknown; maybe pointer: 0x{warp.UnknownMaybePointer:X4})\n",
					"Warp Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else if (eventObject is Sign sign)
			{
				MessageBox.Show(
					$"Sign {Array.IndexOf(selectedMap.SecondaryHeader.Signs, sign) + 1:D2}:\n\n" +
					$"Text ID: {sign.TextID:D2}\n" +
					$"(Unknown: 0x{sign.Unknown:X4})\n",
					"Sign Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else if (eventObject is NPC npc)
			{
				MessageBox.Show(
					$"NPC {Array.IndexOf(selectedMap.SecondaryHeader.NPCs, npc) + 1:D2}:\n\n" +
					$"Sprite: {npc.Sprite:D2}\n" +
					$"(Unknown: 0x{npc.Unknown1:X2})\n" +
					$"(Unknown: 0x{npc.Unknown2:X2})\n" +
					$"(Unknown: 0x{npc.Unknown3:X4})\n" +
					$"(Unknown: 0x{npc.Unknown4:X4})\n" +
					$"(Unknown: 0x{npc.Unknown5:X4})\n" +
					$"(Unknown: 0x{npc.Unknown6:X4})\n",
					"NPC Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// https://github.com/pret/pokegold-spaceworld/
			// https://github.com/2Tie/Elfs2World

			var version = new Version(Application.ProductVersion);

			MessageBox.Show(
				$"{Application.ProductName} v{version.Major}.{version.Minor} - Written 2018 by xdaniel\n" +
				"https://github.com/xdanieldzd/\n" +
				"\n" +
				"Some information gleaned from pret's WIP game disassembly and 2Tie's map viewer Elfs2World.\n" +
				"\n" +
				"Thanks and greetings to everyone involved with the game's release and documentation!\n",
				"About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ofdOpenROM.ShowDialog() == DialogResult.OK)
			{
				LoadROM(ofdOpenROM.FileName);
			}
		}

		private void saveMapImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			sfdSaveMapImage.FileName = $"Map {selectedMap.ParentGroup + 1:D2}-{Array.IndexOf(gameHandler.Maps[selectedMap.ParentGroup], selectedMap) + 1:D2}.png";

			if (sfdSaveMapImage.ShowDialog() == DialogResult.OK)
			{
				var mapBitmap = gameDrawing.GetMapBitmap(selectedMap, useNighttimePalettesToolStripMenuItem.Checked);

				using (var saveBitmap = new Bitmap(mapBitmap))
				using (var g = Graphics.FromImage(saveBitmap))
				{
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
					g.DrawImage(mapBitmap, 0, 0, mapBitmap.Width, mapBitmap.Height);

					if (showEventOverlayToolStripMenuItem.Checked)
						gameDrawing.DrawEventOverlay(g, selectedMap, null, 1);

					saveBitmap.Save(sfdSaveMapImage.FileName);
				}
			}
		}

		private void tilesetViewerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var tilesetForm = new TilesetForm())
			{
				tilesetForm.Initialize(gameHandler, gameDrawing);
				if (selectedMap != null)
					tilesetForm.SetTilesetAndPalette(selectedMap.Tileset, gameDrawing.GetPaletteIndex(selectedMap, useNighttimePalettesToolStripMenuItem.Checked));
				tilesetForm.ShowDialog();
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (gridPen != null)
			{
				gridPen.Dispose();
				gridPen = null;
			}
		}

		private void pbMap_MouseClick(object sender, MouseEventArgs e)
		{
			var zoom = (enableZoomToolStripMenuItem.Checked ? 2 : 1);
			var objectClickX = e.X / 16 / zoom;
			var objectClickY = e.Y / 16 / zoom;

			IEventObject[] newlySelected =
			{
				selectedMap.SecondaryHeader.Warps.FirstOrDefault(x => x.X == objectClickX && x.Y == objectClickY),
				selectedMap.SecondaryHeader.Signs.FirstOrDefault(x => x.X == objectClickX && x.Y == objectClickY),
				selectedMap.SecondaryHeader.NPCs.FirstOrDefault(x => x.X == (objectClickX + 4) && x.Y == (objectClickY + 4))
			};

			SelectEvent(newlySelected.Count(x => x != null) > 1 ? newlySelected.FirstOrDefault(x => (x != null && x != selectedEvent)) : newlySelected.FirstOrDefault(x => x != null));

			if (e.Button == MouseButtons.Left)
				ShowEventInformation(selectedEvent);
			else if ((e.Button == MouseButtons.Right) && (selectedEvent is Warp warp))
			{
				LoadMap(gameHandler.Maps[warp.TargetMapGroup - 1][warp.TargetMapID - 1]);
				SelectEvent(selectedMap.SecondaryHeader.Warps[warp.TargetWarpIndex - 1]);
			}
		}
	}
}
