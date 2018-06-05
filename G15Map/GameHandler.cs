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
		public const uint TilesetOffset = 0xC88D;
		public const int NumTilesets = 0x1C;

		public const uint MapGroupsListOffset = 0x10001;
		public const uint MapGroupPaletteListOffset = 0x954F;
		public const int NumMapGroups = 0x10;

		public const uint PalettesOffset = 0x9C4C;
		public const int NumPalettes = 0x3A;

		//

		public Tileset[] Tilesets { get; private set; }
		public Map[][] Maps { get; private set; }
		public byte[] MapGroupPalettes { get; private set; }
		public Palette[] Palettes { get; private set; }

		//

		public GameHandler(BinaryReader reader)
		{
			reader.BaseStream.Position = TilesetOffset;
			Tilesets = new Tileset[NumTilesets];
			for (int i = 0; i < Tilesets.Length; i++)
				Tilesets[i] = new Tileset(reader, (i < 0x09 || i > 0x18));

			reader.BaseStream.Position = MapGroupsListOffset;
			var mapGroupPointers = new ushort[NumMapGroups];
			for (int i = 0; i < mapGroupPointers.Length; i++)
				mapGroupPointers[i] = reader.ReadUInt16();

			reader.BaseStream.Position = MapGroupPaletteListOffset;
			MapGroupPalettes = new byte[NumMapGroups];
			for (int i = 0; i < MapGroupPalettes.Length; i++)
				MapGroupPalettes[i] = reader.ReadByte();

			Maps = new Map[NumMapGroups][];
			for (int i = 0; i < mapGroupPointers.Length; i++)
			{
				var currentStart = Helpers.CalculateOffset((byte)(MapGroupsListOffset >> 14), mapGroupPointers[i]);
				var nextStart = ((i + 1) < mapGroupPointers.Length ? Helpers.CalculateOffset((byte)(MapGroupsListOffset >> 14), mapGroupPointers[i + 1]) : uint.MaxValue);

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
			Palettes = new Palette[NumPalettes];
			for (int i = 0; i < Palettes.Length; i++)
				Palettes[i] = new Palette(reader);
		}
	}
}
