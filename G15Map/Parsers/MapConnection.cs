using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace G15Map.Parsers
{
	public class MapConnection
	{
		public byte[] Data { get; private set; }    // *12

		public MapConnection(BinaryReader reader)
		{
			Data = reader.ReadBytes(12);
		}
	}
}
