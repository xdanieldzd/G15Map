using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace G15Map
{
	public class SelectablePanel : Panel
	{
		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
		private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

		public bool DisableMouseWheel { get; set; }
		public bool DisableSmoothScrolling { get; set; }
		public bool AlwaysShowHorizontalScroll { get; set; }
		public bool AlwaysShowVerticalScroll { get; set; }

		public SelectablePanel()
		{
			SetStyle(ControlStyles.Selectable | ControlStyles.ResizeRedraw, true);

			TabStop = true;
			DisableMouseWheel = false;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				var cp = base.CreateParams;
				cp.Style |= ((AlwaysShowHorizontalScroll ? 0x00100000 : 0x0) | (AlwaysShowVerticalScroll ? 0x00200000 : 0x0));
				cp.ExStyle |= 0x02000000;
				return cp;
			}
		}

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

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (DisableMouseWheel) return;

			Invalidate();
			base.OnMouseWheel(e);
		}

		protected override void OnScroll(ScrollEventArgs se)
		{
			if (!DisableSmoothScrolling)
				base.OnScroll(se);
			else
			{
				if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
					HandleRatchetScrolling(VerticalScroll, se);
				else if (se.ScrollOrientation == ScrollOrientation.HorizontalScroll)
					HandleRatchetScrolling(HorizontalScroll, se);
			}

			Invalidate();
		}

		private void HandleRatchetScrolling(ScrollProperties scrollProperties, ScrollEventArgs se)
		{
			if (se.Type == ScrollEventType.ThumbPosition)
			{
				scrollProperties.Value = se.OldValue;
			}
			else if (se.Type == ScrollEventType.ThumbTrack)
			{
				int newStep = (se.NewValue / scrollProperties.SmallChange);
				int oldStep = (se.OldValue / scrollProperties.SmallChange);

				if (newStep != oldStep)
					scrollProperties.Value = (se.NewValue / scrollProperties.SmallChange) * scrollProperties.SmallChange;
				else
					scrollProperties.Value = se.OldValue;
			}
			else
				base.OnScroll(se);
		}

		public override void Refresh()
		{
			HandleScrollbarDisplay();
			base.Refresh();
		}

		private void HandleScrollbarDisplay()
		{
			if (AlwaysShowVerticalScroll || AlwaysShowHorizontalScroll)
			{
				SuspendLayout();
				bool autoScrollTemp = AutoScroll;
				AutoScroll = false;

				if (AlwaysShowVerticalScroll)
				{
					if (!VerticalScroll.Visible) VerticalScroll.Visible = true;
					VerticalScroll.Enabled = (DisplayRectangle.Height > ClientRectangle.Height);
				}

				if (AlwaysShowHorizontalScroll)
				{
					if (!HorizontalScroll.Visible) HorizontalScroll.Visible = true;
					HorizontalScroll.Enabled = (DisplayRectangle.Width > ClientRectangle.Width);
				}

				AutoScroll = autoScrollTemp;
				ResumeLayout();
			}
		}
	}
}
