using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace G15Map.Parsers
{
	public class Palette
	{
		public Color[] Colors { get; private set; }

		public Palette(BinaryReader reader)
		{
			Colors = new Color[4];
			for (int i = 0; i < Colors.Length; i++)
			{
				var value = reader.ReadUInt16();
				var r = ((value >> 0) & 0x1F);
				var g = ((value >> 5) & 0x1F);
				var b = ((value >> 10) & 0x1F);
				Colors[i] = Color.FromArgb(((r << 3) | (r >> 2)), ((g << 3) | (g >> 2)), ((b << 3) | (b >> 2)));
			}
		}
	}
}
