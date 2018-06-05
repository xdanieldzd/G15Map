using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers
{
	public class SecondaryMapHeader
	{
		public ushort SomePointerBackwards { get; private set; }    //?? 0040 for Silent Hill, i.e start of bank?

		public byte NumWarps { get; private set; }
		public Warp[] Warps { get; private set; }

		public byte NumSigns { get; private set; }
		public Sign[] Signs { get; private set; }

		public byte NumNPCs { get; private set; }
		public NPC[] NPCs { get; private set; }

		public SecondaryMapHeader(BinaryReader reader)
		{
			SomePointerBackwards = reader.ReadUInt16();

			Warps = Enumerable.Range(0, NumWarps = reader.ReadByte()).Select(x => new Warp(reader)).ToArray();
			Signs = Enumerable.Range(0, NumSigns = reader.ReadByte()).Select(x => new Sign(reader)).ToArray();
			NPCs = Enumerable.Range(0, NumNPCs = reader.ReadByte()).Select(x => new NPC(reader)).ToArray();
		}
	}
}
