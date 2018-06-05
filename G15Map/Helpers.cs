using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace G15Map
{
	public class Helpers
	{
		public static uint CalculateOffset(byte bank, ushort pointer) => (uint)((bank << 14) | (ushort)((((pointer >> 8) & 0x3F) << 8) | (pointer & 0xFF)));
		public static byte CalculateBank(uint offset) => (byte)(offset >> 14);
	}
}
