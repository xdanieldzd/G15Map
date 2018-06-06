using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers.Events
{
	public class NPC : IEventObject
	{
		public byte Sprite { get; private set; }
		public byte Y { get; private set; }
		public byte X { get; private set; }
		public byte Unknown1 { get; private set; }
		public byte Unknown2 { get; private set; }
		public ushort Unknown3 { get; private set; }    // ex. FFFF
		public ushort Unknown4 { get; private set; }    // ex. 0000
		public ushort Unknown5 { get; private set; }    // ex. 0000
		public ushort Unknown6 { get; private set; }    // ex. 0000

		public NPC(BinaryReader reader)
		{
			Sprite = reader.ReadByte();
			Y = reader.ReadByte();
			X = reader.ReadByte();
			Unknown1 = reader.ReadByte();
			Unknown2 = reader.ReadByte();
			Unknown3 = reader.ReadUInt16();
			Unknown4 = reader.ReadUInt16();
			Unknown5 = reader.ReadUInt16();
			Unknown6 = reader.ReadUInt16();
		}
	}
}
