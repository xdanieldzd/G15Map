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
using G15Map.Parsers.Events;

namespace G15Map
{
	public class GameDrawing : IDisposable
	{
		GameHandler gameHandler;

		static readonly Dictionary<byte, (SolidBrush Brush, string Name)> collisionTypesUsed = new Dictionary<byte, (SolidBrush Brush, string Name)>()
		{
			{ 0x00, (new SolidBrush(Color.FromArgb(128, Color.Green)), "Floor") },
			{ 0x05, (new SolidBrush(Color.FromArgb(128, Color.LawnGreen)), "Tree") },
			{ 0x07, (new SolidBrush(Color.FromArgb(128, Color.Red)), "Solid") },

			{ 0x12, (new SolidBrush(Color.FromArgb(128, Color.LawnGreen)), "Cut") },
			{ 0x18, (new SolidBrush(Color.FromArgb(128, Color.DarkGreen)), "Grass") },

			{ 0x21, (new SolidBrush(Color.FromArgb(128, Color.Blue)), "Water") },
			{ 0x22, (new SolidBrush(Color.FromArgb(128, Color.Blue)), "Water\nfall") },
			{ 0x29, (new SolidBrush(Color.FromArgb(128, Color.Blue)), "Water2") },

			{ 0x60, (new SolidBrush(Color.FromArgb(128, Color.IndianRed)), "Warp") },

			{ 0x71, (new SolidBrush(Color.FromArgb(128, Color.IndianRed)), "Door") },
			{ 0x73, (new SolidBrush(Color.FromArgb(128, Color.IndianRed)), "Door2") },

			{ 0x95, (new SolidBrush(Color.FromArgb(128, Color.Orange)), "Sign") },
		};

		static readonly Dictionary<byte, (SolidBrush Brush, string Name)> collisionTypesEarly = new Dictionary<byte, (SolidBrush Brush, string Name)>()
		{
			{ 0x00, (new SolidBrush(Color.FromArgb(128, Color.Green)), "Floor") },
			{ 0x01, (new SolidBrush(Color.FromArgb(128, Color.Red)), "Solid") },
			{ 0x11, (new SolidBrush(Color.FromArgb(128, Color.Orange)), "Ledge") },
			{ 0x21, (new SolidBrush(Color.FromArgb(128, Color.Blue)), "Water") },
			{ 0x2C, (new SolidBrush(Color.FromArgb(128, Color.Blue)), "Water2") },
			{ 0x82, (new SolidBrush(Color.FromArgb(128, Color.DarkGreen)), "Grass") },
			{ 0x8C, (new SolidBrush(Color.FromArgb(128, Color.DarkGreen)), "Grass2") },
			{ 0x62, (new SolidBrush(Color.FromArgb(128, Color.Purple)), "Ladder") },
		};

		static readonly SolidBrush genericBlackBrush = new SolidBrush(Color.FromArgb(128, Color.Black));
		static readonly SolidBrush genericGrayBrush = new SolidBrush(Color.FromArgb(128, Color.DarkSlateGray));

		static readonly SolidBrush warpBrush = new SolidBrush(Color.FromArgb(128, Color.Blue));
		static readonly SolidBrush signBrush = new SolidBrush(Color.FromArgb(128, Color.DarkRed));
		static readonly SolidBrush npcBrush = new SolidBrush(Color.FromArgb(128, Color.DarkGreen));
		static readonly SolidBrush debugWarpBrush = new SolidBrush(Color.FromArgb(128, Color.Orange));

		Dictionary<byte, Bitmap> commonMapTilesBitmaps;
		Dictionary<(byte Tileset, byte Palette), Bitmap> basicTilesetTileBitmaps;
		Dictionary<(byte Tileset, byte Palette, byte MapType), Bitmap> tilesetTileBitmaps;
		Dictionary<(byte Tileset, byte Palette, byte MapType, int WidthInBlocks), Bitmap> tilesetBlockBitmaps;
		Dictionary<(Map Map, byte Palette), Bitmap> mapBitmaps;

