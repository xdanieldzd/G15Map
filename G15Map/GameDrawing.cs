﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

using G15Map.Parsers;

namespace G15Map
{
	public class GameDrawing : IDisposable
	{
		GameHandler gameHandler;

		Dictionary<(byte, byte), Bitmap> tilesetTileBitmaps;
		Dictionary<(byte, byte), Bitmap> tilesetBlockBitmaps;
		Dictionary<(Map, byte), Bitmap> mapBitmaps;

		// TODO: try and make better?
		static readonly Dictionary<byte, SolidBrush> collisionBrushesEarly = new Dictionary<byte, SolidBrush>()
		{
			{ 0x00, new SolidBrush(Color.FromArgb(128, Color.Green)) },		// floor
			{ 0x01, new SolidBrush(Color.FromArgb(128, Color.Red)) },		// solid
			{ 0x11, new SolidBrush(Color.FromArgb(128, Color.Orange)) },	// ledge
			{ 0x21, new SolidBrush(Color.FromArgb(128, Color.Blue)) },		// water
			{ 0x2C, new SolidBrush(Color.FromArgb(128, Color.Blue)) },		// water
			{ 0x82, new SolidBrush(Color.FromArgb(128, Color.DarkGreen)) },	// grass
			{ 0x8C, new SolidBrush(Color.FromArgb(128, Color.DarkGreen)) },	// grass
			{ 0x62, new SolidBrush(Color.FromArgb(128, Color.Purple)) },	// ladder
		};

		static readonly Dictionary<byte, SolidBrush> collisionBrushesProto = new Dictionary<byte, SolidBrush>()
		{
			{ 0x00, new SolidBrush(Color.FromArgb(128, Color.Green)) },		// floor
			{ 0x07, new SolidBrush(Color.FromArgb(128, Color.Red)) },		// solid
			{ 0x21, new SolidBrush(Color.FromArgb(128, Color.Blue)) },		// water
			{ 0x29, new SolidBrush(Color.FromArgb(128, Color.Blue)) },		// water
			{ 0x18, new SolidBrush(Color.FromArgb(128, Color.DarkGreen)) },	// grass
		};

		static readonly SolidBrush genericBrush = new SolidBrush(Color.FromArgb(128, Color.Black));

		static readonly SolidBrush warpBrush = new SolidBrush(Color.FromArgb(128, Color.Blue));
		static readonly SolidBrush signBrush = new SolidBrush(Color.FromArgb(128, Color.DarkRed));
		static readonly SolidBrush npcBrush = new SolidBrush(Color.FromArgb(128, Color.DarkGreen));

		readonly byte bgPaletteRegister;

		public GameDrawing(GameHandler gameHandler)
		{
			this.gameHandler = gameHandler;

			tilesetTileBitmaps = new Dictionary<(byte, byte), Bitmap>();
			tilesetBlockBitmaps = new Dictionary<(byte, byte), Bitmap>();
			mapBitmaps = new Dictionary<(Map, byte), Bitmap>();

			bgPaletteRegister = 0xD8;
		}

		~GameDrawing()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (var brush in collisionBrushesEarly.Where(x => x.Value != null))
					brush.Value.Dispose();
				collisionBrushesEarly.Clear();

				foreach (var brush in collisionBrushesProto.Where(x => x.Value != null))
					brush.Value.Dispose();
				collisionBrushesProto.Clear();

