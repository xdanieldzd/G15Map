using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers
{
	public class DebugWarpTarget
	{
		public byte MapGroup { get; private set; }
		public byte MapID { get; private set; }
		public byte Y { get; private set; }     // ^-
		public byte X { get; private set; }     // V- TODO verify order!

		public DebugWarpTarget(BinaryReader reader)
		{
			MapGroup = reader.ReadByte();
			MapID = reader.ReadByte();
			Y = reader.ReadByte();
			X = reader.ReadByte();
		}
	}
}