		readonly byte bgPaletteRegister;

		public GameDrawing(GameHandler gameHandler)
		{
			this.gameHandler = gameHandler;

			commonMapTilesBitmaps = new Dictionary<byte, Bitmap>();
			basicTilesetTileBitmaps = new Dictionary<(byte Tileset, byte Palette), Bitmap>();

			tilesetTileBitmaps = new Dictionary<(byte Tileset, byte Palette, byte MapType), Bitmap>();
			tilesetBlockBitmaps = new Dictionary<(byte Tileset, byte Palette, byte MapType, int WidthInBlocks), Bitmap>();
			mapBitmaps = new Dictionary<(Map Map, byte Palette), Bitmap>();

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
				foreach (var bitmap in commonMapTilesBitmaps.Where(x => x.Value != null)) bitmap.Value.Dispose();
				commonMapTilesBitmaps.Clear();
				foreach (var bitmap in basicTilesetTileBitmaps.Where(x => x.Value != null)) bitmap.Value.Dispose();
				basicTilesetTileBitmaps.Clear();

				foreach (var bitmap in tilesetTileBitmaps.Where(x => x.Value != null)) bitmap.Value.Dispose();
				tilesetTileBitmaps.Clear();
				foreach (var bitmap in tilesetBlockBitmaps.Where(x => x.Value != null)) bitmap.Value.Dispose();
				tilesetBlockBitmaps.Clear();
				foreach (var bitmap in mapBitmaps.Where(x => x.Value != null)) bitmap.Value.Dispose();
				mapBitmaps.Clear();

				foreach (var type in collisionTypesEarly.Where(x => x.Value.Brush != null)) type.Value.Brush.Dispose();
				collisionTypesEarly.Clear();

				foreach (var type in collisionTypesUsed.Where(x => x.Value.Brush != null)) type.Value.Brush.Dispose();
				collisionTypesUsed.Clear();

				genericBlackBrush.Dispose();
				genericGrayBrush.Dispose();

				warpBrush.Dispose();
				signBrush.Dispose();
				npcBrush.Dispose();
				debugWarpBrush.Dispose();
			}
		}

		private void ConvertTileDataToRGBA(byte[] tileData, Color[] palette, byte bgPaletteRegister, int width, int height, PixelFormat pixelFormat, ref byte[] pixelData)
		{
			int bytesPerPixel = (Image.GetPixelFormatSize(pixelFormat) / 8);

			for (int t = 0; t < tileData.Length; t += 2)
			{
				int dx = (((t >> 4) << 3) % width);
				int dy = ((((t >> 1) % 8) + ((t >> 8) << 3)) % height);

				for (int x = 0; x < 8; x++)
				{
					var raw = (((((tileData[t] >> (7 - x)) & 0x01) << 1) | (tileData[t + 1] >> (7 - x)) & 0x01) & 0x03);
					var color = ((bgPaletteRegister >> (raw << 1)) & 0x03);

					var offset = (((dy * width) + (dx + x)) * bytesPerPixel);
					if (offset >= pixelData.Length) continue;

					pixelData[offset + 0] = palette[color].B;
					pixelData[offset + 1] = palette[color].G;
					pixelData[offset + 2] = palette[color].R;
					pixelData[offset + 3] = palette[color].A;
				}
			}
		}

		private Bitmap GetCommonMapTilesBitmap(byte paletteIdx)
		{
			if (commonMapTilesBitmaps.ContainsKey(paletteIdx))
			{
				return commonMapTilesBitmaps[paletteIdx];
			}
			else
			{
				Palette palette = gameHandler.Palettes[paletteIdx];

				Bitmap bitmap = new Bitmap(128, 16);
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

				byte[] pixelData = new byte[bitmapData.Height * bitmapData.Stride];
				Marshal.Copy(bitmapData.Scan0, pixelData, 0, pixelData.Length);

				ConvertTileDataToRGBA(gameHandler.CommonMapTiles, palette.Colors, bgPaletteRegister, bitmap.Width, bitmap.Height, bitmap.PixelFormat, ref pixelData);

				Marshal.Copy(pixelData, 0, bitmapData.Scan0, pixelData.Length);
				bitmap.UnlockBits(bitmapData);

				return (commonMapTilesBitmaps[paletteIdx] = bitmap);
			}
		}

