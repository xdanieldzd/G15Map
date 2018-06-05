using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G15Map
{
	public partial class TilesetForm : Form
	{
		GameHandler gameHandler;
		GameDrawing gameDrawing;

		public TilesetForm()
		{
			InitializeComponent();
		}

		public void Initialize(GameHandler gameHandler, GameDrawing gameDrawing)
		{
			this.gameHandler = gameHandler;
			this.gameDrawing = gameDrawing;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
