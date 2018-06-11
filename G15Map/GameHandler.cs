using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using G15Map.Parsers;

namespace G15Map
{
	public class GameHandler
	{
		static readonly Dictionary<byte, string> textTable = new Dictionary<byte, string>()
		{
			{ 0x05, "ガ" },
			{ 0x06, "ギ" },
			{ 0x07, "グ" },
			{ 0x08, "ゲ" },
			{ 0x09, "ゴ" },
			{ 0x0A, "ザ" },
			{ 0x0B, "ジ" },
			{ 0x0C, "ズ" },
			{ 0x0D, "ゼ" },
			{ 0x0E, "ゾ" },
			{ 0x0F, "ダ" },
			{ 0x10, "ヂ" },
			{ 0x11, "ヅ" },
			{ 0x12, "デ" },
			{ 0x13, "ド" },

			{ 0x19, "バ" },
			{ 0x1A, "ビ" },
			{ 0x1B, "ブ" },
			{ 0x1C, "ボ" },

			{ 0x26, "が" },
			{ 0x27, "ぎ" },
			{ 0x28, "ぐ" },
			{ 0x29, "げ" },
			{ 0x2A, "ご" },
			{ 0x2B, "ざ" },
			{ 0x2C, "じ" },
			{ 0x2D, "ず" },
			{ 0x2E, "ぜ" },
			{ 0x2F, "ぞ" },
			{ 0x30, "だ" },
			{ 0x31, "ぢ" },
			{ 0x32, "づ" },
			{ 0x33, "で" },
			{ 0x34, "ど" },
			{ 0x35, "ばん　どうろ" },

			{ 0x3A, "ば" },
			{ 0x3B, "び" },
			{ 0x3C, "ぶ" },
			{ 0x3D, "べ" },
			{ 0x3E, "ぼ" },

			{ 0x40, "パ" },
			{ 0x41, "ピ" },
			{ 0x42, "プ" },
			{ 0x43, "ポ" },
			{ 0x44, "ぱ" },
			{ 0x45, "ぴ" },
			{ 0x46, "ぷ" },
			{ 0x47, "ぺ" },
			{ 0x48, "ぽ" },

			{ 0x4F, "\n" },

			{ 0x51, "\n" },
			{ 0x54, "ポケモン" },

			{ 0x7F, " " },
			{ 0x80, "ア" },
			{ 0x81, "イ" },
			{ 0x82, "ウ" },
			{ 0x83, "エ" },
			{ 0x84, "オ" },
			{ 0x85, "カ" },
			{ 0x86, "キ" },
			{ 0x87, "ク" },
			{ 0x88, "ケ" },
			{ 0x89, "コ" },
			{ 0x8A, "サ" },
			{ 0x8B, "シ" },
			{ 0x8C, "ス" },
			{ 0x8D, "セ" },
			{ 0x8E, "ソ" },
			{ 0x8F, "タ" },
			{ 0x90, "チ" },
			{ 0x91, "ツ" },
			{ 0x92, "テ" },
			{ 0x93, "ト" },
			{ 0x94, "ナ" },
			{ 0x95, "ニ" },
			{ 0x96, "ヌ" },
			{ 0x97, "ネ" },
			{ 0x98, "ノ" },
			{ 0x99, "ハ" },
			{ 0x9A, "ヒ" },
			{ 0x9B, "フ" },
			{ 0x9C, "ホ" },
			{ 0x9D, "マ" },
			{ 0x9E, "ミ" },
			{ 0x9F, "ム" },
			{ 0xA0, "メ" },
			{ 0xA1, "モ" },
			{ 0xA2, "ヤ" },
			{ 0xA3, "ユ" },
			{ 0xA4, "ヨ" },
			{ 0xA5, "ラ" },
			{ 0xA6, "ル" },
			{ 0xA7, "レ" },
			{ 0xA8, "ロ" },
			{ 0xA9, "ワ" },
			{ 0xAA, "ヲ" },
			{ 0xAB, "ン" },
			{ 0xAC, "ッ" },
			{ 0xAD, "ャ" },
			{ 0xAE, "ュ" },
			{ 0xAF, "ョ" },
			{ 0xB0, "ィ" },
			{ 0xB1, "あ" },
			{ 0xB2, "い" },
			{ 0xB3, "う" },
			{ 0xB4, "え" },
			{ 0xB5, "お" },
			{ 0xB6, "か" },
			{ 0xB7, "き" },
			{ 0xB8, "く" },
			{ 0xB9, "け" },
			{ 0xBA, "こ" },
			{ 0xBB, "さ" },
			{ 0xBC, "し" },
			{ 0xBD, "す" },
			{ 0xBE, "せ" },
			{ 0xBF, "そ" },
			{ 0xC0, "た" },
			{ 0xC1, "ち" },
			{ 0xC2, "つ" },
			{ 0xC3, "て" },
			{ 0xC4, "と" },
			{ 0xC5, "な" },
			{ 0xC6, "に" },
			{ 0xC7, "ぬ" },
			{ 0xC8, "ね" },
			{ 0xC9, "の" },
			{ 0xCA, "は" },
			{ 0xCB, "ひ" },
			{ 0xCC, "ふ" },
			{ 0xCD, "へ" },
			{ 0xCE, "ほ" },
			{ 0xCF, "ま" },
			{ 0xD0, "み" },
			{ 0xD1, "む" },
			{ 0xD2, "め" },
			{ 0xD3, "も" },
			{ 0xD4, "や" },
			{ 0xD5, "ゆ" },
			{ 0xD6, "よ" },
			{ 0xD7, "ら" },
			{ 0xD8, "り" },
			{ 0xD9, "る" },
			{ 0xDA, "れ" },
			{ 0xDB, "ろ" },
			{ 0xDC, "わ" },
			{ 0xDD, "を" },
			{ 0xDE, "ん" },
			{ 0xDF, "っ" },
			{ 0xE0, "ゃ" },
			{ 0xE1, "ゅ" },
			{ 0xE2, "ょ" },
			{ 0xE3, "ー" },
			{ 0xE4, "゜" },
			{ 0xE5, "゛" },
			{ 0xE6, "？" },
			{ 0xE7, "！" },
			{ 0xE8, "。" },
			{ 0xE9, "ァ" },
			{ 0xEA, "ゥ" },
			{ 0xEB, "ェ" },
			{ 0xEF, "♂" },
			{ 0xF0, "円" },
			{ 0xF1, "×" },
			{ 0xF2, "．" },
			{ 0xF3, "／" },
			{ 0xF4, "ォ" },
			{ 0xF5, "♀" },
			{ 0xF6, "０" },
			{ 0xF7, "１" },
			{ 0xF8, "２" },
			{ 0xF9, "３" },
			{ 0xFA, "４" },
			{ 0xFB, "５" },
			{ 0xFC, "６" },
			{ 0xFD, "７" },
			{ 0xFE, "８" },
			{ 0xFF, "９" },
		};