		private Bitmap GetBasicTilesetBitmap(byte tilesetIdx, byte paletteIdx)
		{
			if (basicTilesetTileBitmaps.ContainsKey((tilesetIdx, paletteIdx)))
			{
				return basicTilesetTileBitmaps[(tilesetIdx, paletteIdx)];
			}
			else
			{
				Tileset tileset = gameHandler.Tilesets[tilesetIdx];
				Palette palette = gameHandler.Palettes[paletteIdx];

				Bitmap bitmap = new Bitmap(128, 48);
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

				byte[] pixelData = new byte[bitmapData.Height * bitmapData.Stride];
				Marshal.Copy(bitmapData.Scan0, pixelData, 0, pixelData.Length);

				ConvertTileDataToRGBA(tileset.TileData, palette.Colors, bgPaletteRegister, bitmap.Width, bitmap.Height, bitmap.PixelFormat, ref pixelData);

				Marshal.Copy(pixelData, 0, bitmapData.Scan0, pixelData.Length);
				bitmap.UnlockBits(bitmapData);

				return (basicTilesetTileBitmaps[(tilesetIdx, paletteIdx)] = bitmap);
			}
		}

		public Bitmap GetTilesetBitmap(byte tilesetIdx, byte paletteIdx, byte mapType)
		{
			if (tilesetTileBitmaps.ContainsKey((tilesetIdx, paletteIdx, mapType)))
			{
				return tilesetTileBitmaps[(tilesetIdx, paletteIdx, mapType)];
			}
			else
			{
				Bitmap bitmap = new Bitmap(128, 48);

				using (Graphics g = Graphics.FromImage(bitmap))
				{
					if (IsCommonTilesNeeded(mapType, tilesetIdx))
					{
						g.DrawImage(GetCommonMapTilesBitmap(paletteIdx), 0, 0);
						g.DrawImage(GetBasicTilesetBitmap(tilesetIdx, paletteIdx), 0, 16);
					}
					else
					{
						g.DrawImage(GetBasicTilesetBitmap(tilesetIdx, paletteIdx), 0, 0);
					}
				}

				return (tilesetTileBitmaps[(tilesetIdx, paletteIdx, mapType)] = bitmap);
			}
		}

		public bool IsCommonTilesNeeded(byte mapType, byte tilesetIdx)
		{
			// https://github.com/pret/pokegold-spaceworld/blob/ece3adc39bad4f6bbd041a83ace7acb21dc586b7/home/map.asm#L1517
			// 00:2D29 -> reg A == map type, if A==(01,02) loadcommon; 00:2D31 -> reg A == tileset ID, if A==1B loadcommon

			return (mapType == 0x01 || mapType == 0x02 || tilesetIdx == 0x1B);
		}

		public byte GetPaletteIndex(Map map, bool isNighttime)
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

		public Bitmap GetTilesetBlocksBitmap(Map map, bool isNighttime, byte mapType, int widthInBlocks)
		{
			return GetTilesetBlocksBitmap(map.Tileset, GetPaletteIndex(map, isNighttime), mapType, widthInBlocks);
		}

