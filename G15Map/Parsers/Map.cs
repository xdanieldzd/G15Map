using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers
{
	public class Map
	{
		public byte RomBank { get; private set; }
		public byte Tileset { get; private set; }
		public byte Type { get; private set; }
		public ushort HeaderPointer { get; private set; }
		public byte Location { get; private set; }
		public byte Music { get; private set; }
		public byte ZeroPadding { get; private set; }

		public bool IsValid { get { return (ZeroPadding == 0); } }

		public PrimaryMapHeader PrimaryHeader { get; private set; }
		public SecondaryMapHeader SecondaryHeader { get; private set; }

		public byte ParentGroup { get; private set; }
		public byte[] MapData { get; private set; }

		public Map(BinaryReader reader, byte mapGroup)
		{
			ParentGroup = mapGroup;

			RomBank = reader.ReadByte();
			Tileset = reader.ReadByte();
			Type = reader.ReadByte();
			HeaderPointer = reader.ReadUInt16();
			Location = reader.ReadByte();
			Music = reader.ReadByte();
			ZeroPadding = reader.ReadByte();

			if (!IsValid)
				return;

			long positionBackup = reader.BaseStream.Position;

			reader.BaseStream.Position = Helpers.CalculateOffset(RomBank, HeaderPointer);
			PrimaryHeader = new PrimaryMapHeader(reader);

			reader.BaseStream.Position = Helpers.CalculateOffset((byte)(reader.BaseStream.Position >> 14), PrimaryHeader.SecondaryHeaderPointer);
			SecondaryHeader = new SecondaryMapHeader(reader);

			reader.BaseStream.Position = Helpers.CalculateOffset((byte)(reader.BaseStream.Position >> 14), PrimaryHeader.MapDataPointer);
			MapData = reader.ReadBytes(PrimaryHeader.Width * PrimaryHeader.Height);

			reader.BaseStream.Position = positionBackup;
		}
	}
}
