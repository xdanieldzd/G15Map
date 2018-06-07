using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers
{
	public class TileAnimation
	{
		public ushort DataPointer { get; private set; }
		public ushort FunctionPointer { get; private set; }

		public TileAnimation(BinaryReader reader)
		{
			DataPointer = reader.ReadUInt16();
			FunctionPointer = reader.ReadUInt16();
		}
	}
}