		public Bitmap GetTilesetBlocksBitmap(byte tilesetIdx, byte paletteIdx, byte mapType, int widthInBlocks)
		{
			if (tilesetBlockBitmaps.ContainsKey((tilesetIdx, paletteIdx, mapType, widthInBlocks)))
			{
				return tilesetBlockBitmaps[(tilesetIdx, paletteIdx, mapType, widthInBlocks)];
			}
			else
			{
				var imageWidth = (Tileset.BlockDimensions * widthInBlocks);
				var imageHeight = (int)Math.Round((float)Tileset.BlockDimensions * ((Tileset.BlockDataSize / 16) / widthInBlocks), MidpointRounding.AwayFromZero);

				Tileset tileset = gameHandler.Tilesets[tilesetIdx];

				Bitmap tilesetBitmap = GetTilesetBitmap(tilesetIdx, paletteIdx, mapType);
				Bitmap bitmap = new Bitmap(imageWidth, imageHeight);

				using (var g = Graphics.FromImage(bitmap))
				{
					g.Clear(Color.DarkOrange);

					for (int b = 0, bn = 0; b < tileset.BlockData.Length; b += 16, bn++)
					{
						for (int t = 0; t < 16; t++)
						{
							var tile = tileset.BlockData[b + t];
							var tsy = (tile / 16) * Tileset.TileDimensions;
							var tsx = (tile % 16) * Tileset.TileDimensions;

							var bby = (((bn / widthInBlocks) * Tileset.BlockDimensions) + ((t / 4) * Tileset.TileDimensions));
							var bbx = (((bn % widthInBlocks) * Tileset.BlockDimensions) + ((t % 4) * Tileset.TileDimensions));

							g.DrawImage(
								tilesetBitmap,
								new Rectangle(bbx, bby, Tileset.TileDimensions, Tileset.TileDimensions),
								new Rectangle(tsx, tsy, Tileset.TileDimensions, Tileset.TileDimensions),
								GraphicsUnit.Pixel);
						}
					}
				}

				return (tilesetBlockBitmaps[(tilesetIdx, paletteIdx, mapType, widthInBlocks)] = bitmap);
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
				if (map == null || !map.IsValid) return new Bitmap(Tileset.BlockDimensions, Tileset.BlockDimensions);

				Bitmap blocksBitmap = GetTilesetBlocksBitmap(map, isNighttime, map.Type, 1);
				Bitmap bitmap = new Bitmap(map.PrimaryHeader.Width * Tileset.BlockDimensions, map.PrimaryHeader.Height * Tileset.BlockDimensions);

				using (var g = Graphics.FromImage(bitmap))
				{
					for (int y = 0; y < map.PrimaryHeader.Height; y++)
					{
						for (int x = 0; x < map.PrimaryHeader.Width; x++)
						{
							var value = map.MapData[(y * map.PrimaryHeader.Width) + x];
							g.DrawImage(blocksBitmap,
								new Rectangle(x * Tileset.BlockDimensions, y * Tileset.BlockDimensions, Tileset.BlockDimensions, Tileset.BlockDimensions),
								new Rectangle(0, value * Tileset.BlockDimensions, Tileset.BlockDimensions, Tileset.BlockDimensions),
								GraphicsUnit.Pixel);
						}
					}
				}

				return (mapBitmaps[(map, paletteIdx)] = bitmap);
			}
		}

		public void DrawEventOverlay(Graphics g, Map map, IEventObject eventObject, int zoom)
		{
			g.SmoothingMode = SmoothingMode.None;
			g.TextContrast = 0;

			int step = (16 * zoom);
			using (var font = new Font(UIHelpers.PrivateFontCollection.Families[0], 7.0f))
			{
				foreach (var warp in map.SecondaryHeader.Warps)
				{
					var rect = new Rectangle(warp.X * step, warp.Y * step, step, step);
					g.FillRectangle(warpBrush, rect);
					if (zoom > 1)
						g.DrawString($"{warp.TargetMapGroup:D2}:{warp.TargetMapID:D2}\n{warp.TargetWarpIndex:D2}", font, Brushes.White, new Point(rect.X, rect.Y));
					if (warp == eventObject)
						g.DrawRectangle(Pens.Red, rect);
				}

				foreach (var sign in map.SecondaryHeader.Signs)
				{
					var rect = new Rectangle(sign.X * step, sign.Y * step, step, step);
					g.FillRectangle(signBrush, rect);
					if (zoom > 1)
						g.DrawString($"{sign.TextID:D2}\n{sign.Unknown:D2}", font, Brushes.White, new Point(rect.X, rect.Y));
					if (sign == eventObject)
						g.DrawRectangle(Pens.Red, rect);
				}

				foreach (var npc in map.SecondaryHeader.NPCs)
				{
					var rect = new Rectangle((npc.X - 4) * step, (npc.Y - 4) * step, step, step);
					g.FillRectangle(npcBrush, rect);
					if (zoom > 1)
						g.DrawString($"{npc.Sprite:D2}\n(...)", font, Brushes.White, new Point(rect.X, rect.Y));
					if (npc == eventObject)
						g.DrawRectangle(Pens.Red, rect);
				}
			}
		}

