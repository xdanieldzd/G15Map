using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers.Events
{
	public class Warp : IEventObject
	{
		public long RomOffset { get; private set; }

		public byte Y { get; private set; }
		public byte X { get; private set; }
		public byte TargetWarpIndex { get; private set; }
		public byte TargetMapGroup { get; private set; }
		public byte TargetMapID { get; private set; }
		public ushort UnknownMaybePointer { get; private set; }

		public Warp(BinaryReader reader)
		{
			RomOffset = reader.BaseStream.Position;

			Y = reader.ReadByte();
			X = reader.ReadByte();
			TargetWarpIndex = reader.ReadByte();
			TargetMapGroup = reader.ReadByte();
			TargetMapID = reader.ReadByte();
			UnknownMaybePointer = reader.ReadUInt16();
		}
	}
}
