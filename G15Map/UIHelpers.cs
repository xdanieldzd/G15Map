using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Drawing.Text;

namespace G15Map
{
	public static class UIHelpers
	{
		/* https://stackoverflow.com/a/23658552 */

		public static PrivateFontCollection PrivateFontCollection { get; private set; }

		static UIHelpers()
		{
			PrivateFontCollection = new PrivateFontCollection();
		}

		public static void AddFontFromResource(string fontResourceName)
		{
			var fontBytes = GetFontResourceBytes(typeof(Program).Assembly, fontResourceName);
			var fontData = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontBytes.Length);
			System.Runtime.InteropServices.Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
			PrivateFontCollection.AddMemoryFont(fontData, fontBytes.Length);
			System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontData);
		}

		private static byte[] GetFontResourceBytes(Assembly assembly, string fontResourceName)
		{
			var resourceStream = assembly.GetManifestResourceStream(fontResourceName);
			if (resourceStream == null)
				throw new Exception(string.Format("Unable to find font '{0}' in embedded resources.", fontResourceName));
			var fontBytes = new byte[resourceStream.Length];
			resourceStream.Read(fontBytes, 0, (int)resourceStream.Length);
			resourceStream.Close();
			return fontBytes;
		}
	}
}
