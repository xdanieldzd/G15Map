using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace G15Map
{
	public class SelectablePanel : Panel
	{
		public bool DisableMouseWheel { get; set; }

		public SelectablePanel()
		{
			SetStyle(ControlStyles.Selectable, true);
			TabStop = true;
			DisableMouseWheel = false;
		}

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

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Focus();
			base.OnMouseDown(e);
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if (keyData == Keys.Up || keyData == Keys.Down) return true;
			if (keyData == Keys.Left || keyData == Keys.Right) return true;
			return base.IsInputKey(keyData);
		}

		protected override void OnEnter(EventArgs e)
		{
			Invalidate();
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			Invalidate();
			base.OnLeave(e);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (DisableMouseWheel) return;

			Invalidate();
			base.OnMouseWheel(e);
		}

		protected override void OnScroll(ScrollEventArgs se)
		{
			Invalidate();
			base.OnScroll(se);
		}
	}
}
