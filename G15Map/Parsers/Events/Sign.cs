using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers.Events
{
	public class Sign : IEventObject
	{
		public byte Y { get; private set; }
		public byte X { get; private set; }
		public byte Unknown { get; private set; }
		public byte TextID { get; private set; }

		public Sign(BinaryReader reader)
		{
			Y = reader.ReadByte();
			X = reader.ReadByte();
			Unknown = reader.ReadByte();
			TextID = reader.ReadByte();
		}
	}
}
