﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace G15Map
{
	public class TreeViewEx : TreeView
	{
		public TreeViewEx() : base() { }

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
		public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
			{
				SetWindowTheme(Handle, "explorer", null);
			}
		}
	}
}
