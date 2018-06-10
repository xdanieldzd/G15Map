using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G15Map.Parsers.Events
{
	public interface IEventObject
	{
		long RomOffset { get; }

		byte X { get; }
		byte Y { get; }
	}
}