		// TODO: 00:2D51 -> pointer to common tiles (007C -> xx:3C00) -- use pointer instead of hardcoded address (tho bank is still missing)?
		public const uint CommonMapTilesOffset = 0x33C00;
		public const int CommonMapTilesSize = 0x200;

		public const uint TilesetOffset = 0xC88D;
		public const int NumTilesets = 0x1C;

		public const uint MapGroupsListOffset = 0x10001;
		public const uint MapGroupPaletteListOffset = 0x954F;
		public const int NumMapGroups = 0x10;

		public const uint PalettesOffset = 0x9C4C;
		public const int NumPalettes = 0x3A;

		public const uint DebugWarpTargetsOffset = 0xC7D5;
		public const uint DebugWarpNamesOffset = 0xFCAAF;
		public const int NumDebugWarps = 0x2D;

		public const uint TileAnimationOffset = 0x8C01F;
		public const int NumTileAnimation = 0x3F;

		public byte[] CommonMapTiles { get; private set; }
		public Tileset[] Tilesets { get; private set; }
		public Map[][] Maps { get; private set; }
		public byte[] MapGroupPalettes { get; private set; }
		public Palette[] Palettes { get; private set; }
		public DebugWarpTarget[] DebugWarpTargets { get; private set; }
		public string[] DebugWarpNames { get; private set; }
		public TileAnimation[] TileAnimations { get; private set; }

		public (string Name, DebugWarpTarget Target)[] DebugWarps { get; private set; }

		public GameHandler(BinaryReader reader)
		{
			reader.BaseStream.Position = CommonMapTilesOffset;
			CommonMapTiles = reader.ReadBytes(CommonMapTilesSize);

			reader.BaseStream.Position = TilesetOffset;
			Tilesets = Enumerable.Range(0, NumTilesets).Select(x => new Tileset(reader)).ToArray();

			reader.BaseStream.Position = MapGroupsListOffset;
			var mapGroupPointers = Enumerable.Range(0, NumMapGroups).Select(x => reader.ReadUInt16()).ToArray();

			reader.BaseStream.Position = MapGroupPaletteListOffset;
			MapGroupPalettes = Enumerable.Range(0, NumMapGroups).Select(x => reader.ReadByte()).ToArray();

			Maps = new Map[NumMapGroups][];
			for (int i = 0; i < mapGroupPointers.Length; i++)
			{
				var currentStart = GameHelpers.CalculateOffset((byte)(MapGroupsListOffset >> 14), mapGroupPointers[i]);
				var nextStart = ((i + 1) < mapGroupPointers.Length ? GameHelpers.CalculateOffset((byte)(MapGroupsListOffset >> 14), mapGroupPointers[i + 1]) : uint.MaxValue);

				reader.BaseStream.Position = currentStart;

				List<Map> currentMaps = new List<Map>();
				while (reader.BaseStream.Position >= currentStart && reader.BaseStream.Position < nextStart)
				{
					var map = new Map(reader, (byte)i);
					if (!map.IsValid) break;
					currentMaps.Add(map);
				}
				Maps[i] = currentMaps.ToArray();
			}

			reader.BaseStream.Position = PalettesOffset;
			Palettes = Enumerable.Range(0, NumPalettes).Select(x => new Palette(reader)).ToArray();

			reader.BaseStream.Position = DebugWarpTargetsOffset;
			DebugWarpTargets = Enumerable.Range(0, NumDebugWarps).Select(x => new DebugWarpTarget(reader)).ToArray();

			reader.BaseStream.Position = DebugWarpNamesOffset;
			DebugWarpNames = Enumerable.Range(0, NumDebugWarps).Select(x => ReadString(reader)).ToArray();

			DebugWarps = Enumerable.Range(0, NumDebugWarps).Select(x => (Name: DebugWarpNames[x], Target: DebugWarpTargets[x])).ToArray();

			reader.BaseStream.Position = TileAnimationOffset;
			TileAnimations = Enumerable.Range(0, NumTileAnimation).Select(x => new TileAnimation(reader)).ToArray();
		}

		private string ReadString(BinaryReader reader)
		{
			var count = 0;  // for sanity check
			var @string = new StringBuilder();
			var end = false;
			while (!end && count < 500)
			{
				var value = reader.ReadByte();
				if (value == 0x50)
					end = true;
				else
					@string.Append(textTable.ContainsKey(value) ? textTable[value] : $"[{value:X2}]");
				count++;
			}
			return @string.ToString();
		}
	}
}
