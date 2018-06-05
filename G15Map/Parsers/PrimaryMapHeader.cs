using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers
{
	[Flags]
	public enum MapConnectionDirections
	{
		None = 0,
		East = (1 << 0),
		West = (1 << 1),
		South = (1 << 2),
		North = (1 << 3)
	}

	public class PrimaryMapHeader
	{
		public byte Height { get; private set; }
		public byte Width { get; private set; }
		public ushort MapDataPointer { get; private set; }
		public ushort SomeImportantPointer { get; private set; }
		public ushort MaybeScriptPointer { get; private set; }
		public ushort SecondaryHeaderPointer { get; private set; }
		public byte ConnectionByte { get; private set; }
		public MapConnectionDirections ConnectionDirections { get; private set; }
		public MapConnection[] ConnectionData { get; private set; }

		public PrimaryMapHeader(BinaryReader reader)
		{
			Height = reader.ReadByte();
			Width = reader.ReadByte();
			MapDataPointer = reader.ReadUInt16();
			SomeImportantPointer = reader.ReadUInt16();
			MaybeScriptPointer = reader.ReadUInt16();
			SecondaryHeaderPointer = reader.ReadUInt16();
			ConnectionByte = reader.ReadByte();

			ConnectionDirections = (MapConnectionDirections)(ConnectionByte & 0x0F);

			ConnectionData = new MapConnection[4];
			for (int i = 0; i < ConnectionData.Length; i++)
			{
				if ((ConnectionByte & (1 << i)) != 0)
					ConnectionData[i] = new MapConnection(reader);
			}
		}
	}
}