		public void DrawDebugWarpOverlay(Graphics g, Map map, int zoom)
		{
			g.SmoothingMode = SmoothingMode.None;
			g.TextContrast = 0;

			int step = (16 * zoom);
			using (var font = new Font(UIHelpers.PrivateFontCollection.Families[0], 7.0f))
			{
				foreach (var (Name, Target) in gameHandler.DebugWarps)
				{
					if (Target.MapGroup != (map.ParentGroup + 1) || (Array.IndexOf(gameHandler.Maps[map.ParentGroup], map) + 1) != Target.MapID) continue;

					var rect = new Rectangle(Target.X * step, Target.Y * step, step, step);
					g.FillRectangle(debugWarpBrush, rect);
				}
			}
		}

		public void DrawTilesetOverlay(Graphics g, int x, int y, int zoom)
		{
			g.SmoothingMode = SmoothingMode.None;
			g.TextContrast = 0;

			int step = (Tileset.TileDimensions * zoom);
			using (var font = new Font(UIHelpers.PrivateFontCollection.Families[0], 7.0f))
			{
				for (int i = 0; i < 96; i++)
				{
					var rect = new Rectangle(x + ((i % 16) * step), y + ((i / 16) * step), step, step);
					g.FillRectangle(genericGrayBrush, rect);
					g.DrawString($"{i:X2}", font, Brushes.White, new Point(rect.X, rect.Y));
				}
			}
		}

		public void DrawCollisionOverlay(Graphics g, int x, int y, byte tilesetIdx, int widthInBlocks, int zoom, bool useEarlyMapping)
		{
			var typeDict = (useEarlyMapping ? collisionTypesEarly : collisionTypesUsed);

			Tileset tileset = gameHandler.Tilesets[tilesetIdx];

			g.SmoothingMode = SmoothingMode.None;
			g.TextContrast = 0;

			int step = (Tileset.CollisionDimensions * zoom);
			using (var font = new Font(UIHelpers.PrivateFontCollection.Families[0], 7.0f))
			{
				for (int b = 0, bn = 0; b < tileset.CollisionData.Length; b += 4, bn++)
				{
					for (int c = 0; c < 4; c++)
					{
						var data = tileset.CollisionData[b + c];

						int cy = (((bn / widthInBlocks) * (step * 2)) + ((c / 2) * step));
						int cx = (((bn % widthInBlocks) * (step * 2)) + ((c % 2) * step));

						var colBrush = (typeDict.ContainsKey(data) ? typeDict[data].Brush : genericBlackBrush);

						g.FillRectangle(colBrush, new Rectangle(x + cx, y + cy, step, step));
						g.DrawString($"{data:X2}", font, Brushes.White, new Point(x + cx, y + cy));
						if (typeDict.ContainsKey(data))
							g.DrawString($"{typeDict[data].Name}", font, Brushes.White, new Point(x + cx, y + cy + 8));
					}
				}
			}
		}

		public void DrawGridOverlay(Graphics g, Point location, Size size, int gridSize, Pen pen)
		{
			g.PixelOffsetMode = PixelOffsetMode.None;
			for (int gy = 0; gy < size.Height; gy += gridSize)
				for (int gx = 0; gx < size.Width; gx += gridSize)
					g.DrawRectangle(pen, new Rectangle(location.X + gx, location.Y + gy, gridSize - 1, gridSize - 1));
		}
	}
}