				genericBrush.Dispose();
				warpBrush.Dispose();
				signBrush.Dispose();
				npcBrush.Dispose();
			}
		}

		public Bitmap GetTilesetBitmap(byte tilesetIdx, byte paletteIdx)
		{
			if (tilesetTileBitmaps.ContainsKey((tilesetIdx, paletteIdx)))
			{
				return tilesetTileBitmaps[(tilesetIdx, paletteIdx)];
			}
			else
			{
				Tileset tileset = gameHandler.Tilesets[tilesetIdx];
				Palette palette = gameHandler.Palettes[paletteIdx];

				Bitmap bitmap = new Bitmap(128, 96);
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

				byte[] pixelData = new byte[bitmapData.Height * bitmapData.Stride];
				Marshal.Copy(bitmapData.Scan0, pixelData, 0, pixelData.Length);

				for (int t = 0, p = 0; t < tileset.TileData.Length; t += 2)
				{
					int dx = (((t >> 4) << 3) % bitmap.Width);
					int dy = ((((t >> 1) % 8) + ((t >> 8) << 3)) % bitmap.Height);

					for (int x = 0; x < 8; x++)
					{
						var raw = (((((tileset.TileData[t] >> (7 - x)) & 0x01) << 1) | (tileset.TileData[t + 1] >> (7 - x)) & 0x01) & 0x03);
						var color = ((bgPaletteRegister >> (raw << 1)) & 0x03);

						p = ((dy * bitmap.Width) + (dx + x)) * (Image.GetPixelFormatSize(bitmap.PixelFormat) / 8);

						if (p >= pixelData.Length) continue;

						pixelData[p + 0] = palette.Colors[color].B;
						pixelData[p + 1] = palette.Colors[color].G;
						pixelData[p + 2] = palette.Colors[color].R;
						pixelData[p + 3] = palette.Colors[color].A;
					}
				}

				Marshal.Copy(pixelData, 0, bitmapData.Scan0, pixelData.Length);
				bitmap.UnlockBits(bitmapData);

				return (tilesetTileBitmaps[(tilesetIdx, paletteIdx)] = bitmap);
			}
		}

		private byte GetPaletteIndex(Map map, bool isNighttime)
		{
			// code starts at ROM2:54E5

			byte paletteIdx;
			switch (map.Type)
			{
				case 0x02:
					paletteIdx = (byte)(isNighttime ? 0x0D : 0x00);
					break;
				case 0x04:
					paletteIdx = (byte)(isNighttime ? 0x0D : 0x0C);
					break;
				case 0x06:
					paletteIdx = 0x03;
					break;
				case 0x05:
					paletteIdx = 0x04;
					break;
				case 0x03:
					// NOTE: hatsudensho, ajito group IDs exceed valid palette indices (results in SGB PAL_SET w/ palette D5 => broken rainbow palette in-game)
					paletteIdx = (byte)(isNighttime ? 0x0D : gameHandler.MapGroupPalettes[(map.ParentGroup + 1) % gameHandler.MapGroupPalettes.Length]);
					break;

				default:
					paletteIdx = gameHandler.MapGroupPalettes[(map.ParentGroup + 1) % gameHandler.MapGroupPalettes.Length];
					break;
			}
			return paletteIdx;
		}

		public Bitmap GetTilesetBlocksBitmap(Map map, bool isNighttime)
		{
			return GetTilesetBlocksBitmap(map.Tileset, GetPaletteIndex(map, isNighttime));
		}

		public Bitmap GetTilesetBlocksBitmap(byte tilesetIdx, byte paletteIdx)
		{
			if (tilesetBlockBitmaps.ContainsKey((tilesetIdx, paletteIdx)))
			{
				return tilesetBlockBitmaps[(tilesetIdx, paletteIdx)];
			}
			else
			{
				Tileset tileset = gameHandler.Tilesets[tilesetIdx];

				Bitmap tilesetBitmap = GetTilesetBitmap(tilesetIdx, paletteIdx);
				Bitmap bitmap = new Bitmap(32, 8192);

				using (var g = Graphics.FromImage(bitmap))
				{
					for (int b = 0; b < tileset.BlockData.Length; b += 16)
					{
						for (int t = 0; t < 16; t++)
						{
							var tile = tileset.BlockData[b + t];
							var tsy = (tile / 16) * 8;
							var tsx = (tile % 16) * 8;

							var bby = ((b / 16) * 32) + (((t / 4)) * 8);
							var bbx = (t % 4) * 8;

							g.DrawImage(tilesetBitmap, new Rectangle(bbx, bby, 8, 8), new Rectangle(tsx, tsy, 8, 8), GraphicsUnit.Pixel);
						}
					}
				}

				return (tilesetBlockBitmaps[(tilesetIdx, paletteIdx)] = bitmap);
			}
		}

		public Bitmap GetMapBitmap(Map map, bool isNighttime)
		{
			var paletteIdx = GetPaletteIndex(map, isNighttime);
			if (mapBitmaps.ContainsKey((map, paletteIdx)))
			{
				return mapBitmaps[(map, paletteIdx)];
			}
			else
			{
				if (map == null || !map.IsValid) return new Bitmap(32, 32);

				Bitmap blocksBitmap = GetTilesetBlocksBitmap(map, isNighttime);
				Bitmap bitmap = new Bitmap(map.PrimaryHeader.Width * 32, map.PrimaryHeader.Height * 32);

				using (var g = Graphics.FromImage(bitmap))
				{
					for (int y = 0; y < map.PrimaryHeader.Height; y++)
					{
						for (int x = 0; x < map.PrimaryHeader.Width; x++)
						{
							var value = map.MapData[(y * map.PrimaryHeader.Width) + x];
							g.DrawImage(blocksBitmap,
								new Rectangle(x * 32, y * 32, 32, 32),
								new Rectangle(0, value * 32, 32, 32),
								GraphicsUnit.Pixel);
						}
					}
				}

				return (mapBitmaps[(map, paletteIdx)] = bitmap);
			}
		}

		public void DrawObjectOverlay(Graphics g, Map map, IInteractiveObject interactive, int zoom)
		{
			g.SmoothingMode = SmoothingMode.None;
			g.TextContrast = 0;

			int step = (16 * zoom);
			using (var font = new Font(MainForm.PrivateFontCollection.Families[0], 7.0f))
			{
				foreach (var warp in map.SecondaryHeader.Warps)
				{
					var rect = new Rectangle(warp.X * step, warp.Y * step, step, step);
					g.FillRectangle(warpBrush, rect);
					if (zoom > 1)
						g.DrawString($"{warp.TargetMapGroup:D2}:{warp.TargetMapID:D2}\n{warp.TargetWarpIndex:D2}", font, Brushes.White, new Point(rect.X, rect.Y));
					if (warp == interactive)
						g.DrawRectangle(Pens.Red, rect);
				}

				foreach (var sign in map.SecondaryHeader.Signs)
				{
					var rect = new Rectangle(sign.X * step, sign.Y * step, step, step);
					g.FillRectangle(signBrush, rect);
					if (zoom > 1)
						g.DrawString($"{sign.TextID:D2}\n{sign.Unknown:D2}", font, Brushes.White, new Point(rect.X, rect.Y));
					if (sign == interactive)
						g.DrawRectangle(Pens.Red, rect);
				}

				foreach (var npc in map.SecondaryHeader.NPCs)
				{
					var rect = new Rectangle((npc.X - 4) * step, (npc.Y - 4) * step, step, step);
					g.FillRectangle(npcBrush, rect);
					if (zoom > 1)
						g.DrawString($"{npc.Sprite:D2}\n(...)", font, Brushes.White, new Point(rect.X, rect.Y));
					if (npc == interactive)
						g.DrawRectangle(Pens.Red, rect);
				}
			}
		}

		public void DrawCollisionOverlay(int tilesetIdx, bool useEarlyMapping, Graphics g)
		{
			Tileset tileset = gameHandler.Tilesets[tilesetIdx];

			g.InterpolationMode = InterpolationMode.NearestNeighbor;
			g.PixelOffsetMode = PixelOffsetMode.None;
			g.SmoothingMode = SmoothingMode.None;
			g.TextContrast = 0;

			var brushDict = (useEarlyMapping ? collisionBrushesEarly : collisionBrushesProto);

			using (var textBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255)))
			using (var font = new Font(MainForm.PrivateFontCollection.Families[0], 7.0f))
			{
				for (int c = 0; c < tileset.CollisionData.Length; c += 4)
				{
					for (int b = 0; b < 4; b++)
					{
						int x = (b % 2) * 16;
						int y = (b / 2) * 16;

						byte value = tileset.CollisionData[c + b];
						var brush = (brushDict.ContainsKey(value) ? brushDict[value] : genericBrush);
						g.FillRectangle(brush, new Rectangle(x, (c * 8) + y, 16, 16));
						g.DrawString($"{value:X2}", font, textBrush, new Point(x, (c * 8) + y));
					}
				}
			}
		}
	}
}
