using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace G15Map.Parsers
{
	public class Tileset
	{
		// TODO: common part for *outdoor tilesets* at 0x33C00, 0x200 bytes, for tiles 0x00-0x1F!
		// NOTES: 00:2D51 -> pointer to common tiles (007C -> xx:3C00)

		// https://github.com/pret/pokegold-spaceworld/blob/ece3adc39bad4f6bbd041a83ace7acb21dc586b7/home/map.asm#L1517
		// 00:2D29 -> reg A == map type, if A==(01,02) loadcommon; 00:2D31 -> reg A == tileset ID, if A==1B loadcommon

		// 23:401F (0x8C01F) -> animation data; 0xFC bytes, 4 bytes per entry: [RAM address 2b][function address 2b]

		//  (addresses might be incorrect as anim *starts*)
		//  23:401F: animated water (tile 03) + animated flowers (tile 38)
		//  23:4023: animated flowers
		//  23:4027: still water + animated flowers
		//  23:402B: "" (diff frame?)
		//  23:402F: "" (diff frame?)
		//  23:4033: "" (diff frame?)
		//  23:4037: "" (diff frame?)
		//  23:403B: still water + still flowers
		//  23:403F: "" (diff frame?)
		//  23:4043: "" (diff frame?)
		//  23:4047: "" (diff frame?)
		//  23:404B: animated water (L/R) + still flowers
		//  23:4053: still water + still flowers
		//  23:4057: ""
		//  23:405B: ""
		//  23:405F: ""
		//  ...
		//  23:4077: (animated water + tile 53; waterfalls?)
		//  ...
		//  23:40A7: (animated water + tile 54; conveyor belts?)
		//  ...
		//  23:40BB: (animated water + tile 54; conveyor belts?)
		//  ...
		//  23:40CB: (animated tile 54)
		//  ...
		//  23:40DF: (animated tile 4F)
		//  ...

		public const int BlockDataSize = 0x800;
		public const int TileDataSize = 0x600;
		public const int CollisionDataSize = 0x400;    // TODO verify

		public const int TileDimensions = 8;
		public const int CollisionDimensions = 16;
		public const int BlockDimensions = 32;

		public byte Bank { get; private set; }
		public ushort BlockDataPointer { get; private set; }
		public ushort TileDataPointer { get; private set; }
		public ushort CollisionDataPointer { get; private set; }
		public ushort AnimationDataPointer { get; private set; }
		public byte Unknown0x09 { get; private set; }
		public byte Unknown0x0A { get; private set; }

		public byte[] BlockData { get; private set; }
		public byte[] TileData { get; private set; }
		public byte[] CollisionData { get; private set; }

		public Tileset(BinaryReader reader, bool hackReadCommonTiles)
		{
			Bank = reader.ReadByte();
			BlockDataPointer = reader.ReadUInt16();
			TileDataPointer = reader.ReadUInt16();
			CollisionDataPointer = reader.ReadUInt16();
			AnimationDataPointer = reader.ReadUInt16();
			Unknown0x09 = reader.ReadByte();
			Unknown0x0A = reader.ReadByte();

			long positionBackup = reader.BaseStream.Position;

			reader.BaseStream.Position = GameHelpers.CalculateOffset(Bank, BlockDataPointer);
			BlockData = reader.ReadBytes(BlockDataSize);
			if (hackReadCommonTiles)
			{
				TileData = new byte[TileDataSize];
				reader.BaseStream.Position = 0x33C00;
				Buffer.BlockCopy(reader.ReadBytes(0x200), 0, TileData, 0, 0x200);
				reader.BaseStream.Position = GameHelpers.CalculateOffset(Bank, TileDataPointer);
				Buffer.BlockCopy(reader.ReadBytes(TileDataSize - 0x200), 0, TileData, 0x200, (TileDataSize - 0x200));
			}
			else
			{
				reader.BaseStream.Position = GameHelpers.CalculateOffset(Bank, TileDataPointer);
				TileData = reader.ReadBytes(TileDataSize);
			}
			reader.BaseStream.Position = GameHelpers.CalculateOffset(Bank, CollisionDataPointer);
			CollisionData = reader.ReadBytes(CollisionDataSize);

			reader.BaseStream.Position = positionBackup;
		}
	}
}
